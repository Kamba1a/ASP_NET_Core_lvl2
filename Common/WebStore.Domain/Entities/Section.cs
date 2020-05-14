using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities
{
    /// <inheritdoc cref="NamedEntity" />
    /// <summary>Сущность секция</summary>
    [Table ("Sections")] // имя таблицы (в данном случае атрибут только для явности - система сама даст такое название (имя сущности + s)
    public class Section : NamedEntity, IOrderedEntity, IProductCollectionEntity
    {
        /// <summary>ID родительской секции (при наличии)</summary>
        public int? ParentId { get; set; }

        public int Order { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        /// <summary>
        /// Родительская секция (при наличии)
        /// </summary>
        [ForeignKey ("ParentId")] // связывание свойств
        public virtual Section ParentSection { get; set; }
    }
}
