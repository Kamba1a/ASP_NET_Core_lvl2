using System.Collections.Generic;

namespace WebStore.Interfaces.Services
{
    public interface IitemData<T>
    {
        /// <summary>
        /// Возвращает список сотрудников
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAll();
        /// <summary>
        /// Возвращает сотрудника по Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        T GetById(int id);
        /// <summary>
        /// Добавляет нового сотрудника в список
        /// </summary>
        /// <param name="employee"></param>
        void AddNew(T employee);
        /// <summary>
        /// Удаляет сотрудника из списка
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);
        /// <summary>
        /// Сохраняет изменения в БД
        /// </summary>
        void Commit();
    }
}
