using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using Newtonsoft.Json;

namespace WebStore.Services
{
    public class CookiesCartStore : ICartStore
    {
        IHttpContextAccessor _httpContextAccessor; //для работы с cookies
        string _cartName;

        public CookiesCartStore(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            //задаем имя корзины, которое включает имя пользователя (а если не авторизован, тогда что включить?)
            _cartName = "cart" + (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated ?
                        _httpContextAccessor.HttpContext.User.Identity.Name : "");
        }

        public Cart Cart
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
    }
}
