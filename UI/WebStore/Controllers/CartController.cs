using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Linq;
using WebStore.Domain.DTO.Orders;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        ICartService _cartService;
        ISqlOrderService _sqlOrderService;

        public CartController(ICartService cartService, ISqlOrderService sqlOrderService)
        {
            _cartService = cartService;
            _sqlOrderService = sqlOrderService;
        }

        public IActionResult Cart()
        {
            return View(
                new OrderViewModel()
                {
                    Cart = _cartService.TransformCartToViewModel(),
                    OrderData = new OrderDetailsViewModel()
                }
                );
        }

        public IActionResult AddToCart(int productId, string returnUrl)
        {
            _cartService.AddToCart(productId);
            return Redirect(returnUrl); //редирект на страницу, где был пользователь
        }

        public IActionResult RemoveAllProducts()
        {
            _cartService.RemoveAllProductsFromCart();
            return RedirectToAction("Cart");
        }

        public IActionResult RemoveProduct(int productId)
        {
            _cartService.RemoveProductFromCart(productId);
            return RedirectToAction("Cart");
        }

        public IActionResult DecrementProductQuantity(int productId)
        {
            _cartService.DecrementProductQuantity(productId);
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public IActionResult AddToCartFromProductDetails(int productId, int quantity)
        {
            _cartService.AddToCart(productId); //потом заменить на отдельный метод, который обрабатывает quantity
            return RedirectToAction("ProductDetails","Catalog", new { productId});
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult OrderCheckout(OrderDetailsViewModel model)
        {
            string userName = User.Identity.Name;
            if (userName == null)
            {
                return RedirectToAction("Cart");
            }

            if (ModelState.IsValid)
            {
                CreateOrderModel createOrderModel = new CreateOrderModel()
                {
                    OrderDetails = model,
                    OrderItems = _cartService.TransformCartToViewModel().CartItems.Select(i=>new OrderItemDTO() 
                    { 
                        ProductId = i.Product.Id,
                        Quantity = i.Quantity,
                        Price = i.Price
                    }).ToList()
                };
                OrderDTO newOrder = _sqlOrderService.CreateOrder(createOrderModel, userName);
                _cartService.RemoveAllProductsFromCart();
                return RedirectToAction("OrderConfirmed", new { orderNumber = newOrder.Id });
            }
            else
            {
                OrderViewModel order = new OrderViewModel()
                {
                    Cart = _cartService.TransformCartToViewModel(),
                    OrderData = model
                };
                return View("Cart", order);
            }
        }

        public IActionResult OrderConfirmed(int orderNumber)
        {
            return View(orderNumber);
        }
    }
}