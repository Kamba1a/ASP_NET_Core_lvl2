using Microsoft.EntityFrameworkCore;
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
            IQueryable<Product> products = _webStoreContext.Products.Include(p=>p.Brand).Include(p=>p.Section);

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
            return _webStoreContext.Products
                .Include(p => p.Brand)
                .Include(p => p.Section)
                .FirstOrDefault(p => p.Id == id);
        }
    }
}
