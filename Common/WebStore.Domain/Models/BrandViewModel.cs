using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Models
{
    public class BrandViewModel : INamedEntity, IOrderedEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        /// <summary>Количество товаров в бренде</summary>
        public int ProductsCount { get; set; }

        public int Order { get; set; }

        /// <summary>Выбран ли бренд в представлении</summary>
        public bool CurrentBrandId { get; set; }
    }
}
