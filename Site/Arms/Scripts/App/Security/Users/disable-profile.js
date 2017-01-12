define('Security/Users/disable-profile', ['Catalog/Shared/Categories/helpers'], function (h) {
    return function (options) {
        // --------------- Данные

        var login = options.login,
            disableProfileBtn = $('[data-x="security/edit/program_block"]'),
            disableProfileUrl = options.disableProfileUrl,
            modalConfirmation = $('[data-x="users/modal_confirm_disable_profile"]');

        // --------------- Инициализация

        disableProfileBtn.on('click', function () {
            showConfirmation();
        });

        $(modalConfirmation).on('click', '[data-x="users/confirm_disable_profile"]', function () {
            modalConfirmation.modal('hide');

            $.post(disableProfileUrl, { login: login })
                .done(updateInUi)
                .fail(function (response) {
                    h.error('Ошибка при отключении от программы', response);
                });
            
            return false;
        });

        // --------------- Действия

        function showConfirmation() {
            var text = 'Действительно отключить от программы? Это необратимая операция!';
            modalConfirmation
                .find('[data-x="users/disable_profile/confirm_text"]').text(text)
                .end()
                .modal('show');
        }

        function updateInUi() {
            window.location.reload();
        }

    };
});
