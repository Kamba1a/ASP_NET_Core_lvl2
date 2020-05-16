using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.Areas.Admin.Controllers
{
    [Area ("Admin")] //пометка, что контроллер принадлежит к области
    [Authorize(Roles = "Admins")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}