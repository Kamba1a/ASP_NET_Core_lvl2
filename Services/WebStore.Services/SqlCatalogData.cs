using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebStore.DAL;
using WebStore.Domain;
using WebStore.Domain.DTO.Catalog;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Services
{
    public class SqlCatalogData : ICatalogData
    {
        WebStoreContext _webStoreContext;

        public SqlCatalogData(WebStoreContext webStoreContext)
        {
            _webStoreContext = webStoreContext;
        }


        public IEnumerable<Brand> GetBrands()
        {
            IQueryable<Brand> brands = _webStoreContext.Brands;
            return brands;
        }

        public IEnumerable<ProductDTO> GetProducts(ProductFilter filter=null)
        {
            IQueryable<Product> products = _webStoreContext.Products.Include(p=>p.Brand).Include(p=>p.Section);

            if (filter != null)
            {
                if (filter.SectionId.HasValue)
                    products = products.Where(product => product.Section.Id.Equals(filter.SectionId));
                if (filter.BrandId.HasValue)
                    products = products.Where(product => product.Brand.Id == filter.BrandId);
                if (filter.ProductsIdList != null)
                    products = products.Where(product => filter.ProductsIdList.Contains(product.Id));
            }

            return products.ToDTO();
        }

        public IEnumerable<Section> GetSections()
        {
            IQueryable<Section> sections = _webStoreContext.Sections;
            return sections;
        }

        public ProductDTO GetProductById(int id)
        {
            return _webStoreContext.Products
                .Include(p => p.Brand)
                .Include(p => p.Section)
                .FirstOrDefault(p => p.Id == id)
                .ToDTO();
        }
    }
}
