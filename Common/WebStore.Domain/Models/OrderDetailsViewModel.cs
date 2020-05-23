using System.ComponentModel.DataAnnotations;

namespace WebStore.Domain.Models
{
    /// <summary>Модель информации о заказе</summary>
    public class OrderDetailsViewModel
    {
        /// <summary>Имя</summary>
        [Required]
        public string Name { get; set; }

        /// <summary>Адрес доставки</summary>
        [Required]
        public string Address { get; set; }

        /// <summary>Номер телефона для связи</summary>
        [Required, DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
    }
}
