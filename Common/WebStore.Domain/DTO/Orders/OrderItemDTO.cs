using WebStore.Domain.Entities.Base;

namespace WebStore.Domain.DTO.Orders
{
    /// <summary>Описание пункта заказа</summary>
    public class OrderItemDTO: BaseEntity
    {
        /// <summary>Идентификатор товара</summary>
        public int ProductId { get; set; }

        /// <summary>Цена одной единицы</summary>
        public decimal Price { get; set; }

        /// <summary>Количество</summary>
        public int Quantity { get; set; }
    }
}
