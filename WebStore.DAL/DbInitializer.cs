using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using WebStore.Domain;
using WebStore.Domain.Entities;

namespace WebStore.DAL
{
    /// <summary>
    /// Класс для наполнения таблиц в БД начальными данными
    /// </summary>
    public static class DbInitializer
    {
        /// <summary>
        /// Наполняет таблицы БД начальными данными
        /// </summary>
        /// <param name="webStoreContext"></param>
        public static void Initialize(WebStoreContext webStoreContext)
        {
            //проверяет, создана ли БД (если нет, то создает ее? непонятно зачем, если нужно апдейтить через миграции, а не так)
            webStoreContext.Database.EnsureCreated();

            //Проверяет - если есть хоть один элемент в таблице, то ничего не делаем
            if (webStoreContext.Products.Any())
            {
                return;
            }

            //просто список с данными, которыми хотим напонить таблицу в базе
            List<Section> sections = new List<Section>()
            {
                new Section()
                {
                    Id = 1,
                    Name = "Sportswear",
                    Order = 0,
                    ParentId = null
                },
                new Section()
                {
                    Id = 2,
                    Name = "Nike",
                    Order = 0,
                    ParentId = 1
                },
                new Section()
                {
                    Id = 3,
                    Name = "Under Armour",
                    Order = 1,
                    ParentId = 1
                },
                new Section()
                {
                    Id = 4,
                    Name = "Adidas",
                    Order = 2,
                    ParentId = 1
                },
                new Section()
                {
                    Id = 5,
                    Name = "Puma",
                    Order = 3,
                    ParentId = 1
                },
                new Section()
                {
                    Id = 6,
                    Name = "ASICS",
                    Order = 4,
                    ParentId = 1
                },
                new Section()
                {
                    Id = 7,
                    Name = "Mens",
                    Order = 1,
                    ParentId = null
                },
                new Section()
                {
                    Id = 8,
                    Name = "Fendi",
                    Order = 0,
                    ParentId = 7
                },
                new Section()
                {
                    Id = 9,
                    Name = "Guess",
                    Order = 1,
                    ParentId = 7
                },
                new Section()
                {
                    Id = 10,
                    Name = "Valentino",
                    Order = 2,
                    ParentId = 7
                },
                new Section()
                {
                    Id = 11,
                    Name = "Dior",
                    Order = 3,
                    ParentId = 7
                },
                new Section()
                {
                    Id = 12,
                    Name = "Versace",
                    Order = 4,
                    ParentId = 7
                },
                new Section()
                {
                    Id = 13,
                    Name = "Armani",
                    Order = 5,
                    ParentId = 7
                },
                new Section()
                {
                    Id = 14,
                    Name = "Prada",
                    Order = 6,
                    ParentId = 7
                },
                new Section()
                {
                    Id = 15,
                    Name = "Dolce and Gabbana",
                    Order = 7,
                    ParentId = 7
                },
                new Section()
                {
                    Id = 16,
                    Name = "Chanel",
                    Order = 8,
                    ParentId = 7
                },
                new Section()
                {
                    Id = 17,
                    Name = "Gucci",
                    Order = 1,
                    ParentId = 7
                },
                new Section()
                {
                    Id = 18,
                    Name = "Womens",
                    Order = 2,
                    ParentId = null
                },
                new Section()
                {
                    Id = 19,
                    Name = "Fendi",
                    Order = 0,
                    ParentId = 18
                },
                new Section()
                {
                    Id = 20,
                    Name = "Guess",
                    Order = 1,
                    ParentId = 18
                },
                new Section()
                {
                    Id = 21,
                    Name = "Valentino",
                    Order = 2,
                    ParentId = 18
                },
                new Section()
                {
                    Id = 22,
                    Name = "Dior",
                    Order = 3,
                    ParentId = 18
                },
                new Section()
                {
                    Id = 23,
                    Name = "Versace",
                    Order = 4,
                    ParentId = 18
                },
                new Section()
                {
                    Id = 24,
                    Name = "Kids",
                    Order = 3,
                    ParentId = null
                },
                new Section()
                {
                    Id = 25,
                    Name = "Fashion",
                    Order = 4,
                    ParentId = null
                },
                new Section()
                {
                    Id = 26,
                    Name = "Households",
                    Order = 5,
                    ParentId = null
                },
                new Section()
                {
                    Id = 27,
                    Name = "Interiors",
                    Order = 6,
                    ParentId = null
                },
                new Section()
                {
                    Id = 28,
                    Name = "Clothing",
                    Order = 7,
                    ParentId = null
                },
                new Section()
                {
                    Id = 29,
                    Name = "Bags",
                    Order = 8,
                    ParentId = null
                },
                new Section()
                {
                    Id = 30,
                    Name = "Shoes",
                    Order = 9,
                    ParentId = null
                }
            };

            //перенос данных в базу
            using (IDbContextTransaction transaction = webStoreContext.Database.BeginTransaction())
            {
                //переносим данные из List в Context-таблицу
                foreach (Section section in sections)
                {
                    webStoreContext.Sections.Add(section);
                }

                //следующие команды нужны для того, чтобы можно было сохранить в полях Id таблицы БД свои значения
                webStoreContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Sections] ON"); //отключаем проверку внешних ключей
                webStoreContext.SaveChanges(); //сохраняем изменения, сделанные в Context, в БД
                webStoreContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Sections] OFF");

                //сохраняем данные о проведенных изменениях?
                transaction.Commit();
            }

            List<Brand> brands = new List<Brand>()
            {
                new Brand()
                {
                    Id = 1,
                    Name = "Acne",
                    Order = 0
                },
                new Brand()
                {
                    Id = 2,
                    Name = "Grüne Erde",
                    Order = 1
                },
                new Brand()
                {
                    Id = 3,
                    Name = "Albiro",
                    Order = 2
                },
                new Brand()
                {
                    Id = 4,
                    Name = "Ronhill",
                    Order = 3
                },
                new Brand()
                {
                    Id = 5,
                    Name = "Oddmolly",
                    Order = 4
                },
                new Brand()
                {
                    Id = 6,
                    Name = "Boudestijn",
                    Order = 5
                },
                new Brand()
                {
                    Id = 7,
                    Name = "Rösch creative culture",
                    Order = 6
                },
            };
            using (var transaction = webStoreContext.Database.BeginTransaction())
            {
                foreach (Brand brand in brands)
                {
                    webStoreContext.Brands.Add(brand);
                }

                webStoreContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Brands] ON");
                webStoreContext.SaveChanges();
                webStoreContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Brands] OFF");
                transaction.Commit();
            }

            List<Product> products = new List<Product>()
            {
                new Product()
                {
                    Id = 1,
                    Name = "Easy Polo Black Edition",
                    Price = 1025,
                    ImageUrl = "product1.jpg",
                    Order = 0,
                    SectionId = 2,
                    BrandId = 1
                },
                new Product()
                {
                    Id = 2,
                    Name = "Easy Polo Black Edition",
                    Price = 1025,
                    ImageUrl = "product2.jpg",
                    Order = 1,
                    SectionId = 2,
                    BrandId = 1
                },
                new Product()
                {
                    Id = 3,
                    Name = "Easy Polo Black Edition",
                    Price = 1025,
                    ImageUrl = "product3.jpg",
                    Order = 2,
                    SectionId = 2,
                    BrandId = 1
                },
                new Product()
                {
                    Id = 4,
                    Name = "Easy Polo Black Edition",
                    Price = 1025,
                    ImageUrl = "product4.jpg",
                    Order = 3,
                    SectionId = 2,
                    BrandId = 1
                },
                new Product()
                {
                    Id = 5,
                    Name = "Easy Polo Black Edition",
                    Price = 1025,
                    ImageUrl = "product5.jpg",
                    Order = 4,
                    SectionId = 2,
                    BrandId = 2
                },
                new Product()
                {
                    Id = 6,
                    Name = "Easy Polo Black Edition",
                    Price = 1025,
                    ImageUrl = "product6.jpg",
                    Order = 5,
                    SectionId = 2,
                    BrandId = 2
                },
                new Product()
                {
                    Id = 7,
                    Name = "Easy Polo Black Edition",
                    Price = 1025,
                    ImageUrl = "product7.jpg",
                    Order = 6,
                    SectionId = 2,
                    BrandId = 2
                },
                new Product()
                {
                    Id = 8,
                    Name = "Easy Polo Black Edition",
                    Price = 1025,
                    ImageUrl = "product8.jpg",
                    Order = 7,
                    SectionId = 25,
                    BrandId = 2
                },
                new Product()
                {
                    Id = 9,
                    Name = "Easy Polo Black Edition",
                    Price = 1025,
                    ImageUrl = "product9.jpg",
                    Order = 8,
                    SectionId = 25,
                    BrandId = 2
                },
                new Product()
                {
                    Id = 10,
                    Name = "Easy Polo Black Edition",
                    Price = 1025,
                    ImageUrl = "product10.jpg",
                    Order = 9,
                    SectionId = 25,
                    BrandId = 3
                },
                new Product()
                {
                    Id = 11,
                    Name = "Easy Polo Black Edition",
                    Price = 1025,
                    ImageUrl = "product11.jpg",
                    Order = 10,
                    SectionId = 25,
                    BrandId = 3
                },
                new Product()
                {
                    Id = 12,
                    Name = "Easy Polo Black Edition",
                    Price = 1025,
                    ImageUrl = "product12.jpg",
                    Order = 11,
                    SectionId = 25,
                    BrandId = 3
                },
            };

            using (var transaction = webStoreContext.Database.BeginTransaction())
            {
                foreach (Product product in products)
                {
                    webStoreContext.Products.Add(product);
                }
                webStoreContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Products] ON");
                webStoreContext.SaveChanges();
                webStoreContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Products] OFF");
                transaction.Commit();
            }
        }

        /// <summary>
        /// Заносит в БД роли пользователей
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void InitializeUsers(IServiceProvider serviceProvider)
        {
            RoleManager<IdentityRole> roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            //добавляем роли
            EnsureRole(roleManager, "Users");
            EnsureRole(roleManager, "Admins");

            //добавляем пользователя администратора
            EnsureRoleToUser(serviceProvider, "Admin", "Admins", "admin123");
        }

        /// <summary>
        /// Добавляет в БД роль, если ее нет
        /// </summary>
        /// <param name="roleManager"></param>
        /// <param name="roleName"></param>
        private static void EnsureRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!roleManager.RoleExistsAsync(roleName).Result) //если проверка на наличие роли в БД не подтвердилась
            {
                roleManager.CreateAsync(new IdentityRole(roleName)).Wait(); //то добавляем роль
            }
        }

        /// <summary>
        /// Добавляет в БД пользователя с указанной ролью, если такого нет 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="userName"></param>
        /// <param name="roleName"></param>
        /// <param name="password"></param>
        private static void EnsureRoleToUser(IServiceProvider serviceProvider, string userName, string roleName, string password)
        {
            UserManager<User> userManager = serviceProvider.GetService<UserManager<User>>();
            IUserStore<User> userStore = serviceProvider.GetService<IUserStore<User>>();

            if (userStore.FindByNameAsync(userName, CancellationToken.None).Result != null) return; // если пользователь с таким именем уже есть, то выходим

            User user = new User // создаем пользователя под указанным именем
            {
                UserName = userName,
                Email = $"{userName}@domain.com"
            };

            if (userManager.CreateAsync(user, password).Result.Succeeded) // добавляем пользователя в БД
            {
                userManager.AddToRoleAsync(user, roleName).Wait(); // если пользователь успешно добавлен, то даем ему указанную роль
            }
        }
    }
}
