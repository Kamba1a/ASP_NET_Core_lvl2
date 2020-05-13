using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities;

namespace WebStore.Models
{
    public class CartViewModel
    {
        public IEnumerable <CartItemViewModel> CartItems { get; set; }
        public decimal TotalPrice => CartItems.Sum(item => item.Price * item.Quantity);
    }
}
