define('Catalog/Orders/Edit/index', [], function () {
    return function() {
        // --------------- Данные

        var navbar = $('[data-x="orders/edit/navbar"]');

        // --------------- Инициализация

        // sticky-заголовок
        navbar.sticky({ topSpacing: 0, center: false });

    };
})
