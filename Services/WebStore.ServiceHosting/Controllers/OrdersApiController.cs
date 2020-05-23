using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Domain;
using WebStore.Domain.DTO.Orders;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    /// <summary>Контроллер заказов</summary>
    [Route(WebAPI.Orders)]
    [ApiController]
    public class OrdersApiController : ControllerBase, ISqlOrderService
    {
        ISqlOrderService _sqlOrderService;
        private readonly ILogger<OrdersApiController> _Logger; //для примера логирования


        public OrdersApiController(ISqlOrderService sqlOrderService, ILogger<OrdersApiController> Logger)
        {
            _sqlOrderService = sqlOrderService;
            _Logger = Logger;
        }

        /// <summary>Создание нового заказа для указанного пользователя</summary>
        /// <param name="UserName">Имя пользователя для которого оформляется заказ</param>
        /// <param name="createOrderModel">Структура формируемого заказа</param>
        /// <returns>Структура сформированного заказа</returns>
        [HttpPost("createOrder/{username}")]
        public OrderDTO CreateOrder([FromBody]CreateOrderModel createOrderModel, string UserName)
        {
            //обязательно в каждом методе контроллера нужна валидация, особенно в методах "Set.."
            if (string.IsNullOrEmpty(UserName))
                throw new ArgumentException("Не указано имя пользователя");

            if (string.IsNullOrEmpty(createOrderModel.OrderDetails.Address))
                throw new ArgumentException("Не указан адрес доставки");

            //пример логирования (должно быть как в контроллере, так и в клиенте)
            _Logger.LogInformation("Создаётся заказ для пользователя {0}", UserName);
            var order = _sqlOrderService.CreateOrder(createOrderModel, UserName);
            _Logger.LogInformation("Заказ для пользователя {0} создан успешно", UserName);
            return order;

            //return _sqlOrderService.CreateOrder(createOrderModel, UserName);
        }

        /// <summary>Получить все заказы пользователя</summary>
        /// <param name="username">Имя пользователя</param>
        /// <returns>Возвращает список заказов указанного пользователя</returns>
        [HttpGet("orders/{username}")]
        public IEnumerable<OrderDTO> GetUserOrders(string username)
        {
            return _sqlOrderService.GetUserOrders(username);
        }
    }
}