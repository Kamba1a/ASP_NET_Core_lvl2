using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.DTO.Catalog
{
    public class BrandDTO : IBaseEntity, INamedEntity, IOrderedEntity
    {
        public int Id { get; set ; }
        public string Name { get; set; }
        public int Order { get; set; }
    }
}
