define('Security/CustomFields/append', ['Catalog/Shared/Categories/helpers'], function (h) {
    return function() {

        // --------------- Данные

        var appendButton = $('[data-x="security/customfields/append"]'),
            list = $('[data-x="security/customfields/list"]'),
            emptyMessage = $('[data-x="security/customfields/empty"]'),
            modal = $('[data-x="security/customfields/append_modal"]'),
            form = modal,
            submitButton = form.find('button[type=submit]'),
            url = form.attr('action'),
            fieldTemplate = $('[data-x="security/customfields/template"]');

        // --------------- Инициализация

        // --------------- События

        appendButton.on('click', function() {
            showAppendForm();
            return false;
        });

        form.on('submit', function() {
            if (form.valid()) {
                submitButton.attr('disabled', 'disabled');

                appendOnServer()
                    .done(appendInUi)
                    .fail(function(response) {
                        h.error('Ошибка при попытке создания дополнительного поля', response);
                    });
            }
            return false;
        });

        // --------------- Действия

        function showAppendForm() {
            form.reset();
            submitButton.removeAttr('disabled');
            modal.modal('show');
        }
        
        function appendOnServer() {
            return $.post(url, form.serialize());
        }

        function appendInUi(response) {
            if (!response || response.success) {
                modal.modal('hide');
                
                if (response && response.data && response.data.id && response.data.name) {
                    var field = fieldTemplate.clone();
                    field.attr('data-customfield-id', response.data.id);
                    field.find('[data-x="security/customfields/name"]').val(response.data.name);
                    field.insertBefore(fieldTemplate).show();
                    
                    emptyMessage.hide();
                    list.show();
                } else {
                    window.location.reload();
                }
            }

            submitButton.removeAttr('disabled');
            form.showErrors(response.errors);
        }
    };
});
