using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebStore.Domain.Entities
{
    /// <summary>
    /// Сущность "Корзина"
    /// </summary>
    public class Cart
    {
        public List<CartItem> Items { get; set; }
        public int ItemsCount => Items?.Sum(item => item.Quantity) ?? 0;
    }
}
