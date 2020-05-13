using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using WebStore.DAL;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Interfaces;
using WebStore.Models;

namespace WebStore.Infrastructure.Services
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

        public IQueryable<Order> GetUserOrders(string username)
        {
            return _webStoreContext.Orders.Where(o => o.User.UserName == username);
        }

        Order ISqlOrderService.CreateOrder(OrderDetailsViewModel model, CartViewModel cart, string UserName)
        {
            Order order = new Order()
            {
                DateTime = DateTime.Now,
                User = _userManager.FindByNameAsync(UserName).Result,
                Address = model.Address,
                Phone = model.Phone,
                TotalPrice = cart.TotalPrice
            };

            using (var transaction = _webStoreContext.Database.BeginTransaction()) {

                _webStoreContext.Orders.Add(order);

                foreach (var item in cart.CartItems)
                {
                    Product product = _webStoreContext.Products.FirstOrDefault(p => p.Id == item.Product.Id);

                    if (product == null) throw new InvalidOperationException("Продукт не найден в базе");

                    OrderItem orderItem = new OrderItem()
                    {
                        Product = product,
                        Price = item.Price,
                        ProductQuantity = item.Quantity,
                        Order = order
                    };
                    _webStoreContext.OrderItems.Add(orderItem);
                }
                _webStoreContext.SaveChanges();
                transaction.Commit();
            }

            return order;
        }
    }
}
