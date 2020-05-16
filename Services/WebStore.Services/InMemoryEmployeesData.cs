using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Services
{
    public class InMemoryEmployeesService: IEmployeesService
    {
        List<Employee> _employees;

        public InMemoryEmployeesService()
        {
            _employees = new List<Employee> {
                new Employee{Id=1, FirstName="Иван", SurName="Иванов", Patronymic="Иванович", Age=30, Position="Специалист"},
                new Employee{Id=2, FirstName="Петр", SurName="Петров", Patronymic="Петрович", Age=35, Position="Начальник отдела"}
            };
        }

        public IEnumerable<Employee> GetAll()
        {
            return _employees;
        }

        public Employee GetById(int id)
        {
            return _employees.FirstOrDefault(empl => empl.Id == id);
        }

        public void Add(Employee employee)
        {
            if (_employees.Count > 0) employee.Id = _employees.Last().Id + 1;
            else employee.Id = 1;
            _employees.Add(employee);
        }

        public void Edit(int id, Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(Employee));
            if (_employees.Contains(employee)) return;

            Employee e = GetById(id);
            if (e is null) return;

            e.FirstName = employee.FirstName;
            e.SurName = employee.SurName;
            e.Patronymic = employee.Patronymic;
            e.Age = employee.Age;
            e.Position = employee.Position;
        }

        public bool Delete(int id)
        {
            Employee employeer = GetById(id);
            if (employeer == null) return false;
            _employees.Remove(GetById(id));
            return true;
        }

        public void SaveChanges()
        {
        }
    }
}
