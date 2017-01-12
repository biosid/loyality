define('AdminSecurity/Users/pwdreset', ['Catalog/Shared/Categories/helpers'], function(h) {
    return function() {

        // --------------- Данные

        var pwdResetButton = $('[data-x="adminsecurity/users/pwdreset/show_button"]'),
            modal = $('[data-x="adminsecurity/users/pwdreset/modal"]'),
            form = modal,
            submitButton = form.find('button[type=submit]'),
            url = form.attr('action');

        // --------------- События

        pwdResetButton.on('click', function(e) {
            e.preventDefault();
            showModal();
        });

        form.on('submit', function(e) {
            e.preventDefault();
            submitForm();
        });

        // --------------- Действия

        function showModal() {
            var userName = form.find('[name="Name"]').val();

            form.reset();
            form.find('[name="Name"]').val(userName);

            submitButton.prop('disabled', false);
            modal.modal('show');
        }
        
        function submitForm() {
            if (!form.valid())
                return;

            submitButton.prop('disabled', true);

            updateOnServer()
                .done(updateInUi)
                .fail(function (response) {
                    h.error('Ошибка при сбросе пароля пользователя', response);
                });
        }
        
        function updateOnServer() {
            return $.post(url, form.serialize());
        }
        
        function updateInUi(response) {
            if (!response || response.success) {
                modal.modal('hide');
            } else {
                submitButton.prop('disabled', false);
                form.showErrors(response.errors);
            }
        }
    };
});
