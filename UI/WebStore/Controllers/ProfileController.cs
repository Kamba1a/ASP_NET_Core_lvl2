using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        ISqlOrderService _sqlOrderService;

        public ProfileController(ISqlOrderService sqlOrderService)
        {
            _sqlOrderService = sqlOrderService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Orders()
        {
            IEnumerable<UserOrderViewModel> orders = _sqlOrderService.GetUserOrders(User.Identity.Name)
                .Select(order => new UserOrderViewModel()
                {
                    OrderNumber = order.Id,
                    Name = order.User.UserName,
                    Address = order.Address,
                    Phone = order.Phone,
                    TotalSum = order.TotalPrice
                });
            return View(orders);
        }
    }
}