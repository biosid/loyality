define('Content/News/Mass-actions', [], function () {
    return function (options) {

        // --------------- Данные

        var list = $(options.list),
            actionsPanel = $('[data-x="news/actions_panel"]'),
            selectAllBtn = $('[data-x="news/selectAll"]'),
            counterText = $('[data-x="news/selected_counter"]'),
            selectedNews = [];


        // --------------- События


        list.on('change', '[name=news]', function () {
            registerSelection(this.checked, [this.value]);
        });

        selectAllBtn.on('change', function () {
            if (this.checked) {
                var ids = list.find('[name=news]').map(function () {
                    return $(this).val();
                }).toArray();
                list.find('[name=news]').prop('checked', true);

                registerSelection(true, ids);
            } else {
                resetSelection();
            }
        });

        // --------------- API

        return {
            selected: function () {
                return selectedNews;
            },
            reset: resetSelection
        };


        // --------------- Действия


        // выбор новости шаг 1. обновление реестра
        function registerSelection(selected, ids) {
            for (var i = 0; i < ids.length; i++) {
                var at = $.inArray(ids[i], selectedNews);
                if (selected && at === -1) {
                    selectedNews.push(ids[i]);
                } else if (!selected && at !== -1) {
                    selectedNews.splice(at, 1);
                }
            }
            updateActionsPanel();
        }

        // выбор новости шаг 2. показ/скрытие панели действий
        function updateActionsPanel() {
            var count = selectedNews.length;
            if (count) {
                actionsPanel.show();
                var text = Globalize.pluralize(
                    count,
                    'Выбрана {1:n0} новость',
                    'Выбрано {2:n0} новости',
                    'Выбрано {5:n0} новостей'
                );
                counterText.text(text);
            } else {
                actionsPanel.hide();
                counterText.text('');
            }
        }

        function resetSelection() {
            selectedNews = [];
            list.find('[name=news]').prop('checked', false);
            selectAllBtn.prop('checked', false);
            updateActionsPanel();
        }
    };
});
