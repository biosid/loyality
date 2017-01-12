define('Content/PlainPages/mass-actions', [], function () {
    return function (options) {

        // --------------- Данные

        var list = $(options.list),
            actionsPanel = $('[data-x="content/plainpages/actions_panel"]'),
            counterNode = $('[data-x="content/plainpages/counter_node"]'),
            selectAllBtn = $('[data-x="content/plainpages/select_all"]'),
            selectedPages = [];


        // --------------- События


        list.on('change', '[name="page"]', function () {
            registerSelection(this.checked, [this.value]);
        });

        selectAllBtn.on('change', function () {
            if (this.checked) {
                var ids = list.find('[name="page"]').map(function () {
                    return $(this).val();
                }).toArray();
                list.find('[name="page"]').prop('checked', true);

                registerSelection(true, ids);
            } else {
                resetSelection();
            }
        });

        resetSelection();

        // --------------- Действия


        // выбор вознаграждения шаг 1. обновление реестра
        function registerSelection(selected, ids) {
            for (var i = 0; i < ids.length; i++) {
                var at = $.inArray(ids[i], selectedPages);
                if (selected && at === -1) {
                    selectedPages.push(ids[i]);
                } else if (!selected && at !== -1) {
                    selectedPages.splice(at, 1);
                }
            }
            updateActionsPanel();
        }

        // выбор вознаграждения шаг 2. показ/скрытие панели действий
        function updateActionsPanel() {
            var count = selectedPages.length;
            if (count) {
                actionsPanel.show();
                var text = Globalize.pluralize(
                    count,
                    'Отмечена {1:n0} страница',
                    'Отмечено {2:n0} страницы',
                    'Отмечено {5:n0} страниц'
                );
                counterNode.text(text);
            } else {
                actionsPanel.hide();
                counterNode.text('');
            }
        }

        function resetSelection() {
            selectedPages = [];
            list.find('[name="page"]').prop('checked', false);
            selectAllBtn.prop('checked', false);
            updateActionsPanel();
        }

        // --------------- API

        return {
            selected: function () {
                return selectedPages;
            },
            reset: resetSelection
        };
    };
});
