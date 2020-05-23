using System;
using System.Collections.Generic;
using WebStore.Domain.Entities.Base;

namespace WebStore.Domain.DTO.Orders
{
    /// <summary>Структура заказа</summary>
    public class OrderDTO: BaseEntity
    {
        /// <summary>Телефон для связи</summary>
        public string Phone { get; set; }

        /// <summary>Адрес доставки</summary>
        public string Address { get; set; }

        /// <summary>Дата формирования</summary>
        public DateTime Date { get; set; }

        /// <summary>Пункты</summary>
        public IEnumerable<OrderItemDTO> OrderItems { get; set; }

        /// <summary>Сумма заказа</summary>
        public decimal TotalPrice { get; set; }

        /// <summary>Имя пользователя</summary>
        public string UserName { get; set; }
    }
}
