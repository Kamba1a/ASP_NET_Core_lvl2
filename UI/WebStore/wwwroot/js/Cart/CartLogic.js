Cart = {                                        //объект логики корзины
    _properties: {                              //набор свойств, хранящих ссылки на действия контроллера
        getCartViewLink: "",
        addToCartLink: "",
        decrementLink: "",
        removeFromCartLink: ""
    },

    init: function (properties) {               //аля "конструктор", метод инициализации скрипта
        $.extend(Cart._properties, properties); //конфигурируя скрипт, можно будет передать в него нужные нам значения свойств (в _Layout.cshtml или там, где все скрипты)

        Cart.initEvents();                      //инициализация всех событий
    },

    initEvents: function () {                       //все события
        $(".add-to-cart").click(Cart.addToCart);    //для всех кнопок с классом .add-to-cart при клике будет происходить событие, которое будет обрабатывать метод addToCart
        $(".cart_quantity_up").click(Cart.incrementItem);
        $(".cart_quantity_down").click(Cart.decrementItem);
        $(".cart_quantity_delete").click(Cart.removeFromCart);

        //необходимо для корректной работы кнопок "Добавить в корзину" на всех страницах каталога:
        $("body").on('DOMSubtreeModified', "#catalog-items-container", function () { //событие DOMSubtreeModified срабатывает всякий раз когда структура или текст внутри элемента изменяется
            $(".add-to-cart").click(Cart.addToCart);    //при переходе на другие страницы каталога, перепривязывает обработчик к кнопкам новых товаров
        });
    },

    addToCart: function (event) {               //обработчик события клика добавления товара в корзину
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

    refreshCartView: function () {              //метод для обновления CartViewComponent
        var container = $("#cart-container");   //получаем элемент <div>, содержащий ViewComponent, по его идентификатору в разметке
        $.get(Cart._properties.getCartViewLink) //get-запрос к методу контроллера, который возвращает представление ViewComponent (html-разметку)
            .done(function (cartHtml) {         //если запрос успешен, передаем html-разметку компонента в div,
                container.html(cartHtml);       //и заменяем его содержимое, таким образом обновляя только элемент корзины, а не целую страницу
            })
            .fail(function () { console.log("refreshCartView fail"); });
    },

    incrementItem: function (event) {   //обработчик события кнопки увеличения количества товаров в корзине
        event.preventDefault();

        var button = $(this);
        const id = button.data("id");           // data-id="..."

        var container = button.closest("tr");   //получаем родительский элемент <tr> (вся строка товара)

        $.get(Cart._properties.addToCartLink + "/" + id)
            .done(function () {
                const count = parseInt($(".cart_quantity_input", container).val());     //получаем значение внутри элемента с указанным классом (только в пределах контейнера)
                $(".cart_quantity_input", container).val(count + 1);                    //меняем значение внутри того же элемента на (count + 1)

                Cart.refreshPrice(container);
                Cart.refreshCartView();
            })
            .fail(function () { console.log("incrementItem fail"); });
    },

    refreshPrice: function (container) {    //метод для обновления цены товара (с учетом количества)
        const quantity = parseInt($(".cart_quantity_input", container).val());
        const price = parseFloat($(".cart_price", container).data("price"));    //получаем цену из словаря (data-price="...")

        const totalPrice = quantity * price;
        var value = totalPrice.toLocaleString("ru-RU", { style: "currency", currency: "RUB" }); //приводим цену к нужному формату

        $(".cart_total_price", container).data("price", totalPrice);    //перезаписываем в словарь новую цену
        $(".cart_total_price", container).html(value);                  //заменяем значение внутри элемента разметки на новую цену 

        Cart.refreshTotalPrice();                                       //обновляем итоговую цену корзины
    },

    refreshTotalPrice: function () {    //метод для обновления итоговой цены в корзине
        var total = 0;

        $(".cart_total_price").each(function () {               //для каждого элемента с указанным классом 
            const price = parseFloat($(this).data("price"));    //получаем цену
            total += price;                                     //и прибавляем ее к общей цене
        });

        var value = total.toLocaleString("ru-RU", { style: "currency", currency: "RUB" });
        $("#total-order-price").html(value);
    },

    decrementItem: function (event) {   //обработчик события кнопки уменьшения количества товаров в корзине
        event.preventDefault();

        var button = $(this);
        const id = button.data("id"); // data-id="..."

        var container = button.closest("tr");

        $.get(Cart._properties.decrementLink + "/" + id)
            .done(function () {
                const count = parseInt($(".cart_quantity_input", container).val());
                if (count > 1) {        //если товара больше единицы, то просто убавляем количество и обновляем цены
                    $(".cart_quantity_input", container).val(count - 1);
                    Cart.refreshPrice(container);
                } else {                //иначе удаляем элемент и обновляем общую цену
                    container.remove();
                    Cart.refreshTotalPrice();
                }
                Cart.refreshCartView();     //в любом случае обновляем CartViewComponent
            })
            .fail(function () { console.log("decrementItem fail"); });
    },

    removeFromCart: function (event) {  //обработчик события кнопки удаления товара из корзины
        event.preventDefault();

        var button = $(this);
        const id = button.data("id"); // data-id="..."

        $.get(Cart._properties.removeFromCartLink + "/" + id)
            .done(function () {
                button.closest("tr").remove();  //находим родительский элемент <tr> (вся строка товара) и сразу удаляем его
                Cart.refreshTotalPrice();
                Cart.refreshCartView();
            })
            .fail(function () { console.log("removeFromCart fail"); });
    }
} 