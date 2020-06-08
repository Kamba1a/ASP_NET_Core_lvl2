using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebStore.Domain;
using WebStore.Domain.DTO.Catalog;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ICatalogData _catalogData;
        private readonly IConfiguration _configuration;
        private const string PAGE_SIZE = "PageSize";

        public CatalogController(ICatalogData catalogData, IConfiguration configuration)
        {
            _catalogData = catalogData;
            _configuration = configuration;
        }

        public IActionResult Shop(int? sectionId, int? brandId, int page = 1)
        {
            int? pageSize = int.TryParse(_configuration[PAGE_SIZE], out int page_size) ? page_size : (int?)null;

            PageProductsDTO products = _catalogData.GetProducts(new ProductFilter
            {
                BrandId = brandId,
                SectionId = sectionId,
                Page = page,
                PageSize = pageSize
            });

            CatalogViewModel catalogViewModel = new CatalogViewModel
            {
                BrandId = brandId,
                SectionId = sectionId,
                Products = products.Products.FromDTO().ToView(),
                PageViewModel = new PageViewModel()
                {
                    PageNumber = page,
                    TotalItems = products.TotalCount,
                    PageSize = pageSize.GetValueOrDefault()
                }
            };

            return View(catalogViewModel);
        }

        public IActionResult ProductDetails(int productId)
        {
            ProductDTO product = _catalogData.GetProductById(productId);
            if (product == null) return NotFound();
            return View(product.FromDTO().ToView());
        }


        #region API

        public IActionResult GetFilteredItems(int? sectionId, int? brandId, int page)
        {
            ProductFilter productFilter = new ProductFilter()
            {
                SectionId = sectionId,
                BrandId = brandId,
                Page = page,
                PageSize = int.Parse(_configuration[PAGE_SIZE])
            };

            IEnumerable<ProductViewModel> products = _catalogData.GetProducts(productFilter).Products
                .FromDTO()  //ProductDTO => Product
                .ToView()   //Product => ProductViewModel
                .OrderBy(p => p.Order);

            return PartialView("Partial/_ProductItems", products);
        }

        #endregion
    }
}