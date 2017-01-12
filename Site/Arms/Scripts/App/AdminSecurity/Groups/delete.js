define('AdminSecurity/Groups/delete', ['Shared/modal-error'], function (merr) {
    return function (options) {

        // --------------- Данные

        var deleteButtons = $('[data-x="adminsecurity/groups/delete"]'),
            modal = $('[data-x="adminsecurity/groups/delete/modal"]'),
            confirmButton = $('[data-x="adminsecurity/groups/delete/confirm"]'),
            groupName = null;

        // --------------- Инициализация

        // --------------- События

        deleteButtons.on('click', function(e) {
            e.preventDefault();

            groupName = $(this).closest('tr').data('adminsecurity-group-name');
            showConfirmation();
        });

        confirmButton.on('click', function(e) {
            e.preventDefault();

            modal.modal('hide');

            deleteOnServer()
                .done(deleteInUi)
                .fail(function() {
                    merr.modalError('При удалении группы «' + groupName + '» произошла ошибка', function () {
                        window.location.reload();
                    });
                });
        });

        // --------------- Действия

        function showConfirmation() {
            var text = 'Действительно удалить группу «' + groupName + '»?';
            modal.find('[data-x="adminsecurity/groups/delete/confirm_text"]').text(text);

            modal.modal('show');
        }
        
        function deleteOnServer() {
            return $.post(options.url, { name: groupName });
        }
        
        function deleteInUi(response) {
            if (response && !response.success) {
                merr.modalError('При удалении группы «' + groupName + '» произошла ошибка',function() {
                    window.location.reload();
                });
            } else {
                window.location.reload();
            }
        }
    };
});
