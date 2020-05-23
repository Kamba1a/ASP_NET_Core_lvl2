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
        /// <summary>
        /// OrdersApiController
        /// </summary>
        public const string Orders = "api/orders";


        // класс в классе - для удобства, просто чтобы выделить вложенные в класс константы
        public static class Identity
            {
            /// <summary>
             /// UsersApiController
             /// </summary>
            public const string Users = "api/users";
            /// <summary>
            /// RolesApiController
            /// </summary>
            public const string Roles = "api/roles";
        }
    }
}
