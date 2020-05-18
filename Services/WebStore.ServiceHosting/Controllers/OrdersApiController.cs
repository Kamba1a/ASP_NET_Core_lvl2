using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Domain.DTO.Orders;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(WebAPI.Orders)]
    [ApiController]
    public class OrdersApiController : ControllerBase, ISqlOrderService
    {
        ISqlOrderService _sqlOrderService;

        public OrdersApiController(ISqlOrderService sqlOrderService)
        {
            _sqlOrderService = sqlOrderService;
        }

        [HttpPost("createOrder/{username}")]
        public OrderDTO CreateOrder([FromBody]CreateOrderModel createOrderModel, string UserName)
        {
            return _sqlOrderService.CreateOrder(createOrderModel, UserName);
        }

        [HttpGet("orders/{username}")]
        public IEnumerable<OrderDTO> GetUserOrders(string username)
        {
            return _sqlOrderService.GetUserOrders(username);
        }
    }
}