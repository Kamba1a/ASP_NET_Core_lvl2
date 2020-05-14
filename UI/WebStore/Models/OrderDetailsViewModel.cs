using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain;

namespace WebStore.Models
{
    /// <summary>
    /// Содержит данные для заказа
    /// </summary>
    public class OrderDetailsViewModel
    {
        /// <summary>
        /// имя пользователя
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// адрес доставки
        /// </summary>
        [Required]
        public string Address { get; set; }

        /// <summary>
        /// телефон для связи
        /// </summary>
        [Required, DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
    }
}
