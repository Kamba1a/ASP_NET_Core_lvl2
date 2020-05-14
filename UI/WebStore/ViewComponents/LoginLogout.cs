using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.ViewComponents
{
    public class LoginLogout: ViewComponent //тк логики в коде нет, можно было реализовать это представление просто как частичное, а не как вью-компонент
    {
        public IViewComponentResult Invoke() //обязательный метод для ViewComponent (либо AsyncInvoke())
        {
            return View();
        }
    }
}
