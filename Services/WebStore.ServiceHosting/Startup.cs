using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using WebStore.DAL;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services;
using WebStore.Logger;
using WebStore.Services;
using WebStore.Services.Data;

namespace WebStore.ServiceHosting
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DAL.WebStoreContext>(options => options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<User, Role>().AddEntityFrameworkStores<WebStoreContext>().AddDefaultTokenProviders();
            services.AddTransient<DbInitializer>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 3;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 1;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = false;
            });

            services.AddSingleton<IEmployeesService, InMemoryEmployeesService>();

            services.AddScoped<ICatalogData, SqlCatalogData>();
            services.AddScoped<ICartStore, CookiesCartStore>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ISqlOrderService, SqlOrderService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddControllers();

            //настройка Swagger
            services.AddSwaggerGen(opt =>
            {
                //заголовок
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "WebStore.API", Version = "v1" });

                //расположене файлов документации, из которых swagger будет брать описание
                const string domain_doc_xml = "WebStore.Domain.xml";
                const string web_api_doc_xml = "WebStore.ServiceHosting.xml";
                const string debug_path = @"bin\debug\netcoreapp3.1";

                //файл документации из ServiceHosting просто подключаем
                opt.IncludeXmlComments(web_api_doc_xml);
                
                //файл документации из Domain проверяем где находится
                if (File.Exists(domain_doc_xml))    //если там же, где и exe-файл, то просто подключаем (если будет развернут на хостинге)
                    opt.IncludeXmlComments(domain_doc_xml);
                else if (File.Exists(Path.Combine(debug_path, domain_doc_xml))) //если в режиме отладки, то указываем путь в папку debug
                    opt.IncludeXmlComments(Path.Combine(debug_path, domain_doc_xml));
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DbInitializer dbInitializer, ILoggerFactory log)
        {
            log.AddLog4Net(); //подключение системы логгирования с помощью созданной нами инфраструктуры 

            dbInitializer.Initialize();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            //подключение Swagger
            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {
                //конечная точка, по которой будет доступна техническая документация по WebAPI (может быть использована для авто генерации клиентов)
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "WebStore.API"); //путь, название
                opt.RoutePrefix = string.Empty; //адрес, по которому доступен UI (Empty - интерфейс будет виден сразу)
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
