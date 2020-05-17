using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Services
{
    public class CookieCartService : ICartService
    {
        ICatalogData _catalogData;
        IHttpContextAccessor _httpContextAccessor; //для работы с cookies
        string _cartName;

        private Cart Cart
        {
            get
            {
                string cookie = _httpContextAccessor.HttpContext.Request.Cookies[_cartName]; //запрос куки с определенным именем
                string json;
                Cart cart;

                if (cookie == null) //если куки не найдена, то создается новая корзина
                {
                    cart = new Cart { Items = new List<CartItem>() }; //создаем корзину
                    json = JsonConvert.SerializeObject(cart);

                    _httpContextAccessor.HttpContext.Response.Cookies.Append(_cartName, json, new CookieOptions { Expires = DateTime.Now.AddDays(1) }); //создаем куки

                    return cart;
                }
                else //иначе возвращаем корзину из куки
                {
                    json = cookie;
                    cart = JsonConvert.DeserializeObject<Cart>(json);

                    _httpContextAccessor.HttpContext.Response.Cookies.Delete(_cartName);
                    _httpContextAccessor.HttpContext.Response.Cookies.Append(_cartName, json, new CookieOptions() { Expires = DateTime.Now.AddDays(1) });

                    return cart;
                }
            }
            set
            {
                string json = JsonConvert.SerializeObject(value);

                _httpContextAccessor.HttpContext.Response.Cookies.Delete(_cartName);
                _httpContextAccessor.HttpContext.Response.Cookies.Append(_cartName, json, new CookieOptions() { Expires = DateTime.Now.AddDays(1) });
            }
        }


        public CookieCartService(ICatalogData catalogData, IHttpContextAccessor httpContextAccessor)
        {
            _catalogData = catalogData;
            _httpContextAccessor = httpContextAccessor;

            //задаем имя корзины, которое включает имя пользователя (а если не авторизован, тогда что включить?)
            _cartName = "cart" + (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated?
                        _httpContextAccessor.HttpContext.User.Identity.Name : "");
        }


        public void DecrementProductQuantity(int productId)
        {
            Cart cart = Cart; //сначала получаем корзину из куки, либо создаем новую

            CartItem cartItem = cart.Items.FirstOrDefault(item => item.ProductId == productId); //находим товар в корзине

            if (cartItem is null) return;   //если такого товара нет, то ничего не делаем
            if (cartItem.Quantity > 0) cartItem.Quantity--; //уменьшаем количество товара
            if (cartItem.Quantity == 0) cart.Items.Remove(cartItem); //если количество достигло нуля, то удаляем товар из корзины

            Cart = cart; //сохраняем корзину в куки
        }

        public void AddToCart(int productId)
        {
            Cart cart = Cart; //сначала получаем корзину из куки, либо создаем новую

            CartItem cartItem = cart.Items.FirstOrDefault(item => item.ProductId == productId); //находим товар в корзине

            if (cartItem != null) cartItem.Quantity++; // если  товар уже добавлен, то увеличиваем его количество
            else cart.Items.Add(new CartItem { ProductId = productId, Quantity = 1 }); //иначе добавляем единицу товара в корзину

            Cart = cart; //сохраняем корзину в куки
        }

        public void RemoveAllProductsFromCart()
        {
            Cart cart = Cart;
            cart.Items.Clear();
            Cart = cart;
        }

        public void RemoveProductFromCart(int productId)
        {
            Cart cart = Cart; //сначала получаем корзину из куки, либо создаем новую

            CartItem cartItem = cart.Items.FirstOrDefault(item => item.ProductId == productId); //находим товар в корзине

            if (cartItem is null) return;   //если такого товара нет, то ничего не делаем
            else cart.Items.Remove(cartItem); //иначе удаляем товар из корзины

            Cart = cart; //сохраняем корзину в куки
        }

        public CartViewModel TransformCartToViewModel()
        {
            IEnumerable<ProductViewModel> products = _catalogData.GetProducts(                      //сначала получаем список Products
                new ProductFilter()
                { 
                    ProductsIdList = Cart.Items.Select(cartItem => cartItem.ProductId).ToList()     //(фильтр по списку ID товаров из корзины)
                })
                .FromDTO().ToView();                                                                //сразу преобразовываем каждый Product в ProductViewModel

            List<CartItemViewModel> cartItems = new List<CartItemViewModel>();

            foreach (var item in Cart.Items)
            {
                CartItemViewModel cartItem = new CartItemViewModel()
                {
                   Product = products.First(p => p.Id==item.ProductId),
                   Quantity = item.Quantity
                };

                cartItems.Add(cartItem);
            }

            CartViewModel cartViewModel = new CartViewModel()
            {
                CartItems = cartItems
            };

            return cartViewModel;
        }

        public Cart GetCart()
        {
            return Cart;
        }
    }
}
