using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStore.DAL;
using WebStore.Domain;
using WebStore.Infrastructure.Interfaces;
using WebStore.Infrastructure.Services;
using WebStore.Models;

namespace WebStore
{
    public class Startup
    {
        private readonly IConfiguration _configuration; //��� ��������� � appsettings.json
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //���� ��������� �������, ����� ����� ������������

            services.AddMvc();
            //(options => options.Filters.Add(new Example_SimpleActionFilter())) ��� ���������� ������� �� ���� ������� ���� ������������

            //������ ����������� SQL:
            services.AddDbContext<DAL.WebStoreContext>(options => options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));


        //Authentication and Authorization//
            //���������� ����
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<WebStoreContext>()
                .AddDefaultTokenProviders();

            //��������� ���������� � ������, ������ ��� (�������������)
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            //��������� ���� � ����� � ���������, ����������� � ����������� (�������������)
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                //options.Cookie.HttpOnly = true;
                //options.Cookie.Expiration = TimeSpan.FromDays(150);
                options.LoginPath = "/Account/Login"; // If the LoginPath is not set here, ASP.NET Core will default to /Account/Login
                options.LogoutPath = "/Account/Logout"; // If the LogoutPath is not set here, ASP.NET Core will default to /Account/Logout
                options.AccessDeniedPath = "/Account/AccessDenied"; // If the AccessDeniedPath is not set here, ASP.NET Core will default to /Account/AccessDenied
                options.SlidingExpiration = true;
            });

        //Dependency//
            //���������� �����������:
            //services.AddSingleton<IService, InMemoryService>(); ����� ����� ������� = ������� ����� ���������� ��������� 
            //services.AddScoped(); ����� ������� ����� http-������� (�� ����������/�������� ��������)
            //services.AddTransient(); - ����������� ��� ������ �������

            services.AddSingleton(typeof(IitemData<EmployeeViewModel>), typeof(InMemoryEmployeesData));
            services.AddSingleton(typeof(IitemData<BookViewModel>), typeof(InMemoryBooksData));

            //services.AddSingleton<ICatalogData, InMemoryCatalogData>(); //���� �� ����������� ��
            services.AddScoped<ICatalogData, SqlCatalogData>(); //����� ����������� �� (� ����� AddScoped)

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //��� ������ ���������� ������ HttpContextAccessor ����� ����� ����������� �����������
            services.AddScoped<ICartService, CookieCartService>(); //������� - AddScoped!
            services.AddScoped<ISqlOrderService, SqlOrderService>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //����� �����������, ��� � ��� ����� �������������� (������� � ���������)

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //����������� ����������� ��������
            app.UseStaticFiles();

            app.UseRouting();

            //���������� �������������� (����� ����������� ����������� ������ ��� ����������� ���������� ������� � ���)
            app.UseAuthentication();
            //���������� ����������� (������ ����� UseRouting � �� UseEndpoints)
            app.UseAuthorization(); //����� ���������� ������ ���� ������������ �������� [Authorize]

            app.UseEndpoints(endpoints =>
            {
                //�������
                endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                //���������� ��-��������� (��� �����������/��� ������/��������)
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{Id?}");

                endpoints.MapControllerRoute("controller1", "{controller=Employee}/{action=Employees}/{Id?}");

                endpoints.MapControllerRoute("controller2", "{controller=Book}/{action=Books}/{Id?}");

                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Hello World!");
                //});
            });

            //������ app.UseEndpoints � ���� ������ (�� ��� ����������� �������� ��������� �������� ��-���������)
            //app.UseMvcWithDefaultRoute();
            //����� ����, � services.AddMvc ����� ���� ��������� options => (options.EnableEndpointRouting = false)
        }
    }
}
