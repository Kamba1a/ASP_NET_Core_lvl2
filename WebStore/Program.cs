using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebStore.DAL;

namespace WebStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run(); //����������� ������

            //��������� ��������� ������ ��� ������� ������� ���������� ������� �� ���������� �������

            IWebHost webHost = BuildWebHost(args);

            //����������� � ����� ����������
            using (IServiceScope serviceScope = webHost.Services.CreateScope()) // ����� ��� ��������� DbContext
            {
                IServiceProvider serviceProvider = serviceScope.ServiceProvider;
                try
                {
                    WebStoreContext webStoreContext = serviceProvider.GetRequiredService<WebStoreContext>();
                    //��� ����������� ����� ���� ������ �� DbInitializer:
                    DbInitializer.Initialize(webStoreContext);
                    DbInitializer.InitializeUsers(serviceProvider);
                }
                catch (Exception ex)
                {
                    ILogger logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Oops. Something went wrong at DB initializing...");
                }
            }

        webHost.Run();
        }


        ////����������� �����
        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
        //    {
        //        //��������� ������ �� Startup.cs
        //        webBuilder.UseStartup<Startup>();
        //    });


        //����������� ����� �������� �� ������:
        private static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().Build();
    }
}
