namespace WebStore.Domain.Entities.Base.Interfaces
{
    /// <inheritdoc />
    /// <summary>Сущность имеющая наименование</summary>
    public interface INamedEntity : IBaseEntity
    {
        /// <summary>Наименование</summary>
        string Name { get; set; }
    }
}
