using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services
{
    /// <summary>Сервис для получения и записи данных корзины в cookies</summary>
    public interface ICartStore
    {
        Cart Cart { get; set; } 
    }
}
