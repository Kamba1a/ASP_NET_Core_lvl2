using WebStore.Domain.Entities.Base;

namespace WebStore.Domain.Entities
{
    /// <summary>
    /// Сущность "Работник"
    /// </summary>
    public class Employee: BaseEntity
    {
        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Фамилия
        /// </summary>
        public string SurName { get; set; }
        /// <summary>
        /// Отчество
        /// </summary>
        public string Patronymic { get; set; }
        /// <summary>
        /// Возраст
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// Должность
        /// </summary>
        public string Position { get; set; }
    }
}
