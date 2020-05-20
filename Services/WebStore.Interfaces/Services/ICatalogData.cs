using System.Collections.Generic;
using WebStore.Domain;
using WebStore.Domain.DTO.Catalog;
using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services
{
    /// <summary>
    /// Интерфейс для работы с товарами
    /// </summary>
    public interface ICatalogData
    {
        /// <summary>
        /// Перечень секций
        /// </summary>
        /// <returns></returns>
        IEnumerable<Section> GetSections();

        /// <summary>
        /// Перечень брендов
        /// </summary>
        /// <returns></returns>
        IEnumerable<Brand> GetBrands();

        /// <summary>
        /// Перечень товаров
        /// </summary>
        /// <param name="filter">Фильтр товаров</param>
        /// <returns></returns>
        IEnumerable<ProductDTO> GetProducts(ProductFilter filter=null);

        /// <summary>
        /// Товар по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ProductDTO GetProductById(int id);
    }
}