using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using WebStore.Domain.Models;

namespace WebStore.TagHelpers
{
    public class PagingTagHelper : TagHelper
    {
        //private readonly IUrlHelperFactory _urlHelperFactory; //сервис для построения ссылок (нужен был до реализации асинхронного пейджинга)

        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        /// <summary>атрибут тег-хелпера page-model, принимающий PageViewModel</summary>
        public PageViewModel PageModel { get; set; }

        /// <summary>атрибут тег-хелпера page-action, принимающий действие контроллера, на которое ссылаются элементы тег-хелпера</summary>
        public string PageAction { get; set; }

        /// <summary>Словарь тег-хелпера для подгрузки и передачи дополнительных параметров</summary>
        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        /// <summary>Конструктор PagingTagHelper</summary>
        /// <param name="urlHelperFactory">сервис для генерации ссылок</param>
        public PagingTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            //_urlHelperFactory = urlHelperFactory;
        }

        /// <summary>Основной метод тег-хелпера</summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);

            TagBuilder ul = new TagBuilder("ul");  //с помощью TagBuilder создаем список <ul> в нашем элементе 
            ul.AddCssClass("pagination");           //присваиваем тегу <ul> класс .pagination

            for (int pageNumber = 1; pageNumber <= PageModel.TotalPages; pageNumber++)
                ul.InnerHtml.AppendHtml(CreateItem(pageNumber/*, urlHelper*/)); //добавляем в <ul> элементы <li> c номерами страниц

            output.Content.AppendHtml(ul);  //выводим созданный список
        }

        /// <summary>Метод создает элемент li списка ul</summary>
        /// <param name="pageNumber"></param>
        /// <param name="urlHelper"></param>
        /// <returns></returns>
        private TagBuilder CreateItem(int pageNumber/*, IUrlHelper urlHelper*/)
        {
            TagBuilder li = new TagBuilder("li");
            TagBuilder a = new TagBuilder("a");

            if (pageNumber == PageModel.PageNumber)     //если номер страницы совпадает с текущей страницей
            {
                li.AddCssClass("active");               //присваиваем элементу класс .active
                a.MergeAttribute("data-page", PageModel.PageNumber.ToString());     //добавить в ссылку атрибут data-page="номер_текущей_страницы" (нужно для скрипта)
            }
            else
            {
                PageUrlValues["page"] = pageNumber;     //записываем в словарь номер страницы

                //a.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);     //генерируем ссылку для этой страницы
                a.Attributes["href"] = "#";     //в случае асинхронной реализации со скриптом ссылка не нужна, поэтому ставим заглушку
                foreach (var (key, value) in PageUrlValues.Where(p => p.Value != null))     //и добавляем в ссылку все параметры из словаря, которые будет использовать скрипт
                    a.MergeAttribute($"data-{key}", value.ToString());
            }

            a.InnerHtml.AppendHtml(pageNumber.ToString());  //внутрь тега <a> вставляем номер страницы
            li.InnerHtml.AppendHtml(a);                     //внутрь элемента <li> добавляем тег <a> со ссылкой

            return li;
        }
    }
}