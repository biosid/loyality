define('Security/Users/reset', ['Catalog/Shared/Categories/helpers'], function (h) {
    return function (options) {
        // --------------- Данные

        var login = options.login,
            resetPassBtn = $('[data-x="security/edit/reset_password"]'),
            resetPassUrl = options.resetPasswordUrl,
            modalConfirmation = $('[data-x="users/modal_confirm_reset"]');

        // --------------- Инициализация

        resetPassBtn.on('click', function () {
            showConfirmation();
        });
        
        $(modalConfirmation).on('click', '[data-x="users/confirm_reset"]', function () {
            modalConfirmation.modal('hide');

            $.post(resetPassUrl, { login: login })
                .done(updateInUi)
                .fail(function (response) {
                    h.error('Ошибка при сбросе пароля', response);
                });

            return false;
        });

        // --------------- Действия

       function showConfirmation() {
           var text = 'Действительно сбросить пароль?';
           modalConfirmation
               .find('[data-x="users/reset/confirm_text"]').text(text)
               .end()
               .modal('show');
       }

        function updateInUi() {
            window.location.reload();
        }

    };
});
