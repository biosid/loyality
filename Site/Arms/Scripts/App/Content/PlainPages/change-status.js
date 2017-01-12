define('Content/PlainPages/change-status', ['Catalog/Shared/Categories/helpers'], function (h) {
    return function(options) {
        // --------------- Данные

        var selector = options.selector,
            url = options.url;

        // --------------- События

        $('[data-x="content/plainpages/changestatus_btn"]').on('click', function () {
            $('[data-x="content/plainpages/changestatus_btn"]').attr('disabled', 'disabled');
            
            var ids = selector.selected(),
                status = $(this).attr('href').replace('#', '');

            changeStatusOnServer(ids, status)
                .done(changeStatusInUi)
                .fail(function(response) {
                    h.error('Ошибка при попытке активации', response);
                });

            return false;
        });

        // --------------- Действия
        
        function changeStatusOnServer(ids, status) {
            return $.post(url, { Ids: ids, Status: status });
        }
        
        function changeStatusInUi() {
            selector.reset();
            window.location.reload();
        }
    };
});
