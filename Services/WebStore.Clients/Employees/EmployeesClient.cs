using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Clients.Base;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Employees
{
    public class EmployeesClient : BaseClient, IEmployeesService
    {
        public EmployeesClient(IConfiguration configuration) : base(configuration, WebAPI.Employees)
        { 
        }

        //все методы реализуются через базовый класс клиента в соответствии с типом запроса, и направляют запросы методам контроллера

        public void Add(Employee employee)
        {
            Post(_ServiceAddress, employee);
        }

        public bool Delete(int id)
        {
            return Delete($"{_ServiceAddress}/{id}").IsSuccessStatusCode;
        }

        public void Edit(int id, Employee employee)
        {
            Put($"{_ServiceAddress}/{id}", employee);
        }

        public IEnumerable<Employee> GetAll()
        {
            return Get<IEnumerable<Employee>>(_ServiceAddress);
        }

        public Employee GetById(int id)
        {
            return Get<Employee>($"{_ServiceAddress}/{id}");
        }

        public void SaveChanges()
        {
            //реализация не нужна
        }
    }
}
