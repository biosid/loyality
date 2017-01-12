define('Catalog/Orders/index', [], function () {
    return function() {
        // --------------- Данные

        var list = $('[data-x="Orders/list"]'),
            fromPicker = $('#from'),
            toPicker = $('#to');

        // --------------- Инициализация

        // выбор начальной даты
        fromPicker.datepicker({
            defaultDate: "-1w",
            onClose: function (selectedDate) {
                toPicker.datepicker("option", "minDate", selectedDate);
            }
        });
        
        // выбор конечной даты
        toPicker.datepicker({
            defaultDate: "+1w",
            onClose: function (selectedDate) {
                fromPicker.datepicker("option", "maxDate", selectedDate);
            }
        });

        // переход на заказ по щелчку на соответствующей сточке в таблице
        list.on('click', '[data-x="Orders/order-row"]', function() {
            var href = $(this).find('a[href]').attr('href');
            window.location = href;
        });

        list.on('click', '[data-x="Orders/order-item-row"]', function() {
            var href = $(this).prevAll('[data-x="Orders/order-row"]').first().find('a[href]').attr('href');
            window.location = href;
        });
    };
});
