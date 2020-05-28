using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Domain.DTO.Orders;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class CartControllerTests
    {
        /// <summary>
        /// OrderCheckout
        /// if (!ModelState.IsValid)
        /// </summary>
        [TestMethod]
        public void OrderCheckout_ModelState_Invalid_Returns_ViewModel()
        {
            var cart_service_mock = new Mock<ICartService>();   //пустые заглушки, т.к. не используются в тесте, но требуются для контроллера
            var order_service_mock = new Mock<ISqlOrderService>();

            var controller = new CartController(cart_service_mock.Object, order_service_mock.Object)
            {
                //т.к. метод использует User.Identity.Name, для теста необходимо создать такого User самим
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "TestUserName"), }))
                    }
                }
            };

            controller.ModelState.AddModelError("error", "InvalidModel"); //вызываем в контроллере ошибку валидации, чтобы сработала ветка метода if !ModelState.IsValid

            const string expected_model_name = "Test order";

            var result = controller.OrderCheckout(new OrderDetailsViewModel { Name = expected_model_name });

            var view_result = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<OrderViewModel>(view_result.Model);

            Assert.Equal(expected_model_name, model.OrderData.Name);
        }

        /// <summary>
        /// OrderCheckout
        /// main branch
        /// </summary>
        [TestMethod]
        public void OrderCheckout_Calls_Service_and_Return_Redirect()
        {
            const int expected_order_id = 1;

            var cart_service_mock = new Mock<ICartService>();
            cart_service_mock
               .Setup(c => c.TransformCartToViewModel())
               .Returns(() => new CartViewModel
               {
                   CartItems = new []
                    {
                        new CartItemViewModel()
                        {
                            Product = new ProductViewModel(){ Name = "Product" },
                            Quantity = 1 
                        }
                    }
               });

            var order_service_mock = new Mock<ISqlOrderService>();
            order_service_mock
               .Setup(c => c.CreateOrder(It.IsAny<CreateOrderModel>(), It.IsAny<string>()))
               .Returns(new OrderDTO
               {
                   Id = expected_order_id
               });

            var controller = new CartController(cart_service_mock.Object, order_service_mock.Object)
            {
                //т.к. метод использует User.Identity.Name, для теста необходимо создать такого User самим
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "TestUserName"), })) 
                    }
                }
            };

            var result = controller.OrderCheckout(new OrderDetailsViewModel
            {
                Name = "Test",
                Address = "Address",
                Phone = "Phone"
            });

            var redirect_result = Assert.IsType<RedirectToActionResult>(result);

            Assert.Null(redirect_result.ControllerName);
            Assert.Equal(nameof(CartController.OrderConfirmed), redirect_result.ActionName);

            Assert.Equal(expected_order_id, redirect_result.RouteValues["orderNumber"]);
        }

        /// <summary>
        /// OrderCheckout
        /// if (userName == null)
        /// </summary>
        [TestMethod]
        public void OrderCheckout_User_Null_Return_Redirect()
        {
            var cart_service_mock = new Mock<ICartService>();
            var order_service_mock = new Mock<ISqlOrderService>();

            var controller = new CartController(cart_service_mock.Object, order_service_mock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity()) //User.Identity.Name = null
                    }
                }
            };

            var result = controller.OrderCheckout(new OrderDetailsViewModel());

            var redirect_result = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(controller.Cart), redirect_result.ActionName);
        }
    }
}