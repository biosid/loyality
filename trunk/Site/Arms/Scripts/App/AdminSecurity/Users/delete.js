define('AdminSecurity/Users/delete', ['Shared/modal-error'], function (merr) {
    return function (options) {

        // --------------- Данные

        var deleteButtons = $('[data-x="adminsecurity/users/delete"]'),
            modal = $('[data-x="adminsecurity/users/delete/modal"]'),
            confirmButton = $('[data-x="adminsecurity/users/delete/confirm"]'),
            userName = null;

        // --------------- Инициализация

        // --------------- События

        deleteButtons.on('click', function (e) {
            e.preventDefault();

            userName = $(this).closest('tr').data('adminsecurity-user-name');
            showConfirmation();
        });

        confirmButton.on('click', function(e) {
            e.preventDefault();

            modal.modal('hide');

            deleteOnServer()
                .done(deleteInUi)
                .fail(function() {
                    merr.modalError('При удалении пользователя «' + userName + '» произошла ошибка', function() {
                        window.location.reload();
                    });
                });
        });

        // --------------- Действия

        function showConfirmation() {
            var text = 'Действительно удалить пользователя «' + userName + '»?';
            modal.find('[data-x="adminsecurity/users/delete/confirm_text"]').text(text);

            modal.modal('show');
        }

        function deleteOnServer() {
            return $.post(options.url, { login: userName });
        }

        function deleteInUi(response) {
            if (response && !response.success) {
                merr.modalError('При удалении пользователя «' + userName + '» произошла ошибка', function () {
                    window.location.reload();
                });
            } else {
                window.location.reload();
            }
        }
    };
});
