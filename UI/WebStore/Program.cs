using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using System;

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
                .ConfigureLogging((hostBuilderContext, loggingBuilder) =>    //пример управлени€ конфигурацией системы логгировани€ в коде (добавление провайдеров и фильтров сообщений)
                    {
                        //loggingBuilder.ClearProviders();  //очистить все провайдеры
                        //loggingBuilder.AddProvider(new ConsoleLoggerProvider(/*конфигураци€ провайдера*/));   //добавить провайдер
                        //loggingBuilder.AddConsole(opt => opt.IncludeScopes = true);   //консольный провайдер с включенными област€ми логгировани€
                        //loggingBuilder.AddEventLog(opt => opt.LogName = "WebStore");  //провайдер, записывающий информацию в системный журнал событий
                        ////loggingBuilder.AddFilter("System", LogLevel.Warning);     //фильтр только по Warning-сообщени€м пространства имен System
                        //loggingBuilder.AddFilter((category, level) =>
                        //{
                        //    if (category.StartsWith("Microsoft"))   //если пространство имен Microsoft (и вложенные в него)
                        //        return level >= LogLevel.Warning;   //будут выбиратьс€ сообщени€ уровн€ Warning и выше
                        //    return true;
                        //});
                    })
                .ConfigureWebHostDefaults(builder =>
                    builder.UseStartup<Startup>())
                .UseSerilog((hostBuilderContext, loggerConfiguration) => // пример конфигурации библиотеки дл€ логгировани€ Serilog в коде
                        loggerConfiguration.ReadFrom.Configuration(hostBuilderContext.Configuration) //считывание конфигурации из конфигурации hostBuilderContext
                       .MinimumLevel.Debug()    //минимальный уровень ведени€ журнала
                       .MinimumLevel.Override("Microsoft", LogEventLevel.Error) //переопределение минимального уровн€ дл€ конкретного пространства имен 
                       .Enrich.FromLogContext() //добавление в процесс логгировани€ информацию из контекста вход€щего запроса
                       .WriteTo.Console(        //вести логи в консоли 
                            outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}]{SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}") //формат записи логов
                       .WriteTo.RollingFile($@".\Logs\WebStore[{DateTime.Now:yyyy-MM-ddTHH-mm-ss}].log")    //вести логи в указанном файле
                       .WriteTo.File(new JsonFormatter(",", true), $@".\Logs\WebStore[{DateTime.Now:yyyy-MM-ddTHH-mm-ss}].log.json")); //вести логи в файле формата json
                       //.WriteTo.Seq("http://localhost:5341/"));     //ведение логов на сервере Seq по указанной Listen Uri (сайт https://datalust.co/seq)
    }
}