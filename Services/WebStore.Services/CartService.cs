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
    public class CartService : ICartService
    {
        ICatalogData _catalogData;
        ICartStore _cartStore;

        public CartService(ICatalogData catalogData, ICartStore cartStore)
        {
            _catalogData = catalogData;
            _cartStore = cartStore;
        }

        public void DecrementProductQuantity(int productId)
        {
            Cart cart = _cartStore.Cart; //сначала получаем корзину из куки, либо создаем новую

            CartItem cartItem = cart.Items.FirstOrDefault(item => item.ProductId == productId); //находим товар в корзине

            if (cartItem is null) return;   //если такого товара нет, то ничего не делаем
            if (cartItem.Quantity > 0) cartItem.Quantity--; //уменьшаем количество товара
            if (cartItem.Quantity == 0) cart.Items.Remove(cartItem); //если количество достигло нуля, то удаляем товар из корзины

            _cartStore.Cart = cart; //сохраняем корзину в куки
        }

        public void AddToCart(int productId)
        {
            var product = _catalogData.GetProducts().Products.FirstOrDefault(p=>p.Id==productId);
            if (product == null) return;

            Cart cart = _cartStore.Cart; //сначала получаем корзину из куки, либо создаем новую

            CartItem cartItem = cart.Items.FirstOrDefault(item => item.ProductId == productId); //находим товар в корзине

            if (cartItem != null) cartItem.Quantity++; // если  товар уже добавлен, то увеличиваем его количество
            else cart.Items.Add(new CartItem { ProductId = productId, Quantity = 1 }); //иначе добавляем единицу товара в корзину

            _cartStore.Cart = cart; //сохраняем корзину в куки
        }

        public void RemoveAllProductsFromCart()
        {
            Cart cart = _cartStore.Cart;
            cart.Items.Clear();
            _cartStore.Cart = cart;
        }

        public void RemoveProductFromCart(int productId)
        {
            Cart cart = _cartStore.Cart; //сначала получаем корзину из куки, либо создаем новую

            CartItem cartItem = cart.Items.FirstOrDefault(item => item.ProductId == productId); //находим товар в корзине

            if (cartItem is null) return;   //если такого товара нет, то ничего не делаем
            else cart.Items.Remove(cartItem); //иначе удаляем товар из корзины

            _cartStore.Cart = cart; //сохраняем корзину в куки
        }

        public CartViewModel TransformCartToViewModel()
        {
            IEnumerable<ProductViewModel> products = _catalogData.GetProducts(                                  //сначала получаем список Products
                new ProductFilter()
                { 
                    ProductsIdList = _cartStore.Cart.Items.Select(cartItem => cartItem.ProductId).ToList()      //(фильтр по списку ID товаров из корзины)
                })
                .Products.FromDTO().ToView();                                                                   //сразу преобразовываем каждый Product в ProductViewModel

            List<CartItemViewModel> cartItems = new List<CartItemViewModel>();

            foreach (var item in _cartStore.Cart.Items)
            {
                var product = products.FirstOrDefault(p=>p.Id==item.ProductId);
                if (product == null)
                {
                    RemoveProductFromCart(item.ProductId);
                    continue;
                }

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
            return _cartStore.Cart;
        }
    }
}
