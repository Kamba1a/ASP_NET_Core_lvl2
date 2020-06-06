using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpleMvcSitemap;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    //Контроллер для работы с пакетом SimpleMvcSitemap для генерации XML карты сайта (для поисковых роботов)
    public class SitemapController : Controller
    {
        public IActionResult Index([FromServices] ICatalogData catalogData)
        {
            List<SitemapNode> nodes = new List<SitemapNode>   //составляем список статических узлов для передачи в карту сайта
            {
                new SitemapNode(Url.Action("Index", "Home")),
                new SitemapNode(Url.Action("ContactUs", "Home")),
                new SitemapNode(Url.Action("Index", "Blogs")),
                new SitemapNode(Url.Action("BlogSingle", "Blogs")),
                new SitemapNode(Url.Action("Shop", "Catalog")),
                new SitemapNode(Url.Action("Index", "WebAPITest")),
            };

            //добавляем к списку все страницы секций товаров
            nodes.AddRange(catalogData.GetSections().Select(section => new SitemapNode(Url.Action("Shop", "Catalog", new { sectionId = section.Id }))));

            //добавляем к списку все страницы брендов товаров
            foreach (var brand in catalogData.GetBrands())
                nodes.Add(new SitemapNode(Url.Action("Shop", "Catalog", new { brandId = brand.Id })));

            //добавляем к списку каждую страницу каждого товара
            foreach (var product in catalogData.GetProducts().Products)
                nodes.Add(new SitemapNode(Url.Action("ProductDetails", "Catalog", new { product.Id })));

            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes)); //передаем список провайдеру для создания карты сайта 
        }
    }
}