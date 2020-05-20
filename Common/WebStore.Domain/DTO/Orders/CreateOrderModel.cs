using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.Models;

namespace WebStore.Domain.DTO.Orders
{
    public class CreateOrderModel
    {
        public OrderDetailsViewModel OrderDetails { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
        public decimal TotalPrice => OrderItems.Sum(o => o.Price * o.Quantity);
    }
}
