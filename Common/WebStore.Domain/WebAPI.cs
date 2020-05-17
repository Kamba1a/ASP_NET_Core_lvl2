using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.Domain
{
    //класс для хранения констант с путями доступа к конкретным API контроллерам
    public static class WebAPI
    {
        /// <summary>
        /// ValuesController
        /// </summary>
        public const string Values = "api/values";
        /// <summary>
        /// EmployeesAPIController
        /// </summary>
        public const string Employees = "api/employees";
        /// <summary>
        /// CatalogAPIController
        /// </summary>
        public const string Catalog = "api/catalog";
    }
}
