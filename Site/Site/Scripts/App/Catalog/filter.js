define('Catalog/filter', function() {
    return function (options) {

        // --------------- Данные

        var container = $(options.el),
            form = $('[data-x="catalog/filter/form"]'),
            trigger = container.find('.more-filter-title'),
            extended = container.find('[data-x="catalog/filter/extended"]'),
            slider = container.find('.rs-slider-range'),
            minText = container.find('.min-price'),
            minVal = container.find('input[name="min_price"]'),
            minTextFocused = false,
            maxText = container.find('.max-price'),
            maxVal = container.find('input[name="max_price"]'),
            maxTextFocused = false,
            sliderFrom = Math.min(minVal.val() || 0, options.maxPriceScale),
            sliderTo = Math.min(maxVal.val() || options.maxPriceScale, options.maxPriceScale);

        // --------------- Инициализация

        // слайдер
        slider.slider({
            range: true,
            min: 0,
            max: options.maxPriceScale,
            values: [sliderFrom, sliderTo],
            slide: function (event, ui) {
                updateSlider(ui.values[0], ui.values[1]);
            }
        });

        // кастомный дропдаун
        if ($('.more-filter select').length) {
            $('.more-filter select').select2({ minimumResultsForSearch: -1, allowClear: true });
        }

        updateSlider(sliderFrom, sliderTo);
        
        // --------------- События

        // открытие
        trigger.on('click', function () {
            if (container.hasClass('open')) {
                close();
            } else {
                open();
            }
        });

        form.on('submit', function () {
            minText.blur();
            maxText.blur();

            if (form.attr('method') != 'POST')
                return;

            var getparams = form.find('[data-x="catalog/filter/input_get"]');

            form.attr('action', window.location.pathname + '?' + getparams.serialize());
            getparams.prop('disabled', true);
            form[0].submit();
        });

        minText.on('focus', function () {
            minText.val(minVal.val() || 0);
            minTextFocused = true;
        });

        maxText.on('focus', function() {
            maxText.val(maxVal.val() || options.maxPriceScale);
            maxTextFocused = true;
        });

        minText.on('blur', function () {
            if (!minTextFocused) {
                return;
            }

            var val = $.trim(minText.val()) != '' ? parseInt(minText.val(), 10) : 0;

            if (isNaN(val)) {
                val = minVal.val() || 0;
            } else {
                val = Math.min(Math.max(val, 0), maxVal.val() || options.maxPriceScale);
            }

            setMinVal(val);
            minTextFocused = false;
        });

        maxText.on('blur', function() {
            if (!maxTextFocused) {
                return;
            }

            var val = $.trim(maxText.val()) != '' ? parseInt(maxText.val(), 10) : options.maxPriceScale;

            if (isNaN(val)) {
                val = maxVal.val() || options.maxPriceScale;
            } else {
                val = Math.max(Math.min(val, options.maxPriceScale), minVal.val() || 0);
            }

            setMaxVal(val);
            maxTextFocused = false;
        });
        
        // --------------- Действия

        function open() {
            extended.show();
            container.addClass('open');
        }
        
        function close() {
            container.removeClass('open');
            extended.hide();
        }
        
        function setMinVal(val) {
            minText.val(Globalize.format(val, 'n0'));
            slider.slider('values', 0, val);

            if (val != 0) {
                minVal.val(val);
            } else {
                minVal.val('');
            }
        }
        
        function setMaxVal(val) {
            maxText.val(Globalize.format(val, 'n0'));
            slider.slider('values', 1, val);

            if (val != options.maxPriceScale) {
                maxVal.val(val);
            } else {
                maxVal.val('');
            }
        }
        
        function updateSlider(from, to) {
            setMinVal(from);
            setMaxVal(to);
        }
        
    };
});
