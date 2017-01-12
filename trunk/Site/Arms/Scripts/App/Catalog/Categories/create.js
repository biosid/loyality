define('Catalog/Categories/create', ['Catalog/Shared/Categories/helpers', 'Catalog/Shared/Categories/navigation'], function (h, nav) {
    return function (options) {

        // --------------- Данные

        var tree = $(options.tree),
            modal = $('[data-x="categories/create/modal"]'),
            catTree = options.catTree,
            catTreeNavigator = nav.categoryNavigation(catTree, {}),
            btnAdd = $('[data-x="categories/create/add"]'),
            form = modal,
            submitButton = form.find('button[type=submit]'),
            url = form.attr('action');

        // --------------- Инициализация

        form.find('[name="Type"] option:selected').addClass('last-selected');

        // --------------- События

        catTree.on('selectedChanged', function (e, selection) {
            if (selection)
                submitButton.removeAttr('disabled');
            else
                submitButton.attr('disabled', 'disabled');
        });

        form.on('click', function (e) {
            e.stopPropagation();
        });

        // добавление категории шаг 1. кнопка добавления
        btnAdd.on('click', function () {
            showAddForm();
            return false;
        });

        // добавление категории шаг 2. сохранение
        form.on('submit', function () {
            if (form.valid()) {
                submitButton.attr('disabled', 'disabled');
                catTreeNavigator.enable(false);

                createOnServer()
                    .done(createInUi)
                    .fail(function (response) {
                        h.error('Ошибка при попытке создания категории', response);
                    });
            }
            return false;
        });

        // переключение типа категории
        form.on('change', '[name="Type"]', function () {
            var select = $(this);
            if (switchType(select.val())) {
                select.find('.last-selected').removeClass('last-selected');
                select.find('option:selected').addClass('last-selected');
            } else {
                select.val(select.find('.last-selected').val());
            }
        });


        // --------------- Действия

        function switchType(type) {
            $('[data-x="catalog/categories/online_error"]').empty();
            
            if (type == 'Online' && modal.find('[data-x="categories/create/online_partner"] option').length == 0) {
                var errorAlert = $('<div class="alert alert-error">Необходимо добавить хотя бы одного online&#8209;партнера, чтобы создать online&#8209;категорию.</div>');
                $('[data-x="catalog/categories/online_error"]').append(errorAlert);
                setTimeout(function() {
                    errorAlert
                        .fadeTo(500, 0)
                        .slideUp(500, function() { errorAlert.remove(); });
                }, 2000);
                return false;
            }

            var toggle = type == 'Online' ? 'show' : 'hide';
            modal.find('[data-x="categories/create/online_url"]')[toggle]();
            modal.find('[data-x="categories/create/notify_url"]')[toggle]();
            modal.find('[data-x="categories/create/online_partner"]')[toggle]();
            return true;
        }

        function showAddForm() {
            // сброс предыдущих значений
            form.reset();
            switchType('Static');
            catTreeNavigator.resetSelection();
            catTreeNavigator.enable(true);

            // обновить список категорий
            catTreeNavigator.enableCategories(null, false);

            var ids = [-1];
            tree.find('tbody tr')
                .each(function () {
                    var m = h.meta($(this));
                    if (!m.online) {
                        ids.push(m.id);
                    }
                });
            catTreeNavigator.enableCategories(ids, true);

            // раскрыть корневую категорию
            catTreeNavigator.expandItem(null, true);

            // убрать сообщение об ошибке для online-категорий
            $('[data-x="catalog/categories/online_error"]').empty();

            modal.modal('show');
        }

        function createOnServer() {
            var args = form.serialize();
            var selectedId = catTreeNavigator.getSelection();
            if (selectedId != null && selectedId >= 0)
                args += '&ParentId=' + encodeURIComponent(selectedId);
            return $.post(url, args);
        }

        function createInUi(response) {
            if (response.success) {
                modal.modal('hide');

                var tr = $($.trim(response.data)).show(),
                    meta = h.meta(tr);

                if (!meta.parent) {
                    // крневая
                    tr.appendTo(tree.find('tbody'));
                } else {
                    // вложенная
                    var parent = h.node(meta.parent),
                        branch = h.subnodes(parent);

                    tr.insertAfter(parent.add(branch).last());

                    if (parent.hasClass('empty'))
                        parent.removeClass('empty closed');

                    while (parent) {
                        if (parent.hasClass('closed')) {
                            tr.hide();
                            break;
                        }
                        var parentMeta = h.meta(parent);
                        parent = parentMeta.parent != null ? h.node(parentMeta.parent) : null;
                    }
                }

                var title = tr.find('input[name="title"]').attr('value');
                $(document).trigger('categoryCreated', [meta.id, meta.parent, title]);
                return;
            }

            if (catTreeNavigator.getSelection() != null)
                submitButton.removeAttr('disabled');
            else
                submitButton.attr('disabled', 'disabled');
            catTreeNavigator.enable(true);

            form.showErrors(response.errors);
        }
    };
});