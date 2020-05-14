namespace WebStore.Domain.Models
{
    /// <summary>
    /// Содержит корзину и данные для заказа
    /// </summary>
    public class OrderViewModel
    {
        public CartViewModel Cart { get; set; }
        public OrderDetailsViewModel OrderData { get; set; }

        //public decimal TotalPrice
        //{
        //    get
        //    {
        //        if (Cart.CartItems.Count > 0)
        //        {
        //            decimal price = 0;
        //            foreach (KeyValuePair<ProductViewModel,int> kvp in Cart.CartItems) price += kvp.Key.Price * kvp.Value;
        //            return price;
        //        }
        //        else return 0;
        //    }
        //}
    }
}
