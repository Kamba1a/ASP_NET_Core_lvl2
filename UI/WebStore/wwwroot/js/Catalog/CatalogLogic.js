//Скрипт для асинхронного пейджинга товаров в каталоге
Catalog = {
    _properties: {
        getUrl: ""      // Catalog/GetFilteredItems
    },

    init: properties => {
        $.extend(Catalog._properties, properties);
        $(".pagination li a").click(Catalog.clickOnPage); //событие клика для каждой кнопки в пагинаторе
    },

    clickOnPage: function (event) {
        event.preventDefault();

        const button = $(this);

        if (button.prop("href").length > 0) {   //условие для всех кнопок, кроме текущей, т.к. в текущей атрибут href=null

            var page = button.data("page");     //номер страницы (из параметра data-page="...")

            const container = $("#catalog-items-container");    //родительский контейнер (блок <div>)

            container.LoadingOverlay("show");   //библиотека jquery-loading-overlay для отображения значка загрузки элемента

            const data = button.data(); //получаем все параметры из словаря data

            let query = "";     //переменная для всех параметров запроса
            for (let key in data) {
                if (data.hasOwnProperty(key))
                    query += key + "=" + data[key] + "&"; //формируем строку из всех ненулевых параметров в словаре
            }

            $.get(Catalog._properties.getUrl + "?" + query) //делаем запроса, присоединив все параметры к адресу запроса
                .done(html => {
                    container.html(html);       //заменяем разметку внутри контейнера на ту, что получили из контроллера (Partial/_ProductItems.cshtml)
                    container.LoadingOverlay("hide");   //скрываем значок загрузки

                    $(".pagination li").removeClass("active");  //удаляем класс .active у всех элементов пагинатора
                    $(".pagination li a").prop("href", "#");    //добавляем всем ссылкам пагинатора атрибут href="#"

                    $(`.pagination li a[data-page=${page}]`)    //ссылке, у которой номер страницы равен текущему номеру:
                        .removeAttr("href")                     //удаляем атрибут href
                        .parent().addClass("active");           //и добавляем класс .active родителю <a>, т.е. элементу <li> пагинатора
                })
                .fail(() => {
                    container.LoadingOverlay("hide");
                    console.log("clickOnPage fail");
                });
        }
    }
} 