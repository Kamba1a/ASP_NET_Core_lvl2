namespace WebStore.Domain.Entities.Base.Interfaces
{
    /// <summary>Сущность имеющая номер для сортировки по порядку</summary>
    public interface IOrderedEntity
    {
        /// <summary>Номер для сортировки по порядку</summary>
        int Order { get; set; }
    }
}
