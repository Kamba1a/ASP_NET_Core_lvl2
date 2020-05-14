using System.Linq;
using WebStore.Domain;
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
        IQueryable<Section> GetSections();

        /// <summary>
        /// Перечень брендов
        /// </summary>
        /// <returns></returns>
        IQueryable<Brand> GetBrands();

        /// <summary>
        /// Перечень товаров
        /// </summary>
        /// <param name="filter">Фильтр товаров</param>
        /// <returns></returns>
        IQueryable<Product> GetProducts(ProductFilter filter=null);

        /// <summary>
        /// Товар по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Product GetProductById(int id);

        /// <summary>
        /// Бренд по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Brand GetBrandById(int id);
    }
}