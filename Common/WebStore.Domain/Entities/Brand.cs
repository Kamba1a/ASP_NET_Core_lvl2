using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities
{
    /// <inheritdoc cref="NamedEntity" />
    /// <summary>Сущность бренд</summary>
    [Table("Brands")] // имя таблицы (в данном случае атрибут только для явности - система сама даст такое название (имя сущности + s)
    public class Brand : NamedEntity, IOrderedEntity, IProductCollectionEntity
    {
        public int Order { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
