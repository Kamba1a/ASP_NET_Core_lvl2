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

        [HttpGet("brands/{id}")]
        public BrandDTO GetBrandById(int id)
        {
            return _catalogData.GetBrandById(id);
        }

        [HttpGet("brands")]
        public IEnumerable<BrandDTO> GetBrands()
        {
            return _catalogData.GetBrands();
        }

        [HttpGet("products/{id}")]
        public ProductDTO GetProductById(int id)
        {
            return _catalogData.GetProductById(id);
        }

        [HttpPost] //метод Post, т.к. нужно передать данные фильтра в теле сообщения, а не в строке запроса
        public PageProductsDTO GetProducts([FromBody]ProductFilter filter = null)
        {
            return _catalogData.GetProducts(filter);
        }

        [HttpGet("sections/{id}")]
        public SectionDTO GetSectionById(int id)
        {
            return _catalogData.GetSectionById(id);
        }

        [HttpGet("sections")]
        public IEnumerable<SectionDTO> GetSections()
        {
            return _catalogData.GetSections();
        }
    }
}