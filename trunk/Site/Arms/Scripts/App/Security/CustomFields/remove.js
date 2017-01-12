define('Security/CustomFields/remove', ['Catalog/Shared/Categories/helpers'], function (h) {
    return function (options) {

        // --------------- Данные

        var list = $('[data-x="security/customfields/list"]'),
            emptyMessage = $('[data-x="security/customfields/empty"]'),
            modal = $('[data-x="security/customfields/confirm_remove_modal"]'),
            confirmButton = $('[data-x="security/customfields/confirm_remove_button"]'),
            customFieldRow = null;

        // --------------- Инициализация

        // --------------- События

        list.on('click', '[data-x="security/customfields/remove"]', function() {
            customFieldRow = $(this).closest('tr');
            
            showConfirmation();

            return false;
        });

        confirmButton.on('click', function () {
            modal.modal('hide');

            removeOnServer()
                .done(removeInUi)
                .fail(function(response) {
                    h.error('Ошибка при удалении дополнительного поля', response);
                });

            return false;
        });

        // --------------- Действия

        function showConfirmation() {
            var name = customFieldRow.find('[data-x="security/customfields/name"]').val();

            modal.find('[data-x="security/customfields/confirm_remove_text"]')
                .text('Действительно удалить поле "' + name + '"?')
                .end()
                .modal('show');
        }
        
        function removeOnServer() {
            var id = customFieldRow.data('customfield-id');
            return $.post(options.url, { id: id });
        }
        
        function removeInUi() {
            customFieldRow.remove();
            if (!list.find('[data-customfield-id]').length) {
                emptyMessage.show();
                list.hide();
            }
        }
    };
});
