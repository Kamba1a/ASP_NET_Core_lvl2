using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.Domain.DTO.Catalog
{
    /// <summary>
    /// Модель, содержащая коллекцию товаров и их количество
    /// </summary>
    public class PageProductsDTO
    {
        /// <summary>Коллекция товаров</summary>
        public IEnumerable<ProductDTO> Products { get; set; }

        /// <summary>Количество товаров</summary>
        public int TotalCount { get; set; }
    }
}