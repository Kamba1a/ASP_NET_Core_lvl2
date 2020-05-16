using System;
using System.Collections.Generic;
using System.Net;

namespace WebStore.Interfaces.Api
{
    /// <summary>
    /// интерфейс тестовой веб-службы (для взаимодействия с веб-службой)
    /// </summary>
    public interface IValueServices
    {
        /// <summary>
        /// получает перечень объектов
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> Get();

        /// <summary>
        /// получает объект по его идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string Get(int id);

        /// <summary>
        /// создает новый объект и создает ссылку с адресом этого объекта
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Uri Post(string value);

        /// <summary>
        /// редактирует объект по его идентификатору, передавая в него значение (value) и возвращает код статуса операции
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        HttpStatusCode Update(int id, string value);

        /// <summary>
        /// удаляет  объект по его идентификатору, возвращая код статуса операции (успех/ошибка)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        HttpStatusCode Delete(int id);
    }
}
