using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using WebStore.Clients.Base;
using WebStore.Domain;
using WebStore.Domain.DTO.Orders;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Orders
{
    public class OrdersClient : BaseClient, ISqlOrderService
    {
        public OrdersClient(IConfiguration Configuration) : base(Configuration, WebAPI.Orders)
        {
        }

        public OrderDTO CreateOrder(CreateOrderModel createOrderModel, string UserName)
        {
            return Post($"{_ServiceAddress}/createOrder/{UserName}", createOrderModel)
                .Content.ReadAsAsync<OrderDTO>().Result;
        }

        public IEnumerable<OrderDTO> GetUserOrders(string username)
        {
            return Get<IEnumerable<OrderDTO>>($"{_ServiceAddress}/orders/{username}");
        }
    }
}
