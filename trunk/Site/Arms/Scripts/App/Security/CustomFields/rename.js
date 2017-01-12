define('Security/CustomFields/rename', ['Catalog/Shared/Categories/helpers', 'Shared/modal-error'], function (h, merr) {
    return function (options) {

        // --------------- Данные

        var list = $('[data-x="security/customfields/list"]');

        // --------------- События

        list.on('focus', '[data-x="security/customfields/name"]', function () {
            var tr = $(this).closest('tr');
            if (tr.hasClass('edit')) {
                return;
            }

            enterRenameMode(tr, $(this).val());

            $(this).closest('td').off('.rename').on('clickoutside.rename', function () {
                exitRenameMode(tr);
            });
        });

        list.on('keyup', '[data-x="security/customfields/name"]', function (e) {
            var tr = $(this).closest('tr'),
                name = $.trim($(this).val());

            if (e.keyCode == 13) { // ENTER
                if (name) {
                    renameCustomField(tr, name);
                    return false;
                }
            } else if (e.keyCode == 27) { // ESCAPE
                exitRenameMode(tr);
            }
            return true;
        });


        list.on('click', '[data-x="security/customfields/apply_rename"]', function () {
            var tr = $(this).closest('tr'),
                name = $.trim(tr.find('[data-x="security/customfields/name"]').val());

            if (name) {
                renameCustomField(tr, name);
                return false;
            }

            return false;
        });

        list.on('click', '[data-x="security/customfields/cancel_rename"]', function () {
            var tr = $(this).closest('tr');
            exitRenameMode(tr);
        });

        // --------------- Действия

        function enterRenameMode(tr, name) {
            exitRenameMode(list.find('tr.edit'));
            tr.data('customfield-name', name).addClass('edit');
        }

        function exitRenameMode(tr) {
            if (!tr.hasClass('edit')) {
                return;
            }
            tr.removeClass('edit');
            tr.find('[data-x="security/customfields/name"]').val(tr.data('customfield-name')).blur();
        }
        
        function renameCustomField(tr, name) {
            var id = tr.data('customfield-id');

            renameOnServer(id, name)
                .done(function(response) {
                    renameInUi(tr, name, response);
                })
                .fail(function(response) {
                    h.error('Ошибка при попытке переименования дополнительного поля', response);
                });
        }
        
        function renameOnServer(id, name) {
            return $.post(options.url, { id: id, name: name });
        }
        
        function renameInUi(tr, name, response) {
            if (!response || response.success) {
                tr.data('customfield-name', name);
                exitRenameMode(tr);
                return;
            }
            showErrors(response.errors);
        }
        
        function showErrors(errors) {
            var messages = [];
            for (var key in errors) {
                messages.push(errors[key]);
            }

            merr.modalError(messages.join('\n'));
        }
    };
});
