using System.Collections.Generic;
using WebStore.Domain;
using WebStore.Domain.DTO.Catalog;
using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services
{
    /// <summary>Сервис для работы с каталогом товаров</summary>
    public interface ICatalogData
    {
        /// <summary>Коллекция секций</summary>
        /// <returns></returns>
        IEnumerable<SectionDTO> GetSections();

        /// <summary>Получить секцию по идентификатору</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SectionDTO GetSectionById(int id);

        /// <summary>Коллекция брендов</summary>
        /// <returns></returns>
        IEnumerable<BrandDTO> GetBrands();

        /// <summary>Получить бренд по идентификатору</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BrandDTO GetBrandById(int id);

        /// <summary>Коллекция товаров</summary>
        /// <param name="filter">Фильтр товаров</param>
        /// <returns></returns>
        PageProductsDTO GetProducts(ProductFilter filter=null);

        /// <summary>Получить товар по идентификатору</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ProductDTO GetProductById(int id);
    }
}