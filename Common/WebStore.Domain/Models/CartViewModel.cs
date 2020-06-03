using System.Collections.Generic;
using System.Linq;

namespace WebStore.Domain.Models
{
    /// <summary>Модель представления корзины</summary>
    public class CartViewModel
    {
        /// <summary>Товары в корзине</summary>
        public IEnumerable <CartItemViewModel> CartItems { get; set; }

        /// <summary>Итоговая цена</summary>
        public decimal TotalPrice => CartItems.Sum(item => item.Price * item.Quantity);

        /// <summary>Итоговое количество товаров</summary>
        public int ItemsCount => CartItems.Sum(item => item.Quantity);
    }
}
