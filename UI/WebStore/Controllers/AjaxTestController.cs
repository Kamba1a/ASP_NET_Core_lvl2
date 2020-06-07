using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class AjaxTestController : Controller
    {
        public IActionResult Index() => View();

        //Методы ниже занимаются одним и тем же, но по-разному возвращают результат

        /// <summary>Метод возвращающей некие данные в виде Json объекта</summary>
        /// <param name="id"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetJSON(int? id, string msg)
        {
            await Task.Delay(2000);

            return Json(new
            {
                Message = $"Response (id:{id ?? -1}): {msg ?? "<null>"}",
                ServerTime = DateTime.Now
            });
        }

        /// <summary>Метод возвращающий некие данные, упакованные в html-разметку</summary>
        /// <param name="id"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetTestView(int? id, string msg)
        {
            await Task.Delay(2000);

            return PartialView("_Partial/_DataView", new AjaxTestViewModel
            {
                Id = id ?? -1,
                Message = msg ?? "<null>",
                ServerTime = DateTime.Now
            });
        }
    }
}