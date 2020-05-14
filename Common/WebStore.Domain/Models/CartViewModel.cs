using System.Collections.Generic;
using System.Linq;

namespace WebStore.Domain.Models
{
    public class CartViewModel
    {
        public IEnumerable <CartItemViewModel> CartItems { get; set; }
        public decimal TotalPrice => CartItems.Sum(item => item.Price * item.Quantity);
    }
}
