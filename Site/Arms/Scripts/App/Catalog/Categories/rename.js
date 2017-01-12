define('Catalog/Categories/rename', ['Catalog/Shared/Categories/helpers'], function (h) {
    return function(options) {

    	// --------------- Данные


        var tree = $(options.tree);


        // --------------- События

        tree.on('focus', '[name="title"]', function(){
            var tr = $(this).closest('tr');
            if (tr.hasClass('edit')) {
                return;
            }
            enterRenameMode(tr, this.value);

            // автозакрытие
            $(this).closest('td').off('.rename').on('clickoutside.rename', function(){
                exitRenameMode(tr);
            });
        });

        tree.on('keyup', '[name=title]', function(e) {
            var tr = $(this).closest('tr'),
                category = h.meta(tr);

            if(e.keyCode == 13) { // ENTER
                if (!$.trim(this.value)) {
                    return false;
                }
                
                renameOnServer(category.id, this.value)
                    .done(function (response) {
                        renameInUi(tr, this.value, response);
                    })
                    .fail(function(response) {
                        h.error('Ошибка при попытке переименования категории', response);
                    });
                
            } else if (e.keyCode == 27) { // ESCAPE
                exitRenameMode(tr);
            }
            return true;
        });

        // фильтрация нежелательного ввода (ибо нет возможности использовать валидацию)
        tree.on('input', '[name=title]', function(){
            if (this.value.indexOf('/') !== -1) { // FORWARD SLASH (/)
                this.value = this.value.replace(/\//g, '');
                $(this).stop(true, true).effect('pulsate');
                return false;
            }
            return true;
        });

        tree.on('click', '[data-x="categories/apply_rename"]', function(){
            var tr = $(this).closest('tr'),
                category = h.meta(tr),
                title = tr.find('[name=title]').val();

            if (!$.trim(title)) {
                return false;
            }

            renameOnServer(category.id, title)
                .done(function(response) {
                    renameInUi(tr, title, response);
                })
                .fail(function(response) {
                    h.error('Ошибка при попытке переименования категории', response);
                });

            return false;
        });

        tree.on('click', '[data-x="categories/cancel_rename"]', function(){
            var tr = $(this).closest('tr');
            exitRenameMode(tr);
        });


		// --------------- Действия


        // переход в режим переименования
        function enterRenameMode(tr, title) {
            exitRenameMode(tree.find('tr.edit'));
            tr.data('title', title).addClass('edit');
        }

        // выход из режима переименования
        function exitRenameMode(tr) {
            if (!tr.hasClass('edit')) {
                return;
            }
            tr.removeClass('edit');
            tr.find('[name=title]').val(tr.data('title')).blur();
        }

        // переименование шаг 1. сервер
        function renameOnServer(id, title) {
            return $.post(options.renameUrl, { id: id, title: title });
        }

        // переименование шаг 2. UI
        function renameInUi(tr, title, response) {
            if (!response || response.success) {
                tr.data('title', title);
                exitRenameMode(tr);

                $(document).trigger('categoryRenamed', [h.meta(tr).id, title]);
                return;
            }
            showErrors(response.errors);
        }

        function showErrors(errors) {
            var messages = [];
            for (var key in errors) {
                messages.push(errors[key]);
            }
            alert(messages.join('\n'));

            // TODO: показать ошибку более интеллектуально
        }
    };
});