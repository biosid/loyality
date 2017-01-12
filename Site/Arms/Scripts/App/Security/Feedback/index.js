define('Security/Feedback/index', [], function () {

    return function(container) {

        var form = container.find('form'),
            inputType = form.find('[name="type"]'),
            inputFrom = form.find('[name="Filters.From"]'),
            inputTo = form.find('[name="Filters.To"]');

        enableEllipsis();
        initDatepicker();

        // переход в ветку
        container.on('click', '.feedback__index_type-cell, .feedback__index_message-cell', function (e) {
            var isLink = $(e.target).closest('a').length == 1;
            if (isLink) {
                return;
            }
            var link = $(this).closest('tr').find('a:eq(0)').attr('href');
            window.location.href = link;
        });

        // мгновенный фильтр по типам веток
        inputType.on('change', function () {
            form.submit();
        });
        
        
        // впихнуть невпихуемое
        function enableEllipsis() {
            container.find('.feedback__index_message').dotdotdot();

            container.find('.feedback__index_last-message-by, .feedback__index_author').dotdotdot({
                wrap: 'letter',
                callback: function (truncated, content) {
                    if (truncated) {
                        this.title = $.trim(content.text());
                    }
                }
            });
        }

        // подключить календарь
        function initDatepicker() {
            inputFrom.datepicker({
                defaultDate: "-1w",
                onClose: function (date) {
                    inputTo.datepicker("option", "minDate", date);
                }
            });

            inputTo.datepicker({
                onClose: function (date) {
                    inputFrom.datepicker("option", "maxDate", date);
                }
            });
        }
        
        
    };
    
});