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
        private readonly IConfiguration _configuration; //для обращения к appsettings.json
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services) //сюда добавляем сервисы, какие будем использовать
        {
            services.AddMvc();

            //(options => options.Filters.Add(new Example_SimpleActionFilter())) //для добавления фильтра ко всем методам всех контроллеров

            services.AddDbContext<DAL.WebStoreContext>(options => options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))); //строка подключения SQL


            //AUTHENTICATION AND AUTHORIZATION//

            services.AddIdentity<User, Role>()
               .AddEntityFrameworkStores<WebStoreContext>()
               .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>      //настройка требований к паролю, логину итд (необязательно)
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

            services.ConfigureApplicationCookie(options =>      //настройка куки и путей к страницам, относящимся к авторизации (необязательно)
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

            //AddSingleton время жизни сервиса = времени жизни запущенной программы 
            //AddScoped равно времени жизни http-запроса (до обновления/закрытия страницы)
            //AddTransient обновляется при каждом запросе

            //services.AddSingleton<IEmployeesService, InMemoryEmployeesService>(); //было до создания клиента
            services.AddSingleton<IEmployeesService, EmployeesClient>();

            //services.AddSingleton<ICatalogData, InMemoryCatalogData>(); //было до подключения БД
            services.AddScoped<ICatalogData, SqlCatalogData>(); //после подключения БД (и стало AddScoped)

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //для работы служебного класса HttpContextAccessor также нужно прописывать зависимость
            services.AddScoped<ICartService, CookieCartService>(); //корзина - AddScoped!
            services.AddScoped<ISqlOrderService, SqlOrderService>();

            services.AddTransient<DbInitializer>();

            services.AddScoped<IValueServices, ValuesClient>(); //регистрация клиента как сервис
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DbInitializer db) //здесь прописываем, что и как будет использоваться (связано с сервисами)
        {
            db.Initialize();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
            }

            app.UseStaticFiles();             //подключение статических ресурсов
            //app.UseDefaultFiles();

            app.UseRouting();

            app.UseAuthentication(); //подключаем аутентификацию (после UseStaticFiles для возможности анонимного доступа к ресурсам)
            app.UseAuthorization(); //подключаем авторизацию (между UseRouting и UseEndpoints), нужно подключать только если используются атрибуты [Authorize]

            app.UseEndpoints(endpoints =>
            {
                //пути по-умолчанию к главным страницам
                endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");  //области
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{Id?}");              //контроллеры

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
