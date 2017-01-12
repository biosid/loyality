define('Content/News/publish', [], function () {
    return function (options) {

        // --------------- Данные

        var selector = options.selector,
            url = options.url;

        // --------------- События

        $('[data-x="news/publish"]').on('click', function (e) {
            e.preventDefault();
            var ids = options.selector.selected();

            activateOnServer(ids, true)
                .done(activateInUi)
                .fail(function (response) {
                    alert('Ошибка при попытке публикации новостей');
                    document.body.innerHTML = response.responseText;
                });
        });
        
        $('[data-x="news/hide"]').on('click', function (e) {
            e.preventDefault();
            var ids = options.selector.selected();

            activateOnServer(ids, false)
                .done(activateInUi)
                .fail(function (response) {
                    alert('Ошибка при попытке скрытия новостей');
                    document.body.innerHTML = response.responseText;
                });
        });
        


        // --------------- Действия

        function activateOnServer(ids, status) {
            return $.post(url, { ids: ids, status: status });
        }

        function activateInUi() {
            selector.reset();
            window.location.reload();
        }
    };
});
