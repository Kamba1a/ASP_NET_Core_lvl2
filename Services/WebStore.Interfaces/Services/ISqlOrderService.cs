using System.Collections.Generic;
using WebStore.Domain.DTO.Orders;

namespace WebStore.Interfaces.Services
{
    public interface ISqlOrderService
    {
        IEnumerable<OrderDTO> GetUserOrders(string username);
        OrderDTO CreateOrder(CreateOrderModel createOrderModel, string UserName);
    }
}
