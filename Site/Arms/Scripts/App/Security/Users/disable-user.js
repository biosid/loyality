define('Security/Users/disable-user', ['Catalog/Shared/Categories/helpers'], function (h) {
    return function (options) {
        // --------------- Данные

        var login = options.login,
            disableUserUrl = options.disableUserUrl,
            disableUserBtn = $('[data-x="security/edit/disable"]'),
            enableBtn = $('[data-x="security/edit/enable"]');

        // --------------- Инициализация

        enableBtn.on('click', function () {
            $.post(disableUserUrl, { login: login, disable: false })
                .done(updateInUi)
                .fail(function (response) {
                    h.error('Ошибка при активации пользователя', response);
                });
        });

        disableUserBtn.on('click', function() {
            $.post(disableUserUrl, { login: login, disable: true })
                .done(updateInUi)
                .fail(function (response) {
                    h.error('Ошибка при деактивации пользователя', response);
                });
        });

        // --------------- Действия

        function updateInUi() {
            window.location.reload();
        }

    };
});
