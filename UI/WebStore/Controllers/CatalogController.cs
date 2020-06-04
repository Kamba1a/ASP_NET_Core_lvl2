using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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

        public CatalogController(ICatalogData catalogData)
        {
            _catalogData = catalogData;
        }

        public IActionResult Shop(int? sectionId, int? brandId)
        {
            IEnumerable<ProductDTO> products = _catalogData.GetProducts(new ProductFilter { BrandId = brandId, SectionId = sectionId }).Products;

            CatalogViewModel catalogViewModel = new CatalogViewModel
            {
                BrandId = brandId,
                SectionId = sectionId,
                Products = products.FromDTO().ToView()
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