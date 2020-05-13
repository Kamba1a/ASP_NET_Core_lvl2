using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities
{
    /// <inheritdoc cref="NamedEntity" />
    /// <summary>
    /// Сущность товар
    /// </summary>
    [Table ("Products")] //имя таблицы (в данном случае атрибут только для явности - система сама даст такое название (имя сущности + s)
    public class Product : NamedEntity, IOrderedEntity
    {
        public int Order { get; set; }

        /// <summary>
        /// ID секции, к которой принадлежит товар
        /// </summary>
        public int SectionId { get; set; }

        /// <summary>
        /// ID бренда, к которому относится товар
        /// </summary>
        public int BrandId { get; set; }

        /// <summary>
        /// Ссылка на картинку
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Цена
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Секция, к которой относится товар
        /// </summary>
        [ForeignKey ("SectionId")] //связывание свойств (в данном случае, атрибуты только для явности, т.к. если EF видит имя свойства и такое же имя + Id, то автоматически их свяжет)
        public virtual Section Section { get; set; }

        /// <summary>
        /// Бренд, к которому относится товар
        /// </summary>
        [ForeignKey ("BrandId")] //связывание свойств
        public virtual Brand Brand { get; set; }
    }
}
