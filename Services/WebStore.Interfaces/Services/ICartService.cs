using WebStore.Domain.Entities;
using WebStore.Domain.Models;

namespace WebStore.Interfaces.Services
{
    public interface ICartService
    {
        Cart GetCart();
        /// <summary>
        /// Уменьшить количество товара в корзине
        /// </summary>
        /// <param name="productId"></param>
        void DecrementProductQuantity(int productId);
        /// <summary>
        /// Удалить товар из корзины
        /// </summary>
        /// <param name="productId"></param>
        void RemoveProductFromCart(int productId);
        /// <summary>
        /// Очистить корзину
        /// </summary>
        void RemoveAllProductsFromCart();
        /// <summary>
        /// Добавляет товар в корзину или увеличивает его количество
        /// </summary>
        /// <param name="productId"></param>
        void AddToCart(int productId);
        /// <summary>
        /// Получает корзину в формате CartViewModel
        /// </summary>
        /// <returns></returns>
        CartViewModel TransformCartToViewModel();
    }
}
