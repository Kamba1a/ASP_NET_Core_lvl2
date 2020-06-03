using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.DTO.Catalog;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;

namespace WebStore.ViewComponents
{
    public class BrandsViewComponent : ViewComponent
    {
        private readonly ICatalogData _catalogData;

        public BrandsViewComponent(ICatalogData catalogData)
        {
            _catalogData = catalogData;
        }

        public IViewComponentResult Invoke(string brandId)
        {
            int? currentBrandId = int.TryParse(brandId, out int id) ? (int?)id : null;

            List<BrandViewModel> brands = GetBrands(currentBrandId);
            return View(brands);
        }

        private List<BrandViewModel> GetBrands(int? currentBrandId)
        {
            IEnumerable<BrandDTO> allBrands = _catalogData.GetBrands();
            List<BrandViewModel> allBrandsList = allBrands.Select(brand => new BrandViewModel
            {
                Id = brand.Id,
                Name = brand.Name,
                Order = brand.Order,
                CurrentBrandId = (currentBrandId.HasValue ? brand.Id == currentBrandId.Value : false)
            }).OrderBy(b => b.Order).ToList();

            foreach(BrandViewModel brand in allBrandsList)
            {
                brand.ProductsCount = GetProductsCount(brand.Id);
            }

            return allBrandsList;
        }

        private int GetProductsCount(int brandId)
        {
            int productCount = 0;

            foreach (ProductDTO product in _catalogData.GetProducts())
            {
                if (product.Brand.Id == brandId) productCount = productCount + 1;
            }

            return productCount;
        }
    }
}
