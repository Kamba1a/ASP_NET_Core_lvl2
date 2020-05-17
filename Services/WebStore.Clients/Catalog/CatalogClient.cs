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

        public IEnumerable<Brand> GetBrands()
        {
            return Get<List<Brand>>($"{_ServiceAddress}/brands");
        }

        public ProductDTO GetProductById(int id)
        {
            return Get<ProductDTO>($"{_ServiceAddress}/products/{id}");
        }

        public IEnumerable<ProductDTO> GetProducts(ProductFilter filter = null)
        {
            return Post(_ServiceAddress, filter ?? new ProductFilter())    //значение null не может быть передано, поэтому обязательно создаем пустой фильтр если
                .Content.ReadAsAsync<IEnumerable<ProductDTO>>().Result;
        }

        public IEnumerable<Section> GetSections()
        {
            return Get<List<Section>>($"{_ServiceAddress}/sections");
        }
    }
}
