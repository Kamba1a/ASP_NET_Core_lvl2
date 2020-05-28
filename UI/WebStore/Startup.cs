using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebStore.Clients.Catalog;
using WebStore.Clients.Employees;
using WebStore.Clients.Identity;
using WebStore.Clients.Orders;
using WebStore.Clients.Values;
using WebStore.Domain.Entities.Identity;
using WebStore.Infrastructure.Middleware;
using WebStore.Interfaces.Api;
using WebStore.Interfaces.Services;
using WebStore.Logger;
using WebStore.Services;

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

            //строка подключения к БД (для реализации WebAPI БД отключена от основного приложения)
            //services.AddDbContext<DAL.WebStoreContext>(options => options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));


            //AUTHENTICATION AND AUTHORIZATION//

            services.AddIdentity<User, Role>()
               //.AddEntityFrameworkStores<WebStoreContext>() //отключение Identity от БД в основном приложении (реализация WebAPI)
               .AddDefaultTokenProviders();

            #region WebAPI Identity clients stores
            //подключаем для работы Identity после отключения БД от основного приложения

            services
               .AddTransient<IUserStore<User>, UsersClient>()
               .AddTransient<IUserPasswordStore<User>, UsersClient>()
               .AddTransient<IUserEmailStore<User>, UsersClient>()
               .AddTransient<IUserPhoneNumberStore<User>, UsersClient>()
               .AddTransient<IUserTwoFactorStore<User>, UsersClient>()
               .AddTransient<IUserLockoutStore<User>, UsersClient>()
               .AddTransient<IUserClaimStore<User>, UsersClient>()
               .AddTransient<IUserLoginStore<User>, UsersClient>();
            services
               .AddTransient<IRoleStore<Role>, RolesClient>();

            #endregion

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
            //services.AddScoped<ICatalogData, SqlCatalogData>(); //после подключения БД (и стало AddScoped)
            services.AddScoped<ICatalogData, CatalogClient>(); //после реализации WebAPI

            services.AddScoped<ICartService, CartService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //для работы служебного класса HttpContextAccessor (используется в CookieCartStore) также нужно прописывать зависимость
            services.AddScoped<ICartStore, CookiesCartStore>();

            //services.AddScoped<ISqlOrderService, SqlOrderService>();
            services.AddScoped<ISqlOrderService, OrdersClient>();

            //регистрация класса для наполнения БД начальными данными (перенесено в Webstore.ServiceHosting после отключения БД от основного приложения)
            //services.AddTransient<DbInitializer>();

            services.AddScoped<IValueServices, ValuesClient>(); //регистрация клиента как сервис
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory log/*, DbInitializer db*/) //здесь прописываем, что и как будет использоваться (связано с сервисами)
        {
            log.AddLog4Net(); //подключение системы логгирования с помощью созданной нами инфраструктуры 

            //наполнение БД начальными данными (перенесено в Webstore.ServiceHosting после отключения БД от основного приложения)
            //db.Initialize();

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

            app.UseMiddleware<ErrorHandlingMiddleware>(); //подключение собственного промежуточного ПО, которое перехватывает ошибки, возникающие ниже по конвейеру запросов

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
