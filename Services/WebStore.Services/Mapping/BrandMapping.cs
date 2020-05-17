using WebStore.Domain.DTO.Catalog;
using WebStore.Domain.Entities;

namespace WebStore.Services.Mapping
{
    public static class BrandMapping
    {
        public static BrandDTO ToDTO(this Brand brand) => brand is null ? null : new BrandDTO
        {
            Id = brand.Id,
            Name = brand.Name
        };

        public static Brand FromDTO(this BrandDTO model) => model is null ? null : new Brand
        {
            Id = model.Id,
            Name = model.Name
        };
    }
}
