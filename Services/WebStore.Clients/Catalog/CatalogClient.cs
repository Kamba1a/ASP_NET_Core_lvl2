using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using WebStore.Clients.Base;
using WebStore.Domain;
using WebStore.Domain.DTO.Catalog;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Catalog
{
    public class CatalogClient : BaseClient, ICatalogData
    {
        public CatalogClient(IConfiguration Configuration) : base(Configuration, WebAPI.Catalog)
        {
        }

        public BrandDTO GetBrandById(int id)
        {
            return Get<BrandDTO>($"{_ServiceAddress}/brands/{id}");
        }

        public IEnumerable<BrandDTO> GetBrands()
        {
            return Get<List<BrandDTO>>($"{_ServiceAddress}/brands");
        }

        public ProductDTO GetProductById(int id)
        {
            return Get<ProductDTO>($"{_ServiceAddress}/products/{id}");
        }

        public PageProductsDTO GetProducts(ProductFilter filter = null)
        {
            return Post(_ServiceAddress, filter ?? new ProductFilter())    //значение null не может быть передано, поэтому обязательно создаем пустой фильтр если
                .Content.ReadAsAsync<PageProductsDTO>().Result;
        }

        public SectionDTO GetSectionById(int id)
        {
            return Get<SectionDTO>($"{_ServiceAddress}/sections/{id}");
        }

        public IEnumerable<SectionDTO> GetSections()
        {
            return Get<List<SectionDTO>>($"{_ServiceAddress}/sections");
        }
    }
}
