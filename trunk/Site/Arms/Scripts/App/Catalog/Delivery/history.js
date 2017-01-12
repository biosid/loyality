define('Catalog/Delivery/History/index', [], function () {
    return function (options) {
        // --------------- Данные

        var partnersSelect = $('[data-x="catalog/delivery/history/partners_select"]'),
            form = $('[data-x="catalog/delivery/history/query_form"]');

        // --------------- Инициализация

        partnersSelect.on('change', function () {
            form.submit();
        });
    };
});
