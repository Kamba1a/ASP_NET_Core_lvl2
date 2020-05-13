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
        private readonly IConfiguration _configuration; //дл€ обращени€ к appsettings.json
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //сюда добавл€ем сервисы, какие будем использовать

            services.AddMvc();
            //(options => options.Filters.Add(new Example_SimpleActionFilter())) дл€ добавлени€ фильтра ко всем методам всех контроллеров

            //строка подключени€ SQL:
            services.AddDbContext<DAL.WebStoreContext>(options => options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));


        //Authentication and Authorization//
            //добавление роли
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<WebStoreContext>()
                .AddDefaultTokenProviders();

            //настройка требований к паролю, логину итд (необ€зательно)
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

            //настройка куки и путей к страницам, относ€щимс€ к авторизации (необ€зательно)
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
            //–азрешение зависимости:
            //services.AddSingleton<IService, InMemoryService>(); врем€ жизни сервиса = времени жизни запущенной программы 
            //services.AddScoped(); равно времени жизни http-запроса (до обновлени€/закрыти€ страницы)
            //services.AddTransient(); - обновл€етс€ при каждом запросе

            services.AddSingleton(typeof(IitemData<EmployeeViewModel>), typeof(InMemoryEmployeesData));
            services.AddSingleton(typeof(IitemData<BookViewModel>), typeof(InMemoryBooksData));

            //services.AddSingleton<ICatalogData, InMemoryCatalogData>(); //было до подключени€ Ѕƒ
            services.AddScoped<ICatalogData, SqlCatalogData>(); //после подключени€ Ѕƒ (и стало AddScoped)

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //дл€ работы служебного класса HttpContextAccessor также нужно прописывать зависимость
            services.AddScoped<ICartService, CookieCartService>(); //корзина - AddScoped!
            services.AddScoped<ISqlOrderService, SqlOrderService>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //здесь прописываем, что и как будет использоватьс€ (св€зано с сервисами)

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //подключение статических ресурсов
            app.UseStaticFiles();

            app.UseRouting();

            //подключаем аутентификацию (после подключени€ статических файлов дл€ возможности анонимного доступа к ним)
            app.UseAuthentication();
            //подключаем авторизацию (именно после UseRouting и до UseEndpoints)
            app.UseAuthorization(); //нужно подключать только если используютс€ атрибуты [Authorize]

            app.UseEndpoints(endpoints =>
            {
                //области
                endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                //контроллер по-умолчанию (им€ контроллера/им€ метода/действие)
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{Id?}");

                endpoints.MapControllerRoute("controller1", "{controller=Employee}/{action=Employees}/{Id?}");

                endpoints.MapControllerRoute("controller2", "{controller=Book}/{action=Books}/{Id?}");

                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Hello World!");
                //});
            });

            //аналог app.UseEndpoints в одну строку (но без возможности подробно настроить маршруты по-умолчанию)
            //app.UseMvcWithDefaultRoute();
            //кроме того, в services.AddMvc тогда надо прописать options => (options.EnableEndpointRouting = false)
        }
    }
}
