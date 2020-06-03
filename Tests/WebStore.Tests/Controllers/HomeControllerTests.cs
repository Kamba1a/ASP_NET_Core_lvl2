using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WebStore.Controllers;

using Assert = Xunit.Assert; // указываем конкретно, что используем Xunit.Assert, а не TestTools.UnitTesting.Assert

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void Index_Returns_View()
        {
            var controller = new HomeController();

            var result = controller.Index();

            Assert.IsType<ViewResult>(result);  //проверка на возврат представления
        }

        //[TestMethod]
        //public void Blog_Returns_View()
        //{
        //    var controller = new HomeController();

        //    var result = controller.Blog();

        //    Assert.IsType<ViewResult>(result);
        //}

        //[TestMethod]
        //public void BlogSingle_Returns_View()
        //{
        //    var controller = new HomeController();

        //    var result = controller.BlogSingle();

        //    Assert.IsType<ViewResult>(result);
        //}


        [TestMethod]
        public void Error404_Returns_View()
        {
            var controller = new HomeController();

            var result = controller.NotFound404();

            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void ContactUs_Returns_View()
        {
            var controller = new HomeController();

            var result = controller.ContactUs();

            Assert.IsType<ViewResult>(result);
        }

        //тест на конкретное исключение
        [TestMethod, ExpectedException(typeof(ApplicationException))] //атрибут указывает, что при получении ApplicationException тест будет считаться успешным
        public void Throw_throw_ApplicationException()
        {
            var controller = new HomeController();

            controller.Throw(string.Empty);     //метод выбрасывает исключение
        }

        [TestMethod]
        public void Throw_throw_ApplicationException_with_Message()
        {
            var controller = new HomeController();
            const string expected_message_text = "Message!";

            var exception = Assert.Throws<ApplicationException>(() => controller.Throw(expected_message_text)); //проверка на выброс конкретного исключения

            Assert.Equal(expected_message_text, exception.Message); //проверка на корректность сообщения об ошибке в исключении
        }

        [TestMethod]
        public void ErrorStatus_404_RedirectTo_NotFound404()
        {
            var controller = new HomeController();
            const string status404 = "404";

            var result = controller.ErrorStatus(status404);

            var redirect_to_action = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(redirect_to_action.ControllerName);
            Assert.Equal(nameof(HomeController.NotFound404), redirect_to_action.ActionName);
        }
    }
}