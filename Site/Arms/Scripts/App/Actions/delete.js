define('Actions/delete', ['Catalog/Shared/Categories/helpers'], function (h) {
    return function (options) {

        // --------------- Данные

        var modal = $('[data-x="actions/actual/delete/modal"]'),
            actionRow = null,
            action = null,
            deleteUrl = options.deleteUrl;

        // --------------- События

        $('[data-x="actions/actual/delete"]').on('click', function () {

            actionRow = $(this).closest('tr');
            action = actionRow.data('actiondata');

            showConfirmation(action);

            return false;
        });

        $('[data-x="actions/actual/confirm_delete"]').on('click', function () {
            modal.modal('hide');

            deleteOnServer()
                .done(deleteInUi)
                .fail(function(response) {
                    h.error('Ошибка при удалении механики', response);
                });
        });

        // --------------- Действия

        function showConfirmation() {
            var text = 'Действительно удалить механику "' + action.name + '"?';
            modal.find('[data-x="actions/actual/delete/confirm_text"]').text(text);

            modal.modal('show');
        }

        function deleteOnServer() {
            return $.post(deleteUrl, { id: action.id });
        }
        
        function deleteInUi() {
            actionRow.remove();
        }
    };
});
