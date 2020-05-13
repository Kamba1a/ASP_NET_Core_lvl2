using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Interfaces;
using WebStore.Models;

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
                Order newOrder = _sqlOrderService.CreateOrder(model, _cartService.TransformCartToViewModel(), userName);
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