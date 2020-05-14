namespace WebStore.Domain.Models
{
    public class CartItemViewModel
    {
        public ProductViewModel Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price => Product.Price;
    }
}
