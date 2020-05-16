using System;
using System.Collections.Generic;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Domain.Entities
{
    public class Order: BaseEntity
    {
        /// <summary>
        /// перечень OrderItems
        /// </summary>
        public virtual IEnumerable<OrderItem> OrderItems { get; set; }
        /// <summary>
        /// пользователь, которому принадлежит заказ
        /// </summary>
        public virtual User User { get; set; }
        /// <summary>
        /// адрес доставки
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Телефон для связи
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// время заказа
        /// </summary>
        public DateTime DateTime { get; set; }
        /// <summary>
        /// общая цена заказа
        /// </summary>
        public decimal TotalPrice { get; set; }
    }
}
