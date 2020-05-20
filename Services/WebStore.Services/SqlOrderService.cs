using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.DAL;
using WebStore.Domain.DTO.Orders;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Services
{
    public class SqlOrderService : ISqlOrderService
    {
        UserManager<User> _userManager;
        WebStoreContext _webStoreContext;

        public SqlOrderService(UserManager<User> userManager, WebStoreContext webStoreContext)
        {
            _userManager = userManager;
            _webStoreContext = webStoreContext;
        }

        public IEnumerable<OrderDTO> GetUserOrders(string username)
        {
            IQueryable<Order> orders = _webStoreContext.Orders
                .Include(o=>o.User)
                .Include(o=>o.OrderItems).ThenInclude(i=>i.Product)
                .Where(o => o.User.UserName == username);

            return orders.ToDTO();
        }

        public OrderDTO CreateOrder(CreateOrderModel orderModel, string UserName)
        {
            Order order = new Order()
            {
                DateTime = DateTime.Now,
                User = _userManager.FindByNameAsync(UserName).Result,
                Address = orderModel.OrderDetails.Address,
                Phone = orderModel.OrderDetails.Phone,
                TotalPrice = orderModel.TotalPrice
            };

            using (var transaction = _webStoreContext.Database.BeginTransaction())
            {

                _webStoreContext.Orders.Add(order);

                foreach (var item in orderModel.OrderItems)
                {
                    Product product = _webStoreContext.Products.FirstOrDefault(p => p.Id == item.ProductId);

                    if (product == null) throw new InvalidOperationException("Продукт не найден в базе");

                    OrderItem orderItem = new OrderItem()
                    {
                        Product = product,
                        Price = product.Price,
                        ProductQuantity = item.Quantity,
                        Order = order, 
                    };
                    _webStoreContext.OrderItems.Add(orderItem);
                }
                _webStoreContext.SaveChanges();
                transaction.Commit();
            }

            return order.ToDTO();
        }
    }
}
