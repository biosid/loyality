define('Catalog/Gifts/activation', ['Catalog/Shared/Categories/helpers'], function (h) {
    return function (options) {

        // --------------- Данные

        var selector = options.selector,
            url = options.url;

        // --------------- События

        $('[data-x="catalog/gifts/activation"]').on('click', function(e) {
            e.preventDefault();
            var ids = options.selector.selected(),
                status = $(this).attr('href').replace('#', '');

            activateOnServer(ids, status)
                .done(activateInUi)
                .fail(function(response) {
                    h.error('Ошибка при попытке смены активности вознаграждения', response);
                });
        });

        // --------------- Действия

        function activateOnServer(ids, status) {
            return $.post(url, { Ids: ids, Status: status });
        }

        function activateInUi() {
            selector.reset();
            window.location.reload();
        }
    };
});
