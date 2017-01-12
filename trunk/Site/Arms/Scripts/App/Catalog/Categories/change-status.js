define('Catalog/Categories/change-status', ['Catalog/Shared/Categories/helpers'], function (h) {
    return function(options) {

        // --------------- Данные


        var tree = $(options.tree),
            actionsPanel = $('[data-x="categories/actions_panel"]'),
            counterNode = $('[data-x="categories/selected_counter"]'),
            btnActivate = $('[data-x="categories/activate"]'),
            btnDeactivate = $('[data-x="categories/deactivate"]'),
            selectedCategories = []
            ;


        // --------------- События


        // выбор категорий
        tree.on('change', '[name=category]', function() {
            // актуализируем реестр выбранных
            registerSelection(this.value, this.checked);

            // обновляем UI
            updateActionsPanel();            
            //return false;
        });

        // активация категорий
        btnActivate.on('click', function() {
            toggleOnServer(true);
            toggleInUi(true);            
            return false;
        });

        // деактивация категорий
        btnDeactivate.on('click', function() {
            toggleOnServer(false);
            toggleInUi(false);
            return false;
        });


        // --------------- Действия

        // выбор категории шаг 1. обновление реестра

        function registerSelection(id, selected) {
            id = +id;
            var at = $.inArray(id, selectedCategories);
            if (selected && at === -1) {
                selectedCategories.push(id);
            } else if (at !== -1) {
                selectedCategories.splice(at, 1);
            }
        }

        // выбор категории шаг 2. показ/скрытие панели действий

        function updateActionsPanel() {
            var count = selectedCategories.length;
            if (count) {
                actionsPanel.show();
                var text = Globalize.pluralize(
                    count,
                    'Выбрана {1:n0} категория',
                    'Выбрано {2:n0} категории',
                    'Выбрано {5:n0} категорий'
                );
                counterNode.text(text);
            } else {
                actionsPanel.hide();
                counterNode.text('');
            }
        }

        // смена статуса шаг 1. смена на сервере

        function toggleOnServer(enable) {
            var length = selectedCategories.length;
            return $.post(options.statusUrl, { ids: selectedCategories, enable: enable })
                .fail(function(response) {
                    h.error('Ошибка при попытке смены статуса категорий', response);
                });
        }

        // смена статуса шаг 2. обновление UI

        function toggleInUi(enable) {
            // обновить ноды категорий
            var ids = [];
            for (var id, i = 0; id = selectedCategories[i]; i++) {
                ids.push('#cat-' + id);
            }
            var nodes = $(ids.join(','))[enable ? 'removeClass' : 'addClass']('disabled'); // класс
            nodes.find('[data-x="categories/status"]').text(enable ? 'Активная' : 'Неактивная'); // статус

            // обновить статусы доступности
            h.updateUnavailability(tree);

            // обнулить реестр выбраных категорий
            tree.find('[name=category]').prop('checked', false);
            selectedCategories = [];
            updateActionsPanel();
        }
    };
});