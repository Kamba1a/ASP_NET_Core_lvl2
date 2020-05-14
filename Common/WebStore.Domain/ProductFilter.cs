using System.Collections.Generic;

namespace WebStore.Domain
{
    /// <summary>
    /// Класс для фильтрации товаров
    /// </summary>
    public class ProductFilter
    {
        /// <summary>
        /// Секция, к которой принадлежит товар
        /// </summary>
        public int? SectionId { get; set; }

        /// <summary>
        /// Бренд товара
        /// </summary>
        public int? BrandId { get; set; }
        /// <summary>
        /// Перечень ID товаров
        /// </summary>
        public List<int> ProductsIdList { get; set; }
    }
}
