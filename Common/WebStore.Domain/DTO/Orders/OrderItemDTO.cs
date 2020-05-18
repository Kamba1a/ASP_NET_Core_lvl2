using WebStore.Domain.Entities.Base;

namespace WebStore.Domain.DTO.Orders
{
    public class OrderItemDTO: BaseEntity
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
