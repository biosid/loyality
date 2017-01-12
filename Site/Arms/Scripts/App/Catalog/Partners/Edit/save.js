define('Catalog/Partners/Edit/save', [], function () {
    return function () {
        // --------------- Данные

        var btnSave = $('[data-x="partners/edit/save"]'),
            form = $('[data-x="partners/edit/form"]');

        // --------------- Инициализация

        btnSave.on('click', function () {
            form.submit();
        });
    };
});
