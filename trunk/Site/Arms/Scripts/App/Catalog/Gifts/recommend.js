define('Catalog/Gifts/recommend', ['Catalog/Shared/Categories/helpers'], function (h) {
    return function (options) {

        // --------------- Данные

        var selector = options.selector,
            url = options.url;

        // --------------- События

        $('[data-x="gifts/recommend"]').on('click', function () {
            $('[data-x="gifts/recommend"]').attr('disabled', 'disabled');

            var ids = options.selector.selected(),
                isRecommended = $(this).attr('href').replace('#', '');

            recommendOnServer(ids, isRecommended)
                .done(moderateInUi)
                .fail(function (response) {
                    h.error('Ошибка при попытке смены признака "рекомендовано"', response);
                });

            return false;
        });

        // --------------- Действия

        function recommendOnServer(ids, isRecommended) {
            return $.post(url, { Ids: ids, IsRecommended: isRecommended });
        }

        function moderateInUi() {
            selector.reset();
            window.location.reload();
        }
    };
});
