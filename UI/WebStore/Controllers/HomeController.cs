using Microsoft.AspNetCore.Mvc;
using System;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        // GET: /<controller>/Blog
        public IActionResult Blog()
        {
            return View();
        }

        // GET: /<controller>/BlogSingle
        public IActionResult BlogSingle()
        {
            return View();
        }


        // GET: /<controller>/Checkout
        public IActionResult Checkout()
        {
            return View();
        }

        // GET: /<controller>/ContactUs
        public IActionResult ContactUs()
        {
            return View();
        }

        // GET: /<controller>/NotFound404
        public IActionResult NotFound404()
        {
            return View();
        }

        /// <summary>Метод выбрасывает исключение ApplicationException</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Throw(string id) => throw new ApplicationException(id); //для тестового вызова ошибки

        //метод для примера модульного тестирования
        public IActionResult ErrorStatus(string Code)
        {
            switch (Code)
            {
                default:
                    return Content($"Error code:{Code}");
                case "404":
                    return RedirectToAction("NotFound404", "Home");
            }
        }
    }
}