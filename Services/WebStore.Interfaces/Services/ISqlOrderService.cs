using System.Linq;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;

namespace WebStore.Interfaces.Services
{
    public interface ISqlOrderService
    {
        IQueryable<Order> GetUserOrders(string username);
        Order CreateOrder(OrderDetailsViewModel model, CartViewModel cart, string UserName);
    }
}
