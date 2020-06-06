using System.Collections.Generic;

namespace WebStore.Domain
{
    /// <summary>Класс для фильтрации товаров</summary>
    public class ProductFilter
    {
        /// <summary>Фильтр по идентификатору секции товара</summary>
        public int? SectionId { get; set; }

        /// <summary>Фильтр по идетификатору бренда товара</summary>
        public int? BrandId { get; set; }
        
        /// <summary>Коллекция ID товаров, которые пропустит фильтр</summary>
        public List<int> ProductsIdList { get; set; }

        /// <summary>Номер текущей страницы</summary>
        public int Page { get; set; }

        /// <summary>Количество товаров на странице</summary>
        public int? PageSize { get; set; }
    }
}
