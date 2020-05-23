using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace WebStore
{
    public class Program
    {
        public static void Main(string[] args) =>
            CreateHostBuilder(args)
               .Build()
               .Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((hostBuilderContext, loggingBuilder)=>    //пример управления конфигурацией системы логгирования в коде (добавление провайдеров и фильтров сообщений)
                    {
                        //loggingBuilder.ClearProviders();  //очистить все провайдеры
                        //loggingBuilder.AddProvider(new ConsoleLoggerProvider(/*конфигурация провайдера*/));   //добавить провайдер
                        //loggingBuilder.AddConsole(opt => opt.IncludeScopes = true);   //консольный провайдер с включенными областями логгирования
                        //loggingBuilder.AddEventLog(opt => opt.LogName = "WebStore");  //провайдер, записывающий информацию в системный журнал событий
                        ////loggingBuilder.AddFilter("System", LogLevel.Warning);     //фильтр только по Warning-сообщениям пространства имен System
                        //loggingBuilder.AddFilter((category, level) =>
                        //{
                        //    if (category.StartsWith("Microsoft"))   //если пространство имен Microsoft (и вложенные в него)
                        //        return level >= LogLevel.Warning;   //будут выбираться сообщения уровня Warning и выше
                        //    return true;
                        //});
                    })
                .ConfigureWebHostDefaults(builder =>
                    builder.UseStartup<Startup>());
    }
}