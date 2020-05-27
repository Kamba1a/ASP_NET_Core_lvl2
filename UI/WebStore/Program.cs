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
                .ConfigureLogging((hostBuilderContext, loggingBuilder) =>    //������ ���������� ������������� ������� ������������ � ���� (���������� ����������� � �������� ���������)
                    {
                        //loggingBuilder.ClearProviders();  //�������� ��� ����������
                        //loggingBuilder.AddProvider(new ConsoleLoggerProvider(/*������������ ����������*/));   //�������� ���������
                        //loggingBuilder.AddConsole(opt => opt.IncludeScopes = true);   //���������� ��������� � ����������� ��������� ������������
                        //loggingBuilder.AddEventLog(opt => opt.LogName = "WebStore");  //���������, ������������ ���������� � ��������� ������ �������
                        ////loggingBuilder.AddFilter("System", LogLevel.Warning);     //������ ������ �� Warning-���������� ������������ ���� System
                        //loggingBuilder.AddFilter((category, level) =>
                        //{
                        //    if (category.StartsWith("Microsoft"))   //���� ������������ ���� Microsoft (� ��������� � ����)
                        //        return level >= LogLevel.Warning;   //����� ���������� ��������� ������ Warning � ����
                        //    return true;
                        //});
                    })
                .ConfigureWebHostDefaults(builder =>
                    builder.UseStartup<Startup>())
                .UseSerilog((hostBuilderContext, loggerConfiguration) => // ������ ������������ ���������� ��� ������������ Serilog � ����
                        loggerConfiguration.ReadFrom.Configuration(hostBuilderContext.Configuration) //���������� ������������ �� ������������ hostBuilderContext
                       .MinimumLevel.Debug()    //����������� ������� ������� �������
                       .MinimumLevel.Override("Microsoft", LogEventLevel.Error) //��������������� ������������ ������ ��� ����������� ������������ ���� 
                       .Enrich.FromLogContext() //���������� � ������� ������������ ���������� �� ��������� ��������� �������
                       .WriteTo.Console(        //����� ���� � ������� 
                            outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}]{SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}") //������ ������ �����
                       .WriteTo.RollingFile($@".\Logs\WebStore[{DateTime.Now:yyyy-MM-ddTHH-mm-ss}].log")    //����� ���� � ��������� �����
                       .WriteTo.File(new JsonFormatter(",", true), $@".\Logs\WebStore[{DateTime.Now:yyyy-MM-ddTHH-mm-ss}].log.json")); //����� ���� � ����� ������� json
                       //.WriteTo.Seq("http://localhost:5341/"));     //������� ����� �� ������� Seq �� ��������� Listen Uri (���� https://datalust.co/seq)
    }
}