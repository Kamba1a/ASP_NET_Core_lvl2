using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле \"Имя\" является обязательным")]
        [StringLength(50, ErrorMessage = "Недопустимая длина строки в поле \"Имя\"")]
        [Display(Name ="Имя")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле \"Фамилия\" является обязательным")]
        [StringLength(50, ErrorMessage = "Недопустимая длина строки в поле \"Фамилия\"")]
        [DisplayName("Фамилия")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле \"Отчество\" является обязательным")]
        [StringLength(50, ErrorMessage = "Недопустимая длина строки в поле \"Отчество\"")]
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле \"Возраст\" является обязательным")]
        [Range(18,99, ErrorMessage = "Возраст не может быть меньше 18 или больше 99 лет")]
        [Display(Name = "Возраст")]
        public int Age { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле \"Должность\" является обязательным")]
        [StringLength(500, ErrorMessage = "Недопустимая длина строки в поле \"Должность\"")]
        [Display(Name = "Должность")]
        public string Position { get; set; }
    }
}
