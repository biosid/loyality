define('Catalog/Gifts/mass-actions', [], function () {
    return function(options) {

        // --------------- Данные

        var list = $(options.list),
            actionsPanel = $('[data-x="gifts/actions_panel"]'),
            counterNode = $('[data-x="gifts/selected_counter"]'),
            selectAllBtn = $('[data-x="gifts/selectAll"]'),
            selectedGifts = [];


        // --------------- События


        list.on('change', '[name=gift]', function() {
            registerSelection(this.checked, [this.value]);
            //return false;
        });

        selectAllBtn.on('change', function () {
            if (this.checked) {
                var ids = list.find('[name=gift]').map(function () {
                    return $(this).val();
                }).toArray();
                list.find('[name=gift]').prop('checked', true);

                registerSelection(true, ids);
            } else {
                resetSelection();
            }

            //return false;
        });

        // --------------- API

        var o = { 
            selected: function(){
                return selectedGifts;
            },
            reset: resetSelection
        };

        list.data('massActions', o);

        return o;

        // --------------- Действия


        // выбор вознаграждения шаг 1. обновление реестра
        function registerSelection(selected, ids) {
            for (var i = 0; i < ids.length; i++) {
                var at = $.inArray(ids[i], selectedGifts);
                if (selected && at === -1) {
                    selectedGifts.push(ids[i]);
                } else if (!selected && at !== -1) {
                    selectedGifts.splice(at, 1);
                }
            }
            updateActionsPanel();
        }
        
        // выбор вознаграждения шаг 2. показ/скрытие панели действий
        function updateActionsPanel() {
            var count = selectedGifts.length;
            if (count) {
                actionsPanel.show();
                var text = Globalize.pluralize(
                    count,
                    'Выбрано {1:n0} вознаграждение',
                    'Выбрано {2:n0} вознаграждения',
                    'Выбрано {5:n0} вознаграждений'
                );
                counterNode.text(text);
            } else {
                actionsPanel.hide();
                counterNode.text('');
            }
        }

        function resetSelection() {
            selectedGifts = [];
            list.find('[name=gift]').prop('checked', false);
            updateActionsPanel();
        }
    };
});
