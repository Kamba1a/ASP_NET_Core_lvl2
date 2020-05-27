using System;
using System.Reflection;
using System.Xml;
using log4net;
using log4net.Repository;
using Microsoft.Extensions.Logging;

namespace WebStore.Logger
{
    /// <summary>Класс для записи логов</summary>
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _log;

        /// <summary>Конструктор Log4NetLogger</summary>
        /// <param name="categoryName">Имя категории ведения журнала</param>
        /// <param name="configuration">Раздел xml-файла, в котором находятся настройки конфигурации для log4net</param>
        public Log4NetLogger(string categoryName, XmlElement configuration)
        {
            //создаем репозиторий системы логгирования (для чего служит этот репозиторий?)
            ILoggerRepository loggerRepository = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));

            //получаем логгер из репозитория (не очень понятно что это значит)
            _log = LogManager.GetLogger(loggerRepository.Name, categoryName);

            //конфигурируем работу log4net?
            log4net.Config.XmlConfigurator.Configure(loggerRepository, configuration);
        }

        public IDisposable BeginScope<TState>(TState state) => null; //возвращаем null, т.к. не реализована поддержка журнала с учетом областей

        /// <summary>
        /// Проверка, работает ли логгер для указанного уровня ведения журнала
        /// </summary>
        /// <param name="level">Уровень сообщения</param>
        /// <returns></returns>
        public bool IsEnabled(LogLevel level)
        {
            switch (level)
            {
                default: throw new ArgumentOutOfRangeException(nameof(level), level, null);

                case LogLevel.Trace:
                case LogLevel.Debug:
                    return _log.IsDebugEnabled;

                case LogLevel.Information:
                    return _log.IsInfoEnabled;

                case LogLevel.Warning:
                    return _log.IsWarnEnabled;

                case LogLevel.Error:
                    return _log.IsErrorEnabled;

                case LogLevel.Critical:
                    return _log.IsFatalEnabled;

                case LogLevel.None:
                    return false;
            }
        }

        /// <summary>
        /// Запись сообщения в журнал
        /// </summary>
        /// <typeparam name="TState">?</typeparam>
        /// <param name="level">Уровень сообщения</param>
        /// <param name="id">?</param>
        /// <param name="state">?</param>
        /// <param name="error">исключение Exception</param>
        /// <param name="formatter">?</param>
        public void Log<TState>(
            LogLevel level,
            EventId id,
            TState state,
            Exception error,
            Func<TState, Exception, string> formatter)
        {
            if (formatter is null)
                throw new ArgumentNullException(nameof(formatter));

            if (!IsEnabled(level)) return; //если логер выключен для указанного уровня, то ничего не делаем

            var log_message = formatter(state, error); //формирование сообщения с помощью formatter

            if (string.IsNullOrEmpty(log_message) && error is null) return;

            switch (level) //запись сообщения
            {
                default: throw new ArgumentOutOfRangeException(nameof(level), level, null);

                case LogLevel.Trace:
                case LogLevel.Debug:
                    _log.Debug(log_message);
                    break;

                case LogLevel.Information:
                    _log.Info(log_message);
                    break;

                case LogLevel.Warning:
                    _log.Warn(log_message);
                    break;

                case LogLevel.Error:
                    _log.Error(log_message, error);
                    break;

                case LogLevel.Critical:
                    _log.Fatal(log_message, error);
                    break;

                case LogLevel.None:
                    break;
            }
        }
    }
}