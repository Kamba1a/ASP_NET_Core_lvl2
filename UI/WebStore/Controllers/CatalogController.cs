using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
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
            IQueryable<Product> products = _catalogData.GetProducts(new ProductFilter { BrandId = brandId, SectionId = sectionId });

            CatalogViewModel catalogViewModel = new CatalogViewModel
            {
                BrandId = brandId,
                SectionId = sectionId,
                Products = products.ToView()
            };

            return View(catalogViewModel);
        }

        public IActionResult ProductDetails(int productId)
        {
            Product product = _catalogData.GetProductById(productId);
            if (product == null) return NotFound();
            return View(product.ToView());
        }
    }
}