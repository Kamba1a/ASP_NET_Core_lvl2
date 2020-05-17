using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Domain.DTO.Catalog;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(WebAPI.Catalog)]
    [ApiController]
    public class CatalogApiController : ControllerBase, ICatalogData
    {
        ICatalogData _catalogData;

        public CatalogApiController(ICatalogData catalogData)
        {
            _catalogData = catalogData;
        }

        [HttpGet("brands")]
        public IEnumerable<Brand> GetBrands()
        {
            return _catalogData.GetBrands();
        }

        [HttpGet("products/{id}")]
        public ProductDTO GetProductById(int id)
        {
            return _catalogData.GetProductById(id);
        }

        [HttpPost] //метод Post, т.к. требуется передача фильтра
        public IEnumerable<ProductDTO> GetProducts([FromBody]ProductFilter filter = null)
        {
            return _catalogData.GetProducts(filter);
        }

        [HttpGet("sections")]
        public IEnumerable<Section> GetSections()
        {
            return _catalogData.GetSections();
        }
    }
}