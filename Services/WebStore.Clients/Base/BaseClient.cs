using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WebStore.Clients.Base
{
    /// <summary>
    /// базовый клиент
    /// </summary>
    public abstract class BaseClient
    {
        protected readonly string _ServiceAddress;
        protected readonly HttpClient _Client;

        /// <summary>
        /// Конструктор базового клиента
        /// </summary>
        /// <param name="Configuration">объект конфигурации приложения (для извлечения адреса хоста)</param>
        /// <param name="ServiceAddress">адрес сервиса (контроллера) (api/[controller])</param>
        protected BaseClient(IConfiguration Configuration, string ServiceAddress)
        {
            _ServiceAddress = ServiceAddress;
            _Client = new HttpClient            //создаем клиент, который сможет делать HTTP-запросы
            {
                BaseAddress = new Uri(Configuration["WebApiURL"]), //базовый адрес клиента
                DefaultRequestHeaders =
                {
                    Accept =
                    {
                        new MediaTypeWithQualityHeaderValue("application/json") //формат, в котором клиент будет получать данные от сервиса
                    }
                }
            };
        }
    }
}
