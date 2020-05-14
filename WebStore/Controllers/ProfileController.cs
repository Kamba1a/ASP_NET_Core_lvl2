using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Interfaces;
using WebStore.Models;

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