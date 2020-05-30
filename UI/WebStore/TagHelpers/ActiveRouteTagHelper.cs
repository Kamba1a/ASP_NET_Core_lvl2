using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebStore.TagHelpers
{
    //благодаря наследованию система сама обнаруживает наш класс, как TagHelper
    //класс можно сразу использовать во View как элемент разметки (тег), обращаясь к нему по имени, составленому через дефисы (ActiveRouteTagHelper = <active-route/>)
    //через атрибут [HtmlTargetElement("TagHelperName")] можно изменить имя, которое будет использоваться вместо имени класса как тег в разметке

    /// <summary>Тег-хелпер для подсветки активных ссылок</summary>
    [HtmlTargetElement(Attributes = AttributeName)] //указываем, что класс будет вызываться при наличии атрибута is-active-route внутри тега разметки
    //[HtmlTargetElement(Attributes = "attribute1, attribute2")] //можно задать несколько атрибутов
    public class ActiveRouteTagHelper : TagHelper
    {
        /// <summary>Атрибут для вызова класса из элемента разметки</summary>
        private const string AttributeName = "is-active-route";
        /// <summary>Атрибут для ссылки, подсветка которой необходима только при совпадении имени контроллера</summary>
        private const string IgnoreActionName = "ignore-action";

        #region свойства тег-хелпера для захвата информации из разметки
        //публичные свойства tag-хелпера автоматически наполняются через атрибуты элементов

        /// <summary>имя контроллера из маршрута ссылки</summary>
        [HtmlAttributeName("asp-controller")] //связываем свойство класса с атрибутом элемента разметки
        public string Controller { get; set; }

        /// <summary>имя action-метода контроллера из маршрута ссылки</summary>
        [HtmlAttributeName("asp-action")]
        public string Action { get; set; }

        /// <summary>словарь для дополнительных параметров маршрута</summary>
        [HtmlAttributeName("asp-all-route-data", //атрибут обеспечивает связность с доп.параметрами маршрута и их подгрузку в словарь
        DictionaryAttributePrefix = "asp-route-")] //словарь может использоваться в разметке с префиксом asp-route-(остальное после префикса будет ключом словаря)
        public IDictionary<string, string> RouteValues { get; set; }
            = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        #endregion


        /// <summary>контекст представления, в котором используется тег-хелпер</summary>
        [ViewContext] //необходимо явно указать, что это ViewContext
        [HtmlAttributeNotBound] //указывает, что свойство не связано с элементом разметки
        public ViewContext ViewContext { get; set; }

        /// <summary>Основной метод, обязательный для работы тег-хелпера</summary>
        /// <param name="tagHelperContext">объект, представляющий контекст тега (его содержимое, атрибуты)</param>
        /// <param name="tagHelperOutput">объект для редактирования содержимого тега</param>
        public override void Process(TagHelperContext tagHelperContext, TagHelperOutput tagHelperOutput)
        {
            bool is_ignore_action = tagHelperContext.AllAttributes.ContainsName(IgnoreActionName); //проверка, есть ли у тега атрибут ignore-action

            if (IsActive(is_ignore_action)) MakeActive(tagHelperOutput);

            tagHelperOutput.Attributes.RemoveAll(AttributeName); //скрываем использование аттрибута в разметке (через консоль браузера нельзя будет увидеть)
            tagHelperOutput.Attributes.RemoveAll(IgnoreActionName);
        }

        /// <summary>Метод проверяет, активна ли ссылка</summary>
        /// <param name="isIgnoreAction"></param>
        /// <returns></returns>
        private bool IsActive(bool isIgnoreAction)
        {
            RouteValueDictionary route_values = ViewContext.RouteData.Values;   //получение данных из текущего запроса

            string current_controller = route_values["controller"].ToString();  //получение текущего имени контроллера
            string current_action = route_values["action"].ToString();          //получение текущего имени действия

            const StringComparison ignore_case = StringComparison.OrdinalIgnoreCase;    //метод сравнения без учета регистра
            if (!string.IsNullOrEmpty(Controller) && !string.Equals(current_controller, Controller, ignore_case))
                return false;   //если не совпадает имя текущего контроллера и контроллера в ссылке

            //if (!string.IsNullOrEmpty(Action) && !string.Equals(current_action, Action, ignore_case))
            //    return false;   //если не совпадает имя текущего действия и действия в ссылке

            if (!isIgnoreAction && !string.IsNullOrEmpty(Action) && !string.Equals(current_action, Action, ignore_case))
                return false;

            foreach (var (key, value) in RouteValues)
                if (!route_values.ContainsKey(key) || route_values[key].ToString() != value)
                    return false;   //при несовпадении ключей или значений ключей из словаря доп.параметров маршрута

            return true;
        }

        /// <summary>Метод делает ссылку активной</summary>
        /// <param name="tagHelperOutput"></param>
        private static void MakeActive(TagHelperOutput tagHelperOutput)
        {
            TagHelperAttribute class_attribute = tagHelperOutput.Attributes.FirstOrDefault(attr => attr.Name == "class"); //ищем атрибут класса в элементе разметки

            if (class_attribute is null) tagHelperOutput.Attributes.Add("class", "active"); //если такого атрибута нет, то добавляем его с нужным значением
            else
            {
                if (class_attribute.Value.ToString()?.Contains("active") ?? false) return; //если в атрибуте уже есть значение active, то ничего не делаем

                tagHelperOutput.Attributes.SetAttribute("class",
                    class_attribute.Value is null
                        ? "active"                              //если значения нет, то устанавливаем его
                        : class_attribute.Value + " active");   //иначе добавляем его к остальным значениям
            }
        }
    }
}