using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Models
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
