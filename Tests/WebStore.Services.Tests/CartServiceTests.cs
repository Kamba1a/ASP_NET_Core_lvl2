using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Domain;
using WebStore.Domain.DTO.Catalog;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;
using Assert = Xunit.Assert;

//можно запускать тесты через консоль (cmd из папки проекта)
//
//правильный порядок команд:
// dotnet restore
// dotnet build --no-restore
// dotnet test --no-build
//
// dotnet publish
//последняя команда компилирует приложение и публикует итоговый набор файлов в каталоге

namespace WebStore.Services.Tests
{
    [TestClass]
    public class CartServiceTests
    {
        private Cart _Cart;
        private Mock<ICatalogData> _CatalogDataMock;
        private Mock<ICartStore> _CartStoreMock;
        private ICartService _CartService;

        [TestInitialize] //выполняется перед каждым TestMethod
        public void TestInitialize()
        {
            _Cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new CartItem { ProductId = 1, Quantity = 1 },
                    new CartItem { ProductId = 2, Quantity = 3 }
                }
            };

            _CatalogDataMock = new Mock<ICatalogData>();
            _CatalogDataMock
               .Setup(c => c.GetProducts(It.IsAny<ProductFilter>()))
               .Returns(new List<ProductDTO>
                {
                    new ProductDTO
                    {
                        Id = 1,
                        Name = "Product 1",
                        Price = 1.1m,
                        Order = 0,
                        ImageUrl = "Product1.png",
                        Brand = new BrandDTO { Id = 1, Name = "Brand 1" },
                        Section = new SectionDTO { Id = 1, Name = "Section 1"}
                    },
                    new ProductDTO
                    {
                        Id = 2,
                        Name = "Product 2",
                        Price = 2.2m,
                        Order = 0,
                        ImageUrl = "Product2.png",
                        Brand = new BrandDTO { Id = 2, Name = "Brand 2" },
                        Section = new SectionDTO { Id = 2, Name = "Section 2"}
                    },
                });

            _CartStoreMock = new Mock<ICartStore>();
            _CartStoreMock
                .Setup(c => c.Cart)
                .Returns(_Cart);

            _CartService = new CartService(_CatalogDataMock.Object, _CartStoreMock.Object);
        }

        /// <summary>Тестирование модели представления корзины</summary>
        [TestMethod]
        public void CartViewModel_Returns_Correct_TotalPrice()
        {
            var cart_view_model = new CartViewModel
            {
                CartItems = new[]
                {
                    new CartItemViewModel()
                    {
                        Product = new ProductViewModel(){ Id = 1, Name = "Product 1", Price = 1000 },
                        Quantity = 1
                    },
                    new CartItemViewModel()
                    {
                        Product = new ProductViewModel(){ Id = 2, Name = "Product 2", Price = 500 },
                        Quantity = 3
                    }
                }
            };
            const decimal expected_price = 2500;

            var actual_price = cart_view_model.TotalPrice;

            Assert.Equal(expected_price, actual_price);
        }

        /// <summary>Тестирование модели корзины</summary>
        [TestMethod]
        public void Cart_Class_ItemsCount_returns_Correct_Quantity()
        {
            const int expected_count = 4;

            var actual_count = _Cart.ItemsCount;

            Assert.Equal(expected_count, actual_count);
        }

        [TestMethod]
        public void CartService_AddToCart_WorkCorrect()
        {
            _Cart.Items.Clear();

            const int expected_id = 5;

            _CartService.AddToCart(expected_id);

            Assert.Equal(1, _Cart.ItemsCount);

            Assert.Single(_Cart.Items);
            Assert.Equal(expected_id, _Cart.Items[0].ProductId);
        }

        [TestMethod]
        public void CartService_RemoveProductFromCart_Remove_Correct_Item()
        {
            const int item_id = 1;

            _CartService.RemoveProductFromCart(item_id);

            Assert.Single(_Cart.Items);
            Assert.Equal(2, _Cart.Items[0].ProductId);
        }

        [TestMethod]
        public void CartService_RemoveAllProductsFromCart_ClearCart()
        {
            _CartService.RemoveAllProductsFromCart();

            Assert.Empty(_Cart.Items);
        }

        [TestMethod]
        public void CartService_DecrementProductQuantity_Correct()
        {
            const int item_id = 2;

            _CartService.DecrementProductQuantity(item_id);

            Assert.Equal(3, _Cart.ItemsCount);
            Assert.Equal(2, _Cart.Items.Count);
            Assert.Equal(item_id, _Cart.Items[1].ProductId);
            Assert.Equal(2, _Cart.Items[1].Quantity);
        }

        [TestMethod]
        public void CartService_Remove_Item_When_Decrement_to_0()
        {
            const int item_id = 1;

            _CartService.DecrementProductQuantity(item_id);

            Assert.Equal(3, _Cart.ItemsCount);
            Assert.Single(_Cart.Items);
        }

        [TestMethod]
        public void CartService_TransformCartToViewModel_WorkCorrect()
        {
            var result = _CartService.TransformCartToViewModel();

            Assert.Equal(7.7m, result.TotalPrice);
            Assert.Equal(1.1m, result.CartItems.First().Price);
        }
    }
}
