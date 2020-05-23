using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace WebStore.Logger
{
    /// <summary>класс содержащий метод расширения для ILoggerFactory</summary>
    public static class Log4NetExtensions
    {
        /// <summary>
        /// Добавление провайдера с указанным файлом конфигурации  
        /// </summary>
        /// <param name="Factory"></param>
        /// <param name="ConfigurationFile">Путь к файлу конфигурации log4net</param>
        /// <returns></returns>
        public static ILoggerFactory AddLog4Net(this ILoggerFactory loggerFactory, string configurationFile = "log4net.config")
        {
            //требуется проверка расположения указанного файла конфигурации
            if (!Path.IsPathRooted(configurationFile)) //если путь относительный (ведет не из корневого каталога диска)
            {
                var assembly = Assembly.GetEntryAssembly() //получаем сборку, из которой происходит запуск приложения
                               ?? throw new InvalidOperationException("Не найдена сборка с точкой входа в приложение");

                var dir = Path.GetDirectoryName(assembly.Location) //получаем директорию расположения сборки
                          ?? throw new InvalidOperationException("НЕ определена директория расположения стартовой сборки");

                configurationFile = Path.Combine(dir, configurationFile); //корректируем путь к файлу
            }

            //добавляем провайдер с указанным файлом конфигурации
            loggerFactory.AddProvider(new Log4NetLoggerProvider(configurationFile));

            return loggerFactory;
        }
    }
}