using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WebStore.Infrastructure.Middleware
{
    //Собственное промежуточное ПО для централизованной обработки исключений (можно использовать при отладке)
    /// <summary>Перехватывает ошибку (возникающую в следующим далее промежуточном ПО) и записывает ее в лог</summary>
    public class ErrorHandlingMiddleware
    {
        /// <summary>содержит ссылку на тот делегат запроса, который стоит следующим в конвейере обработки запроса</summary>
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Перехват ошибки, возникающей далее по конвейеру запросов
        /// </summary>
        /// <param name="context">Контекст запроса для обработки</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context) //метод обрабатывает запрос и вызывает делегат, передавая контекст дальше по конвейеру
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                HandleException(context, e);
                throw;
            }
        }

        /// <summary>Запись информации об ошибке в журнал</summary>
        /// <param name="Context"></param>
        /// <param name="Error"></param>
        private void HandleException(HttpContext Context, Exception Error)
        {
            _logger.LogError(Error, "Ошибка при обработке запроса {0}", Context.Request.Path);
        }
    }
}