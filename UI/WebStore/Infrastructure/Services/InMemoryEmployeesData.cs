using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Infrastructure.Interfaces;
using WebStore.Models;

namespace WebStore.Infrastructure.Services
{
    public class InMemoryEmployeesData: IitemData<EmployeeViewModel>
    {
        List<EmployeeViewModel> _employees;

        public InMemoryEmployeesData()
        {
            _employees = new List<EmployeeViewModel> {
                new EmployeeViewModel{Id=1, FirstName="Иван", LastName="Иванов", Patronymic="Иванович", Age=30, Position="Специалист"},
                new EmployeeViewModel{Id=2, FirstName="Петр", LastName="Петров", Patronymic="Петрович", Age=35, Position="Начальник отдела"}
            };
        }

        public IEnumerable<EmployeeViewModel> GetAll()
        {
            return _employees;
        }

        public EmployeeViewModel GetById(int id)
        {
            return _employees.FirstOrDefault(empl => empl.Id == id);
        }

        public void AddNew(EmployeeViewModel employee)
        {
            if (_employees.Count > 0) employee.Id = _employees.Last().Id + 1;
            else employee.Id = 1;
            _employees.Add(employee);
        }

        public void Delete(int id)
        {
            EmployeeViewModel employeer = GetById(id);
            if (employeer!=null) _employees.Remove(GetById(id));
        }

        public void Commit()
        {

        }
    }
}
