using System;
using System.Collections.Generic;
using WebStore.Domain.Entities.Base;

namespace WebStore.Domain.DTO.Orders
{
    public class OrderDTO: BaseEntity
    {
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<OrderItemDTO> OrderItems { get; set; }
        public decimal TotalPrice { get; set; }
        public string UserName { get; set; }
    }
}
