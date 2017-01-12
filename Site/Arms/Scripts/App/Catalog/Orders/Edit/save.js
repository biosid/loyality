define('Catalog/Orders/Edit/save', [], function () {
    return function () {
        // --------------- Данные

        var btnSave = $('[data-x="orders/edit/save"]'),
            form = $('[data-x="orders/edit/form"]'),
            saveButton = $('[data-x="orders/edit/save"]'),
            saveMessage = $('[data-x="orders/edit/save_message"]'),
            statusSelect = $('[data-x="orders/edit/status_select"]'),
            statusDescText = $('[data-x="orders/edit/status_desc_text"]'),
            modifiableValues = {
                status: { getCurrentValue: getSelectedStatus },
                statusDesc: { getCurrentValue: getStatusDesc }
            },
            isModified = false;

        // --------------- Инициализация

        // начальные значения
        setInitialValues();

        // скрыть кнопку и сообщение о сохранении изменений
        saveButton.hide();
        saveMessage.hide();

        btnSave.on('click', function () {
            form.submit();
        });

        // --------------- События

        statusSelect.on('change', checkForModifications);
        statusDescText.on('change', checkForModifications);
        statusDescText.on('keyup', checkForModifications);

        // --------------- Действия

        // получение выбранного статуса заказа
        function getSelectedStatus() {
            var selected = statusSelect.find(':selected');
            if (selected)
                return selected.attr('value');
            else
                return null;
        }
        
        // получение введенного описания статуса заказа
        function getStatusDesc() {
            return statusDescText.val();
        }

        // установка начальных значений редактируемых полей
        function setInitialValues() {
            for (var name in modifiableValues) {
                var value = modifiableValues[name];
                value.initial = value.getCurrentValue();
            }
        }

        // проверка на наличие изменений
        function isAnyModified() {
            for (var name in modifiableValues) {
                var value = modifiableValues[name];
                if (value.getCurrentValue() != value.initial)
                    return true;
            }
            return false;
        }

        // изменился ли статус наличия изменений
        function isModifiedChanged() {
            var modified = isAnyModified();
            if (modified != isModified) {
                isModified = modified;
                return true;
            } else {
                return false;
            }
        }

        // проверка на наличие изменений и скрытие/показ кнопки и сообщения
        function checkForModifications() {
            if (isModifiedChanged()) {
                if (isModified) {
                    saveButton.show();
                    saveMessage.show();
                } else {
                    saveButton.hide();
                    saveMessage.hide();
                }
            }
        }
    };
});
