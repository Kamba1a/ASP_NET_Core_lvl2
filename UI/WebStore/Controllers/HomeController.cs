using Microsoft.AspNetCore.Mvc;

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

        // GET: /<controller>/Cart
        public IActionResult Cart()
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
    }
}