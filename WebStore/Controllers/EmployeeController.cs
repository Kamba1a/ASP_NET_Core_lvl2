using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Infrastructure;
using WebStore.Infrastructure.Interfaces;
using WebStore.Infrastructure.Services;
using WebStore.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebStore.Controllers
{
    //[Example_SimpleActionFilter] //через атрибут можно подключить фильтр к целому контроллеру или отдельным экшн-методам
    //[Route("users")] //пример маршрутизации атрибутами (путь будет не employee, а users)
    [Authorize] //для всех методов контроллера требуется авторизация, если в методе не указано [AllowAnonymous]
    public class EmployeeController : Controller
    {
        IitemData<EmployeeViewModel> _employees;

        public EmployeeController(IitemData<EmployeeViewModel> employees)
        {
            _employees = employees;
        }

        //GET: /<controller>/employees
        //[Route("all")] //пример маршрутизации атрибутами (// GET: users/all)
        [AllowAnonymous] //просматривать список могут все
        public IActionResult Employees()
        {
            return View(_employees.GetAll());
        }

        //GET: /<controller>/employees/details/{id}
        //[Route("{id}")] //пример маршрутизации атрибутами (// GET: /users/{id})
        [Authorize(Roles = "Admins, Users")] //для просмотра деталей о работнике нужна роль админа или юзера
        public IActionResult Details(int id)
        {
            return View(_employees.GetById(id));
        }

        //GET: /<controller>/employees/edit/{id?}
        [Authorize(Roles ="Admins")] //для редактирования работника требуется роль админа
        public IActionResult Edit(int? id)
        {
            if (!id.HasValue) return View(new EmployeeViewModel());
            EmployeeViewModel employee = _employees.GetById(id.Value);
            if (employee==null) return NotFound();
            return View(employee);
        }

        [HttpPost, Authorize(Roles = "Admins")]
        public IActionResult Edit(EmployeeViewModel _employee)
        {
            //помимо использования атрибутов в модели, можно добавить проверку валидации в экшн-методе контроллера:
            //if (_employee.Age < 18 || _employee.Age > 99) ModelState.AddModelError("Age", "Некорректно указан возраст");

            if (!ModelState.IsValid) return View(_employee); //валидация

            EmployeeViewModel employee = _employees.GetById(_employee.Id);
            if (employee == null) _employees.AddNew(_employee);
            else
            {
                employee.FirstName = _employee.FirstName;
                employee.LastName = _employee.LastName;
                employee.Patronymic = _employee.Patronymic;
                employee.Age = _employee.Age;
                employee.Position = _employee.Position;
            }
            _employees.Commit();
            return RedirectToAction("Employees");
        }

        [Authorize(Roles = "Admins")]
        public IActionResult Delete(int id)
        {
            _employees.Delete(id);
            return RedirectToAction("Employees");
        }
    }
}
