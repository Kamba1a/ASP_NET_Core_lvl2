using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStore.Clients.Employees;
using WebStore.Clients.Values;
using WebStore.DAL;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.Models;
using WebStore.Interfaces.Api;
using WebStore.Interfaces.Services;
using WebStore.Services;
using WebStore.Services.Data;

namespace WebStore
{
    public class Startup
    {
        private readonly IConfiguration _configuration; //��� ��������� � appsettings.json
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services) //���� ��������� �������, ����� ����� ������������
        {
            services.AddMvc();

            //(options => options.Filters.Add(new Example_SimpleActionFilter())) //��� ���������� ������� �� ���� ������� ���� ������������

            services.AddDbContext<DAL.WebStoreContext>(options => options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))); //������ ����������� SQL


            //AUTHENTICATION AND AUTHORIZATION//

            services.AddIdentity<User, Role>()
               .AddEntityFrameworkStores<WebStoreContext>()
               .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>      //��������� ���������� � ������, ������ ��� (�������������)
            {
                // Password settings
                options.Password.RequiredLength = 3;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = false;
                //options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCD...123457890";
            });

            services.ConfigureApplicationCookie(options =>      //��������� ���� � ����� � ���������, ����������� � ����������� (�������������)
            {
                // Cookie settings
                //options.Cookie.Name = "WebStore";
                //options.Cookie.HttpOnly = true;
                //options.Cookie.Expiration = TimeSpan.FromDays(10);
                options.SlidingExpiration = true;

                options.LoginPath = "/Account/Login"; // If the LoginPath is not set here, ASP.NET Core will default to /Account/Login
                options.LogoutPath = "/Account/Logout"; // If the LogoutPath is not set here, ASP.NET Core will default to /Account/Logout
                options.AccessDeniedPath = "/Account/AccessDenied"; // If the AccessDeniedPath is not set here, ASP.NET Core will default to /Account/AccessDenied
            });


            //DEPENDENCY//

            //AddSingleton ����� ����� ������� = ������� ����� ���������� ��������� 
            //AddScoped ����� ������� ����� http-������� (�� ����������/�������� ��������)
            //AddTransient ����������� ��� ������ �������

            //services.AddSingleton<IEmployeesService, InMemoryEmployeesService>(); //���� �� �������� �������
            services.AddSingleton<IEmployeesService, EmployeesClient>();

            //services.AddSingleton<ICatalogData, InMemoryCatalogData>(); //���� �� ����������� ��
            services.AddScoped<ICatalogData, SqlCatalogData>(); //����� ����������� �� (� ����� AddScoped)

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //��� ������ ���������� ������ HttpContextAccessor ����� ����� ����������� �����������
            services.AddScoped<ICartService, CookieCartService>(); //������� - AddScoped!
            services.AddScoped<ISqlOrderService, SqlOrderService>();

            services.AddTransient<DbInitializer>();

            services.AddScoped<IValueServices, ValuesClient>(); //����������� ������� ��� ������
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DbInitializer db) //����� �����������, ��� � ��� ����� �������������� (������� � ���������)
        {
            db.Initialize();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
            }

            app.UseStaticFiles();             //����������� ����������� ��������
            //app.UseDefaultFiles();

            app.UseRouting();

            app.UseAuthentication(); //���������� �������������� (����� UseStaticFiles ��� ����������� ���������� ������� � ��������)
            app.UseAuthorization(); //���������� ����������� (����� UseRouting � UseEndpoints), ����� ���������� ������ ���� ������������ �������� [Authorize]

            app.UseEndpoints(endpoints =>
            {
                //���� ��-��������� � ������� ���������
                endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");  //�������
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{Id?}");              //�����������

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
