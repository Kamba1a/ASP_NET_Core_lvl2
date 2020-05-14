using System.Collections.Generic;

namespace WebStore.Domain.Entities.Base.Interfaces
{
    /// <summary>
    /// Сущность, содержащая в себе перечень товаров
    /// </summary>
    interface IProductCollectionEntity
    {
        /// <summary>
        /// Перечень товаров
        /// </summary>
        public ICollection<Product> Products { get; set; }
    }
}
