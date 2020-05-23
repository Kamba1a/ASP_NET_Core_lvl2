using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.Models;

namespace WebStore.Domain.DTO.Orders
{
    /// <summary>Структура оформляемого заказа</summary>
    public class CreateOrderModel
    {
        /// <summary>Информация о заказе</summary>
        public OrderDetailsViewModel OrderDetails { get; set; }

        /// <summary>Пункты заказа</summary>
        public List<OrderItemDTO> OrderItems { get; set; }

        /// <summary>Сумма заказа</summary>
        public decimal TotalPrice => OrderItems.Sum(o => o.Price * o.Quantity);
    }
}
