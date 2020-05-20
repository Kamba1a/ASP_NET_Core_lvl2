using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebStore.Controllers
{
    //[Example_SimpleActionFilter] //через атрибут можно подключить фильтр к целому контроллеру или отдельным экшн-методам
    //[Route("users")] //пример маршрутизации атрибутами (путь будет не employee, а users)
    [Authorize] //для всех методов контроллера требуется авторизация, если в методе не указано [AllowAnonymous]
    public class EmployeeController : Controller
    {
        IEmployeesService _employees;

        public EmployeeController(IEmployeesService employees)
        {
            _employees = employees;
        }

        //GET: /<controller>/employees
        //[Route("all")] //пример маршрутизации атрибутами (// GET: users/all)
        [AllowAnonymous] //просматривать список могут все
        public IActionResult Employees()
        {
            return View(_employees.GetAll().ToView());
        }

        //GET: /<controller>/employees/details/{id}
        //[Route("{id}")] //пример маршрутизации атрибутами (// GET: /users/{id})
        [Authorize(Roles = "Admins, Users")] //для просмотра деталей о работнике нужна роль админа или юзера
        public IActionResult Details(int id)
        {
            return View(_employees.GetById(id).ToView());
        }

        //GET: /<controller>/employees/edit/{id?}
        [Authorize(Roles ="Admins")] //для редактирования работника требуется роль админа
        public IActionResult Edit(int? id)
        {
            if (!id.HasValue) return View(new EmployeeViewModel());
            Employee employee = _employees.GetById(id.Value);
            if (employee==null) return NotFound();
            return View(employee.ToView());
        }

        [HttpPost, Authorize(Roles = "Admins")]
        public IActionResult Edit(EmployeeViewModel model)
        {
            //помимо использования атрибутов в модели, можно добавить проверку валидации в экшн-методе контроллера:
            //if (_employee.Age < 18 || _employee.Age > 99) ModelState.AddModelError("Age", "Некорректно указан возраст");

            if (!ModelState.IsValid) return View(model); //валидация

            Employee employee = _employees.GetById(model.Id);

            if (employee == null) _employees.Add(model.FromView());
            else _employees.Edit(employee.Id, model.FromView());

            _employees.SaveChanges();
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
