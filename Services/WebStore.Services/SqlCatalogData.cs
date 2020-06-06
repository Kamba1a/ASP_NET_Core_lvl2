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


        public IEnumerable<BrandDTO> GetBrands()
        {
            IQueryable<Brand> brands = _webStoreContext.Brands;
            return brands.ToDTO();
        }

        public PageProductsDTO GetProducts(ProductFilter filter=null)
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

            int totalCount = products.Count(); //итоговое количество товаров
            
            if (filter?.PageSize != null)       //если в фильтре указано количество товаров на странице
                products = products
                   .Skip((filter.Page - 1) * (int)filter.PageSize)  //пропускаем количество товаров, которые должны быть на предыдущих страницах
                   .Take((int)filter.PageSize);                     //оставляем только те товары и в том количестве, которые должны быть на текущей странице

            return new PageProductsDTO()
            {
                Products = products.ToDTO(),
                TotalCount = totalCount
            };
        }

        public IEnumerable<SectionDTO> GetSections()
        {
            IQueryable<Section> sections = _webStoreContext.Sections;
            return sections.ToDTO();
        }

        public ProductDTO GetProductById(int id)
        {
            return _webStoreContext.Products
                .Include(p => p.Brand)
                .Include(p => p.Section)
                .FirstOrDefault(p => p.Id == id)
                .ToDTO();
        }

        public SectionDTO GetSectionById(int id)
        {
            return _webStoreContext.Sections
                .FirstOrDefault(s => s.Id==id)
                .ToDTO();
        }

        public BrandDTO GetBrandById(int id)
        {
            return _webStoreContext.Brands
                .FirstOrDefault(b => b.Id == id)
                .ToDTO();
        }
    }
}
