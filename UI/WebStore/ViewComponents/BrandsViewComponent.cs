using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<BrandViewModel> brands = GetBrands();
            return View(brands);
        }

        private List<BrandViewModel> GetBrands()
        {
            IQueryable<Brand> allBrands = _catalogData.GetBrands();
            List<BrandViewModel> allBrandsList = allBrands.Select(brand => new BrandViewModel
            {
                Id = brand.Id,
                Name = brand.Name,
                Order = brand.Order,
                //ProductsCount = GetProductsCount(brand.Id) //так выдает ошибку "There is already an open DataReader associated with this Command which must be closed first"
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

            foreach (Product product in _catalogData.GetProducts())
            {
                if (product.BrandId == brandId) productCount = productCount + 1;
            }

            return productCount;
        }
    }
}
