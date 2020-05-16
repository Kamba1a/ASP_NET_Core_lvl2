using System.Linq;
using WebStore.DAL;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Services
{
    public class SqlCatalogData : ICatalogData
    {
        WebStoreContext _webStoreContext;

        public SqlCatalogData(WebStoreContext webStoreContext)
        {
            _webStoreContext = webStoreContext;
        }


        public IQueryable<Brand> GetBrands()
        {
            return _webStoreContext.Brands;
        }

        public IQueryable<Product> GetProducts(ProductFilter filter=null)
        {
            IQueryable<Product> products = _webStoreContext.Products;

            if (filter != null)
            {
                if (filter.SectionId.HasValue)
                    products = products.Where(product => product.SectionId.Equals(filter.SectionId));
                if (filter.BrandId.HasValue)
                    products = products.Where(product => product.BrandId == filter.BrandId);
                if (filter.ProductsIdList != null)
                    products = products.Where(product => filter.ProductsIdList.Contains(product.Id));
            }

            return products;
        }

        public IQueryable<Section> GetSections()
        {
            return _webStoreContext.Sections;
        }

        public Product GetProductById(int id)
        {
            return GetProducts().FirstOrDefault(product => product.Id==id);

            //вариант из методички, который сразу подтягивает модель бренда в товаре (.Include) и можно обойтись без доп.метода GetBrandById (но только если входной тип - DbSet)
            //return _webStoreContext
            //           .Products
            //           .Include(p => p.Brand)
            //           .Include(p => Section)
            //           .FirstOrDefault(p => p.Id == id);

        }

        public Brand GetBrandById(int id)
        {
            return GetBrands().FirstOrDefault(brand => brand.Id==id);
        }
    }
}
