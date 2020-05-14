using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;

namespace WebStore.DAL
{
    /// <summary>
    /// Класс для доступа к таблицам БД
    /// </summary>
    public class WebStoreContext: IdentityDbContext<User, Role, string> //до подключения Identity просто :DbContext
    {
        public WebStoreContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// Таблица БД с перечнем товаров
        /// </summary>
        public DbSet<Product> Products { get; set; }
        /// <summary>
        /// Таблица БД с перечнем брендов
        /// </summary>
        public DbSet<Brand> Brands { get; set; }
        /// <summary>
        /// Таблица БД с перечнем категорий
        /// </summary>
        public DbSet<Section> Sections { get; set; }
        /// <summary>
        /// Таблица БД с перечнем заказов
        /// </summary>
        public DbSet<Order> Orders { get; set; }
        /// <summary>
        /// Таблица БД с перечнем заказанных товаров
        /// </summary>
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
