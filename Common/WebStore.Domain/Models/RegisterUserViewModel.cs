using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebStore.Domain.Models
{
    /// <summary>Модель представления полей для регистрации пользователя</summary>
    public class RegisterUserViewModel
    {
        [Required, MaxLength(256)]
        [Remote("IsNameFree", "Account")]   //подключаем метод IsNameFree в контроллере Account для своей валидации с помощью ненавязчивого AJAX
        public string UserName { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
