define("Shared/dtinterval", [], function() {

    // --------------- API

    var o = {
        setupDtInterval: function(fromSelector, toSelector, options) {

            var fromPicker = $(fromSelector),
                toPicker = $(toSelector);

            // --------------- Инициализация

            options = $.extend({ minDate: null, maxDate: null }, options);

            var fromOptions = $.extend({}, {
                defaultDate: "-1w",
                beforeShow: function() {
                    var toDate = toPicker.datepicker('getDate');
                    return {
                        minDate: options.minDate,
                        maxDate: limitDate(toDate, options.minDate, options.maxDate) || options.maxDate
                    };
                },
                onSelect: function() {
                    fromPicker.closest('form').validate().element(fromSelector);
                }
            }, options.fromOptions);

            var toOptions = $.extend({}, {
                defaultDate: "+1w",
                beforeShow: function () {
                    var fromDate = fromPicker.datepicker('getDate');
                    return {
                        minDate: limitDate(fromDate, options.minDate, options.maxDate) || options.minDate,
                        maxDate: options.maxDate
                    };
                }
            }, options.toOptions);

            fromPicker.datepicker(fromOptions);
            fromPicker.mask("99.99.9999");
            
            toPicker.datepicker(toOptions);
            toPicker.mask("99.99.9999");

            function limitDate(date, minDate, maxDate) {
                return date &&
                    ((minDate && maxDate && new Date(Math.min(Math.max(date, minDate), maxDate))) ||
                     (minDate && new Date(Math.max(date, minDate))) ||
                     (maxDate && new Date(Math.min(date, maxDate))) ||
                     date);
            }
        }
    };

    return o;
});
