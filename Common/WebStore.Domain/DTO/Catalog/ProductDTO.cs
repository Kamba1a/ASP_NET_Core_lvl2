using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.DTO.Catalog
{
    //модель нужна для работы с WebAPI, чтобы избежать проблем с сериализацией данных в json
    //(проблемы возникают, когда модели ссылаются друг на друга, вызывая зацикливание сериализатора до бесконечности)
    //модели DTO создаются специально с учетом, чтобы не было такой зацикленности
    public class ProductDTO: IBaseEntity, INamedEntity, IOrderedEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public BrandDTO Brand { get; set; }
        public SectionDTO Section { get; set; }
    }
}
