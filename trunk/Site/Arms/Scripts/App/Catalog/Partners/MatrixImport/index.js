define('Catalog/Partners/MatrixImport/index', [], function () {
    return function() {
        // --------------- Данные

        var uploadButton = $('[data-x="partners/import/upload"]');

        // --------------- Инициализация

        uploadButton.on('click', function() {
            var btn = $(this);
            btn.button('loading');
        });
    };
});
