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
                .ConfigureLogging((hostBuilderContext, loggingBuilder)=>    //������ ���������� ������������� ������� ������������ � ���� (���������� ����������� � �������� ���������)
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
                    builder.UseStartup<Startup>());
    }
}