using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using WebStore.Clients.Base;
using WebStore.Domain;
using WebStore.Interfaces.Api;

namespace WebStore.Clients.Values
{
    /// <summary>
    /// Клиент для тестового WebAPI
    /// </summary>
    public class ValuesClient : BaseClient, IValueServices
    {
        public ValuesClient(IConfiguration Configuration) : base(Configuration, WebAPI.Values)
        {
        }

        public IEnumerable<string> Get()
        {
            var response = _Client.GetAsync(_ServiceAddress).Result;    //адрес, куда направляется запрос
            if (response.IsSuccessStatusCode)                           //если запрос успешен, то получаем результат
                return response.Content.ReadAsAsync<IEnumerable<string>>().Result;

            return Enumerable.Empty<string>();                          //иначе вернуть пустую колллекцию 
        }

        public string Get(int id)
        {
            var response = _Client.GetAsync($"{_ServiceAddress}/{id}").Result;
            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsAsync<string>().Result;

            return string.Empty;
        }

        public Uri Post(string value)
        {
            var response = _Client.PostAsJsonAsync($"{_ServiceAddress}/post", value).Result;
            return response.EnsureSuccessStatusCode().Headers.Location;     //если операция выполнена успешно, то обращаемся к заголовкам, из которых извлекаем Url
        }

        public HttpStatusCode Update(int id, string value)
        {
            var response = _Client.PutAsJsonAsync($"{_ServiceAddress}/put/{id}", value).Result;
            return response.EnsureSuccessStatusCode().StatusCode;   //возвращается просто статусный код
        }

        public HttpStatusCode Delete(int id)
        {
            var response = _Client.DeleteAsync($"{_ServiceAddress}/delete/{id}").Result;
            return response.StatusCode;
        }
    }
}
