Cart = {                                        //объект логики корзины
    _properties: {                              //набор свойств, хранящих ссылки на действия контроллера
        getCartViewLink: "",
        addToCartLink: ""
    },

    init: function (properties) {               //аля "конструктор", метод инициализации скрипта
        $.extend(Cart._properties, properties); //конфигурируя скрипт, можно будет передать в него нужные нам значения свойств (в _Layout.cshtml или там, где все скрипты)

        $(".add-to-cart").click(Cart.addToCart); //для всех кнопок с классом .add-to-cart при клике будет происходить событие, которое будет обрабатывать метод addToCart
    },

    addToCart: function (event) {               //обработчик события клика
        event.preventDefault();                 //отключаем работу ссылки (таким образом при клике не будет обновляться страница)

        var button = $(this);                   //получаем из разметки сам объект, к которому применяется скрипт
        const id = button.data("id");           //получаем из объекта идентификатор продукта с помощью словаря (в разметке как data-id="product.id")

        //fetch() - можно использовать вместо $.get
        $.get(Cart._properties.addToCartLink + "/" + id)            //get-запрос к методу контроллера
            .done(function () {                                     //если запрос успешен
                Cart.showToolTip(button);                           //всплывающая подсказка над кнопкой
                Cart.refreshCartView();                             //и обновление ViewComponent (не страницы)
            })
            .fail(function () { console.log("addToCart fail"); });  //если неуспешен - лог
    },

    showToolTip: function (button) {    //метод для всплывающей подсказки
        button.tooltip({ title: "Добавлено в корзину!" }).tooltip("show"); //tooltip - компонент Bootstrap, сначала создаем, потом вызываем метод "show" для показа
        setTimeout(function () {
            button.tooltip("destroy"); //по истечении таймера удаляем компонент tooltip (не просто скрываем, а именно удаляем)
        }, 500);
    },

    refreshCartView: function () {              //метод для обновления ViewComponent
        var container = $("#cart-container");   //получаем элемент <div>, содержащий ViewComponent, по его идентификатору в разметке
        $.get(Cart._properties.getCartViewLink) //get-запрос к методу контроллера, который возвращает представление ViewComponent (html-разметку)
            .done(function (cartHtml) {         //если запрос успешен, передаем html-разметку компонента в div,
                container.html(cartHtml);       //и заменяем его содержимое, таким образом обновляя только элемент корзины, а не целую страницу
            })
            .fail(function () { console.log("refreshCartView fail"); });
    }
} 