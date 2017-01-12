define('Catalog/Gifts/moderation', ['Catalog/Shared/Categories/helpers'], function (h) {
    return function(options) {

        // --------------- Данные

        var selector = options.selector,
            url = options.url;

        // --------------- События

        $('[data-x="gifts/moderation"]').on('click', function () {
            $('[data-x="gifts/moderation"]').attr('disabled', 'disabled');
            
            var ids = options.selector.selected(),
                status = $(this).attr('href').replace('#', '');

            moderateOnServer(ids, status)
                .done(moderateInUi)
                .fail(function(response) {
                    h.error('Ошибка при попытке смены статуса модерации', response);
                });

            return false;
        });

        // --------------- Действия

        function moderateOnServer(ids, status) {
            return $.post(url, {Ids:ids, Status: status});
        }

        function moderateInUi() {
            selector.reset();
            window.location.reload();
        }
    };
});
