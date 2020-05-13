using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Entities.Base;

namespace WebStore.Domain.Entities
{
    public class OrderItem: BaseEntity
    {
        /// <summary>
        /// товар в заказе
        /// </summary>
        public virtual Product Product { get; set; }
        /// <summary>
        /// количество товаров
        /// </summary>
        public int ProductQuantity { get; set; }
        /// <summary>
        /// заказ, к которому относится OrderItem
        /// </summary>
        public virtual Order Order { get; set; }
        /// <summary>
        /// цена одного товара
        /// </summary>
        public decimal Price { get; set; }
    }
}
