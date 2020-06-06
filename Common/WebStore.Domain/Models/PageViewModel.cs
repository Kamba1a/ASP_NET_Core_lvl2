using System;

namespace WebStore.Domain.Models
{
    /// <summary>Модель для постраничного отображения списка товаров в каталоге магазина</summary>
    public class PageViewModel
    {
        /// <summary>Общее количество товаров в запросе</summary>
        public int TotalItems { get; set; }

        /// <summary>Количество товаров на одной странице</summary>
        public int PageSize { get; set; }

        /// <summary>Номер текущей страницы</summary>
        public int PageNumber { get; set; }

        /// <summary>Общее количество страниц</summary>
        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
    }
}