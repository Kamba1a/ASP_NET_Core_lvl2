using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using WebStore.Domain;
using WebStore.Domain.Entities;

namespace WebStore.DAL
{
    /// <summary>
    /// Класс для доступа к таблицам БД
    /// </summary>
    public class WebStoreContext: IdentityDbContext<User> //до подключения Identity просто :DbContext
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
