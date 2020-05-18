using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using WebStore.Domain.DTO.Orders;
using WebStore.Domain.Entities;

namespace WebStore.Services.Mapping
{
    public static class OrderMapping
    {
        public static IEnumerable<OrderDTO> ToDTO(this IEnumerable<Order> orders)
        {
            return orders.Select(o=>o.ToDTO());
        }

        public static OrderDTO ToDTO(this Order order) => order is null ? null : new OrderDTO
        {

            Id = order.Id,
            Address = order.Address,
            Phone = order.Phone,
            Date = order.DateTime,
            OrderItems = order.OrderItems.ToDTO(),
            TotalPrice = order.TotalPrice,
            UserName = order.User.UserName
        };

        public static IEnumerable<OrderItemDTO> ToDTO(this IEnumerable<OrderItem> items)
        {
            return items.Select(i => i.ToDTO());
        }

        public static OrderItemDTO ToDTO(this OrderItem orderItem) => orderItem is null ? null : new OrderItemDTO
        {
            Id = orderItem.Id,
            ProductId = orderItem.Product.Id,
            Quantity = orderItem.ProductQuantity,
            Price = orderItem.Price
        };
    }
}
