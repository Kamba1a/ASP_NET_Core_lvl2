using System.ComponentModel.DataAnnotations;

namespace WebStore.Domain.Models
{
    /// <summary>
    /// Класс с полями для авторизации пользователя
    /// </summary>
    public class LoginUserViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

        /// <summary>
        /// адрес для редиректа пользователя на страницу, на которой он был
        /// </summary>
        public string ReturnUrl { get; set; }
    }
}
