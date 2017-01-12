define('Catalog/Gifts/Import/index', [], function () {
    return function (options) {

        // --------------- Данные

        var uploadButton = $('[data-x="gifts_import/upload_button"]'),
            form = $('[data-x="gifts/import/form"]'),
            fileInputs = $('[data-x="gifts/import/file_input"]');

        // --------------- Инициализация

        uploadButton.on('click', function () {
            if (fileInputs.val() != "") {
                var btn = $(this);
                btn.button('loading');
            } else {
                return false;
            }
        });

        // --------------- События


        // --------------- Действия


    };
});