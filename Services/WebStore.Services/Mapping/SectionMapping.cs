using WebStore.Domain.DTO.Catalog;
using WebStore.Domain.Entities;

namespace WebStore.Services.Mapping
{
    public static class SectionMapping
    {
        public static SectionDTO ToDTO(this Section section) => section is null ? null : new SectionDTO
        {
            Id = section.Id,
            Name = section.Name
        };

        public static Section FromDTO(this SectionDTO model) => model is null ? null : new Section
        {
            Id = model.Id,
            Name = model.Name
        };
    }
}
