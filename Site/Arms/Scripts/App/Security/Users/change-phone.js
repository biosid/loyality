define('Security/Users/change-phone', ['Catalog/Shared/Categories/helpers'], function(h) {
    return function() {

        // --------------- Данные

        var showButton = $('[data-x="security/edit/change_phone"]'),
            modal = $('[data-x="security/users/change_phone_modal"]'),
            form = modal,
            loginField = form.find('[name="Login"]'),
            submitButton = form.find('button[type=submit]'),
            url = form.attr('action');

        // --------------- События

        showButton.on('click', function() {
            showForm();
        });

        form.on('submit', function() {
            if (form.valid()) {
                submitButton.attr('disabled', 'disabled');

                updateOnServer()
                    .done(updateInUi)
                    .fail(function(response) {
                        h.error('Ошибка при смене номера телефона', response);
                    });
            }
            return false;
        });


        // --------------- Действия

        function showForm() {
            var login = loginField.val();

            form.reset();

            loginField.val(login);

            submitButton.removeAttr('disabled');
            modal.modal('show');
        }

        function updateOnServer() {
            return $.post(url, form.serialize());
        }

        function updateInUi(response) {
            if (!response || response.success) {
                modal.modal('hide');

                if (response && response.data && response.data.newUrl) {
                    window.location.replace(response.data.newUrl);
                }

                return;
            }

            submitButton.removeAttr('disabled');
            form.showErrors(response.errors);
        }
    };
});
