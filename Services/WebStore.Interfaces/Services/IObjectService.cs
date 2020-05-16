using System.Collections.Generic;

namespace WebStore.Interfaces.Services
{
    /// <summary>
    /// Универсальный интерфейс для работы с объектами
    /// (делался ради интереса)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObjectService<T>
    {
        /// <summary>
        /// Возвращает коллекцию объектов
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAll();
        /// <summary>
        /// Возвращает объект по Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        T GetById(int id);
        /// <summary>
        /// Добавляет новый объект в коллекцию
        /// </summary>
        /// <param name="employee"></param>
        void AddNew(T employee);
        /// <summary>
        /// Удаляет объект из коллекции
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);
        /// <summary>
        /// Сохраняет изменения в БД
        /// </summary>
        void Commit();
    }
}
