using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(WebAPI.Employees)]
    [ApiController]
    public class EmployeesApiController : ControllerBase, IEmployeesService //контроллер просто переадресовывает все вызовы на IEmployeesService
    {
        IEmployeesService _employeesService;

        public EmployeesApiController(IEmployeesService employeesService)
        {
            _employeesService = employeesService;
        }

        [HttpPost]
        public void Add([FromBody]Employee Employee) //атрибут [FromBody] ограничивает поиск модели только в теле сообщения (чтобы не получить ее из адресной строки), тут он только для явности
        {
            _employeesService.Add(Employee);
        }

        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return _employeesService.Delete(id);
        }

        [HttpPut("{id}")]
        public void Edit(int id, [FromBody]Employee employee)
        {
            _employeesService.Edit(id, employee);
        }

        [HttpGet]
        public IEnumerable<Employee> GetAll()
        {
            return _employeesService.GetAll();
        }

        [HttpGet("{id}")]
        public Employee GetById(int id)
        {
            return _employeesService.GetById(id);
        }

        [NonAction] //атрибут показывает, что метод снаружи никак не вызывается, т.к. нужен только для реализации интерфейса
        public void SaveChanges()
        {
            _employeesService.SaveChanges();
        }
    }
}