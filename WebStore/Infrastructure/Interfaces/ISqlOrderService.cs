using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Models;

namespace WebStore.Infrastructure.Interfaces
{
    public interface ISqlOrderService
    {
        IQueryable<Order> GetUserOrders(string username);
        Order CreateOrder(OrderDetailsViewModel model, CartViewModel cart, string UserName);
    }
}
