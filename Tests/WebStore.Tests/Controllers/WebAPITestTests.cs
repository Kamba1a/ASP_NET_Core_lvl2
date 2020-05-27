using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Interfaces.Api;

using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class WebAPITestTests
    {
        //здесь для тестирования контроллеру необходима передача IValueService
        //но чтобы не тестировать несколько классво одновременно, вместо IValueService нужен тест-дублер
        //вместо создания и реализации еще одного класса, гораздо легче использовать специальные библиотеки, например Moq

        [TestMethod]
        public void Index_Returns_View_with_Values()
        {
            var expected_result = new[] { "1", "2", "3" };

            var valueService_Mock = new Mock<IValueServices>();    //создаем Mock-объект, указывая нужный нам тип (IValueServices)

            valueService_Mock                                               //необходимо настроить Mock-объект для имитации работы реального класса
               .Setup(service => service.Get()).Returns(expected_result);   //при обращении к методу сервиса Get будет возвращаться наш результат-константа

            var controller = new WebAPITestController(valueService_Mock.Object);    //передаем Mock-объект контроллеру

            var result = controller.Index();    //return View(_ValueServices.Get());

            var view_result = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<string>>(view_result.Model);

            Assert.Equal(expected_result.Length, model.Count());

            //с помощью Mock-объектов можно осуществить дополнительные проверки, например:
            valueService_Mock.Verify(service => service.Get());     //проверка, что в ходе теста был задействован метод Get
            valueService_Mock.VerifyNoOtherCalls();                 //проверка, что в ходе теста не было вызовов других методов
        }
    }
}