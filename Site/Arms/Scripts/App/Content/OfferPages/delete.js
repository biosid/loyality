define('Content/OfferPages/delete', ['Catalog/Shared/Categories/helpers'], function (h) {
    return function (options) {

        // --------------- Данные

        var modal = $('[data-x="content/offerpages/modal_confirm_delete"]'),
            pageRow = null,
            page = null,
            url = options.url,
            selector = options.selector;

        // --------------- События

        $('[data-x="content/offerpages/delete"]').on('click', function () {

            pageRow = $(this).closest('tr');
            page = pageRow.data('pagedata');

            showConfirmation();

            return false;
        });

        $('[data-x="content/offerpages/confirm_delete"]').on('click', function () {
            modal.modal('hide');

            deleteOnServer()
                .done(deleteInUi)
                .fail(function (response) {
                    h.error('Ошибка при удалении оферты', response);
                });
        });

        // --------------- Действия

        function showConfirmation() {
            var text = 'Действительно удалить оферту "' + page.title + '"?';
            modal.find('[data-x="content/offerpages/confirm_delete_text"]').text(text);
            modal.modal('show');
        }

        function deleteOnServer() {
            return $.post(url, { partnerId: page.id });
        }

        function deleteInUi() {
            selector.reset();
            pageRow.remove();
            
            if (!selector.length) {
                window.location.reload();
            }
        }
    };
});
