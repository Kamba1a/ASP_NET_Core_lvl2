using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebStore.Domain.DTO.Catalog;
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

        public static ProductDTO ToDTO(this Product product) => product is null ? null : new ProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            Order = product.Order,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            Brand = product.Brand.ToDTO(),
            Section = product.Section.ToDTO(),
        };

        public static Product FromDTO(this ProductDTO model) => model is null ? null : new Product
        {
            Id = model.Id,
            Name = model.Name,
            Order = model.Order,
            Price = model.Price,
            ImageUrl = model.ImageUrl,
            BrandId = model.Brand.Id,
            Brand = model.Brand.FromDTO(),
            SectionId = model.Section.Id,
            Section = model.Section.FromDTO(),
        };

        public static IEnumerable<Product> FromDTO(this IEnumerable<ProductDTO> Products) => Products?.Select(FromDTO);

        public static IEnumerable<ProductDTO> ToDTO(this IEnumerable<Product> Products) => Products?.Select(ToDTO);
    }
}
