using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;

namespace WebStore.Services.Mapping
{
    public static class ProductMapping
    {
        public static ProductViewModel ToView(this Product product)
        {
            return new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Order = product.Order,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                BrandName = product.Brand.Name
            };
        }

        public static IEnumerable<ProductViewModel> ToView(this IEnumerable<Product> products)
        {
            return products
                .Select(p => p.ToView())
                .OrderBy(p => p.Order);
        }
    }
}
