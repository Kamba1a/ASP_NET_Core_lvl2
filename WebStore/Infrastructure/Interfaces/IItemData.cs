using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;

namespace WebStore.Infrastructure.Interfaces
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
