using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using WebStore.Controllers;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;

namespace WebStore.ViewComponents
{
    public class BreadCrumbsViewComponent : ViewComponent
    {
        private readonly ICatalogData _catalogData;

        public BreadCrumbsViewComponent(ICatalogData catalogData)
        {
            _catalogData = catalogData;
        }

        public IViewComponentResult Invoke()
        {
            var (type, id, from_type) = GetParametres();

            switch (type)
            {
                default: return View(Enumerable.Empty<BreadCrumbViewModel>());

                case BreadCrumbType.Section:
                    return View(new[]
                    {
                        new BreadCrumbViewModel
                        {
                            BreadCrumbType = BreadCrumbType.Section,
                            Id = id,
                            Name = _catalogData.GetSectionById(id).Name
                        }
                    });

                case BreadCrumbType.Brand:
                    return View(new[]
                    {
                        new BreadCrumbViewModel
                        {
                            BreadCrumbType = BreadCrumbType.Brand,
                            Id = id,
                            Name = _catalogData.GetBrandById(id).Name
                        }
                    });

                case BreadCrumbType.Product:
                    var product = _catalogData.GetProductById(id);
                    return View(new[]
                    {
                        new BreadCrumbViewModel
                        {
                            BreadCrumbType = from_type,
                            Id = from_type == BreadCrumbType.Section
                                ? product.Section.Id
                                : product.Brand.Id,
                            Name = from_type == BreadCrumbType.Section
                                ? product.Section.Name
                                : product.Brand.Name
                        },
                        new BreadCrumbViewModel
                        {
                            BreadCrumbType = BreadCrumbType.Product,
                            Id = product.Id,
                            Name = product.Name
                        }
                    });
            }
        }

        private (BreadCrumbType Type, int Id, BreadCrumbType FromType) GetParametres()
        {
            BreadCrumbType type = Request.Query.ContainsKey("SectionId")
                ? BreadCrumbType.Section
                : Request.Query.ContainsKey("BrandId")
                    ? BreadCrumbType.Brand
                    : BreadCrumbType.None;

            if ((string)ViewContext.RouteData.Values["action"] == nameof(CatalogController.ProductDetails))
                type = BreadCrumbType.Product;

            var id = 0;

            var from_type = BreadCrumbType.Section;

            switch (type)
            {
                default: throw new ArgumentOutOfRangeException();
                case BreadCrumbType.None: break;

                case BreadCrumbType.Section:
                    id = int.Parse(Request.Query["sectionId"].ToString());
                    break;

                case BreadCrumbType.Brand:
                    id = int.Parse(Request.Query["brandId"].ToString());
                    break;

                case BreadCrumbType.Product:

                    //реализация с вебинара:
                    //id = int.Parse(ViewContext.RouteData.Values["productId"].ToString() ?? string.Empty);     //так ключ не находит, выдает ошибку
                    //if (Request.Query.ContainsKey("fromBrand"))   //не понятен смысл условия, т.к. при переходе по ссылке на товар (откуда угодно),
                    //from_type = BreadCrumbType.Brand;             //from_type всегда будет становится типом Brand

                    //своя реализация:
                    id = int.Parse(Request.Query["productId"].ToString());  //через ViewContext.RouteData.Value (как в вебинаре) не находило
                    if (Request.Query["fromBrand"] == "brand")  //fromBrand размещен в ссылке на товар со страницы магазина как asp-route-fromBrand
                        from_type = BreadCrumbType.Brand;

                    break;
            }

            return (type, id, from_type); //кортеж
        }
    }
}