using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStore.Clients.Catalog;
using WebStore.Clients.Employees;
using WebStore.Clients.Orders;
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
        private readonly IConfiguration _configuration; //äëÿ îáðàùåíèÿ ê appsettings.json
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services) //ñþäà äîáàâëÿåì ñåðâèñû, êàêèå áóäåì èñïîëüçîâàòü
        {
            services.AddMvc();

            //(options => options.Filters.Add(new Example_SimpleActionFilter())) //äëÿ äîáàâëåíèÿ ôèëüòðà êî âñåì ìåòîäàì âñåõ êîíòðîëëåðîâ

            services.AddDbContext<DAL.WebStoreContext>(options => options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))); //ñòðîêà ïîäêëþ÷åíèÿ SQL


            //AUTHENTICATION AND AUTHORIZATION//

            services.AddIdentity<User, Role>()
               .AddEntityFrameworkStores<WebStoreContext>()
               .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>      //íàñòðîéêà òðåáîâàíèé ê ïàðîëþ, ëîãèíó èòä (íåîáÿçàòåëüíî)
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

            services.ConfigureApplicationCookie(options =>      //íàñòðîéêà êóêè è ïóòåé ê ñòðàíèöàì, îòíîñÿùèìñÿ ê àâòîðèçàöèè (íåîáÿçàòåëüíî)
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

            //AddSingleton âðåìÿ æèçíè ñåðâèñà = âðåìåíè æèçíè çàïóùåííîé ïðîãðàììû 
            //AddScoped ðàâíî âðåìåíè æèçíè http-çàïðîñà (äî îáíîâëåíèÿ/çàêðûòèÿ ñòðàíèöû)
            //AddTransient îáíîâëÿåòñÿ ïðè êàæäîì çàïðîñå

            //services.AddSingleton<IEmployeesService, InMemoryEmployeesService>(); //áûëî äî ñîçäàíèÿ êëèåíòà
            services.AddSingleton<IEmployeesService, EmployeesClient>();

            //services.AddSingleton<ICatalogData, InMemoryCatalogData>(); //áûëî äî ïîäêëþ÷åíèÿ ÁÄ
            //services.AddScoped<ICatalogData, SqlCatalogData>(); //ïîñëå ïîäêëþ÷åíèÿ ÁÄ (è ñòàëî AddScoped)
            services.AddScoped<ICatalogData, CatalogClient>(); //ïîñëå ðåàëèçàöèè WebAPI

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //äëÿ ðàáîòû ñëóæåáíîãî êëàññà HttpContextAccessor òàêæå íóæíî ïðîïèñûâàòü çàâèñèìîñòü
            services.AddScoped<ICartService, CookieCartService>(); //êîðçèíà - AddScoped!

            //services.AddScoped<ISqlOrderService, SqlOrderService>();
            services.AddScoped<ISqlOrderService, OrdersClient>();

            services.AddTransient<DbInitializer>();

            services.AddScoped<IValueServices, ValuesClient>(); //ðåãèñòðàöèÿ êëèåíòà êàê ñåðâèñ
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DbInitializer db) //çäåñü ïðîïèñûâàåì, ÷òî è êàê áóäåò èñïîëüçîâàòüñÿ (ñâÿçàíî ñ ñåðâèñàìè)
        {
            db.Initialize();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
            }

            app.UseStaticFiles();             //ïîäêëþ÷åíèå ñòàòè÷åñêèõ ðåñóðñîâ
            //app.UseDefaultFiles();

            app.UseRouting();

            app.UseAuthentication(); //ïîäêëþ÷àåì àóòåíòèôèêàöèþ (ïîñëå UseStaticFiles äëÿ âîçìîæíîñòè àíîíèìíîãî äîñòóïà ê ðåñóðñàì)
            app.UseAuthorization(); //ïîäêëþ÷àåì àâòîðèçàöèþ (ìåæäó UseRouting è UseEndpoints), íóæíî ïîäêëþ÷àòü òîëüêî åñëè èñïîëüçóþòñÿ àòðèáóòû [Authorize]

            app.UseEndpoints(endpoints =>
            {
                //ïóòè ïî-óìîë÷àíèþ ê ãëàâíûì ñòðàíèöàì
                endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");  //îáëàñòè
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{Id?}");              //êîíòðîëëåðû

                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Hello World!");
                //});
            });

            //àíàëîã app.UseEndpoints â îäíó ñòðîêó (íî áåç âîçìîæíîñòè ïîäðîáíî íàñòðîèòü ìàðøðóòû ïî-óìîë÷àíèþ)
            //app.UseMvcWithDefaultRoute();
            //êðîìå òîãî, â services.AddMvc òîãäà íàäî ïðîïèñàòü options => (options.EnableEndpointRouting = false)
        }
    }
}
