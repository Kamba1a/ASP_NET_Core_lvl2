using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.DTO.Catalog;
using WebStore.Domain.Entities;

namespace WebStore.Services.Mapping
{
    public static class SectionMapping
    {
        public static SectionDTO ToDTO(this Section section) => section is null ? null : new SectionDTO
        {
            Id = section.Id,
            Name = section.Name,
            Order = section.Order,
            ParentId = section.ParentId
        };

        public static Section FromDTO(this SectionDTO model) => model is null ? null : new Section
        {
            Id = model.Id,
            Name = model.Name,
            Order = model.Order,
            ParentId = model.ParentId
        };

        public static IEnumerable<SectionDTO> ToDTO(this IEnumerable<Section> sections)
        {
            return sections.Select(s => s.ToDTO());
        }
    }
}
