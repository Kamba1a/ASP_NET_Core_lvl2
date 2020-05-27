using System.Collections.Concurrent;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace WebStore.Logger
{
    /// <summary>Класс-провайдер для создания логов</summary>
    public class Log4NetLoggerProvider : ILoggerProvider
    {
        private readonly string _configurationFile;

        //Потокобезопасный словарь <имя категории, логгер для обслуживания категории>
        private readonly ConcurrentDictionary<string, Log4NetLogger> _loggers = new ConcurrentDictionary<string, Log4NetLogger>();

        /// <summary>Конструктор Log4NetLoggerProvider</summary>
        /// <param name="configurationFile">Путь к файлу конфигурации</param>
        public Log4NetLoggerProvider(string configurationFile)
        {
            _configurationFile = configurationFile;
        }

        /// <summary>
        /// Получает экзмемпляр Log4NetLogger по имени категории или создает новый
        /// </summary>
        /// <param name="categoryName">имя категории</param>
        /// <returns>Log4NetLogger</returns>
        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName,  //ищет в словаре ключ по имени категории
                category =>     //если ключ не найден, то создает в словаре новую пару <Key,Value> (category, new Log4NetLogger)
                {
                    var xml = new XmlDocument();    //создаем новый xml
                    xml.Load(_configurationFile);   //загружаем туда данные из файла конфигурации
                    return new Log4NetLogger(category, xml["log4net"]);     //передаем в конструктор Log4NetLogger имя категории и раздел <log4net> из файла конфигурации
                });
        }

        public void Dispose() => _loggers.Clear();  //очистка словаря при вызове Dispose
    }
}