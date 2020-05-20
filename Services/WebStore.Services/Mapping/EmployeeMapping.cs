using System.Collections.Generic;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;

namespace WebStore.Services.Mapping
{
    public static class EmployeeMapping
    {
        public static EmployeeViewModel ToView(this Employee e) => new EmployeeViewModel //!! this обязательно для возможности применения метода СРАЗУ к модели (напр. employee.ToView())
        {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.SurName,
            Patronymic = e.Patronymic,
            Age = e.Age,
            Position = e.Position
        };

        public static Employee FromView(this EmployeeViewModel e) => new Employee
        {
            FirstName = e.FirstName,
            SurName = e.LastName,
            Patronymic = e.Patronymic,
            Age = e.Age,
            Position = e.Position
        };

        public static IEnumerable<EmployeeViewModel> ToView(this IEnumerable<Employee> employees)
        {
            List<EmployeeViewModel> employeesList = new List<EmployeeViewModel>();
            foreach(var e in employees)
            {
                EmployeeViewModel employee = e.ToView();
                employeesList.Add(employee);
            }
            return employeesList;
        }
    }
}
