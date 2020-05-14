using Microsoft.AspNetCore.Identity;

namespace WebStore.Domain.Entities.Identity
{
    public class User : IdentityUser //в некоторых случаях класс можно оставить пустым (см. первый курс)
    {
        public const string Administrator = "Admin";
        public const string AdminDefaultPassword = "admin123";
    }
}