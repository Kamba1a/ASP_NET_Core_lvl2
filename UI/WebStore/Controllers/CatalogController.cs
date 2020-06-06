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
        ICatalogData _catalogData;
        IConfiguration _configuration;

        public CatalogController(ICatalogData catalogData, IConfiguration configuration)
        {
            _catalogData = catalogData;
            _configuration = configuration;
        }

        public IActionResult Shop(int? sectionId, int? brandId, int page = 1)
        {
            int? pageSize = int.TryParse(_configuration["PageSize"], out int page_size) ? page_size : (int?)null;

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
    }
}