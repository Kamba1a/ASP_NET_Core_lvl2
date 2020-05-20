using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

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

        //ниже создаем синхронную и асинхронную версию методов для каждого типа запроса
        //синхронная версия нужна только в нашем случае, т.к. все сервисы синхронные (по-правильному все должно быть асинхронное)
        //синхронные версии просто выполняем через асинхронные
        //параметр CancellationToken нужен во всех асинхронных операциях для возможности их отмены вызывающей стороной (не обязателен)

        protected T Get<T>(string url) => GetAsync<T>(url).Result;
        protected async Task<T> GetAsync<T>(string url, CancellationToken Cancel = default) //where T : new() //- таким образом можно указать, что шаблон создан только для типов, у которых есть конструктор по-умолчанию
        {
            var response = await _Client.GetAsync(url, Cancel);                                 //делаем запрос через клиент
            return await response.EnsureSuccessStatusCode().Content.ReadAsAsync<T>(Cancel);     //в случае успеха возвращаются данные, иначе HttpRequestExeption

            //первоначальная реализация:
            //if (response.IsSuccessStatusCode) return await response.Content.ReadAsAsync<T>(Cancel);
            //return new T(); //в случае ошибки возвращаем значение по-умолчанию
        }

        protected HttpResponseMessage Post<T>(string url, T item) => PostAsync(url, item).Result;

        protected async Task<HttpResponseMessage> PostAsync<T>(string url, T item, CancellationToken Cancel = default)
        {
            var response = await _Client.PostAsJsonAsync(url, item, Cancel); //передает item в формате json
            return response.EnsureSuccessStatusCode(); //возвращает статус-код выполнения операции
        }

        protected HttpResponseMessage Put<T>(string url, T item) => PutAsync(url, item).Result;

        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T item, CancellationToken Cancel = default)
        {
            var response = await _Client.PutAsJsonAsync(url, item, Cancel);
            return response.EnsureSuccessStatusCode();
        }

        protected HttpResponseMessage Delete(string url) => DeleteAsync(url).Result;

        protected async Task<HttpResponseMessage> DeleteAsync(string url, CancellationToken Cancel = default)
        {
            return await _Client.DeleteAsync(url, Cancel);
        }
    }
}
