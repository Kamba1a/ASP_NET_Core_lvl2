using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;

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
                Products = products.Select(product => new ProductViewModel
                {
                    Id = product.Id,
                    ImageUrl = product.ImageUrl,
                    Name = product.Name,
                    Order = product.Order,
                    Price = product.Price
                }).OrderBy(p => p.Order).ToList()
            };

            return View(catalogViewModel);
        }

        public IActionResult ProductDetails(int productId)
        {
            Product product = _catalogData.GetProductById(productId);
            if (product == null) return NotFound();

            ProductViewModel productViewModel = new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                BrandName = _catalogData.GetBrandById(product.BrandId).Name ?? String.Empty
            };

            return View(productViewModel);
        }
    }
}