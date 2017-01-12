define('Catalog/Delivery/index', [], function () {
    return function (options) {
        // --------------- Данные

        var partnersSelect = $('[data-x="catalog/delivery/partners_select"]'),
            form = $('[data-x="catalog/delivery/query_form"]'),
            hideBinded = $('[data-x="catalog/delivery/hidebinded"]');

        // --------------- Инициализация

        partnersSelect.on('change', function() {
            form.submit();
        });

        hideBinded.on('change', function() {
            form.submit();
        });

    };
});
