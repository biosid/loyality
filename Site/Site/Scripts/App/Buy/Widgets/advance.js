define('Buy/Widgets/advance', ['Buy/Widgets/common'], function (common) {

    return function (selector, settings) {
        return common.widget(selector, 'advance', create, settings);
    };

    function create(settings) {
        // приватные переменные

        var $element = this,
            $optCapt,
            $optDlvrCapt,
            $reqCapt,
            $desc,
            $rangeDesc,
            $dlvrDesc,
            $reqDesc,
            $sliderWrapper,
            $slider,
            $sliderMin,
            $sliderMax,
            $valueText,
            textFocused = false,
            isRequired = false,
            isRange = true,
            isSelected = false,
            minValue = 0,
            maxValue = 100,
            value = 0;

        // инициализация виджета

        init();

        // публичные переменные и методы

        return {
            $element: $element,

            resetToOptionalRange: function(minVal, maxVal) {
                isRequired = false;
                isRange = true;
                minValue = minVal;
                maxValue = maxVal;

                showControls();

                resetRange();

                $element.trigger('advance-changed');
            },

            resetToOptionalDelivery: function(deliveryPriceBonus, deliveryPriceRur) {
                isRequired = false;
                isRange = false;
                value = deliveryPriceRur;

                var dlvrPriceText = Globalize.pluralize(deliveryPriceRur, '{1:n0} рубль', '{2:n0} рубля', '{5:n0} рублей') +
                    Globalize.pluralize(deliveryPriceBonus, ' ({1:n0} бонус)', ' ({2:n0} бонуса)', ' ({5:n0} бонусов)');

                $dlvrDesc.find('span').text(dlvrPriceText);

                showControls();

                $element.trigger('advance-changed');
            },

            resetToRequiredRange: function(minVal, maxVal) {
                isRequired = true;
                isRange = true;
                minValue = minVal;
                maxValue = maxVal;

                showControls();

                resetRange();

                $element.trigger('advance-changed');
            },

            resetToRequired: function(val) {
                isRequired = true;
                isRange = false;
                value = val;

                showControls();

                $element.trigger('advance-changed');
            },
            
            hide: function () {
                $element.hide();

                $element.trigger('advance-changed');
            },

            get: function () {
                return $element.is(':visible') && (isRequired || isSelected) ? value : 0;
            },

            set: function (val) {
                if ($element.is(':visible')) {

                    val = +val;

                    if (val == 0) {
                        if (isRequired || !isSelected) {
                            return;
                        } else {
                            isSelected = false;
                        }
                    } else {
                        updateValue(val, true);
                        if (value > 0 && !isRequired) {
                            isSelected = true;
                        }
                    }

                    showControls();
                    $element.trigger('advance-changed');
                }
            }
        };

        // приватные методы

        function init() {
            settings = $.extend({
                optCaptSelector: '[data-x="/buy/advance/optional"]',
                optDlvrCaptSelector: '[data-x="/buy/advance/optional-delivery"]',
                reqCaptSelector: '[data-x="/buy/advance/required"]',
                descSelector: '.advance__description',
                rangeDescSelector: '[data-x="/buy/advance/range-desc"]',
                dlvrDescSelector: '[data-x="/buy/advance/delivery-desc"]',
                reqDescSelector: '[data-x="/buy/advance/required-desc"]',
                sliderWrapperSelector: '.advance__slider-wrapper',
                sliderSelector: '.advance__slider',
                sliderMinSelector: '.advance__slider-min',
                sliderMaxSelector: '.advance__slider-max',
                valueTextSelector: '.advance__slider-value'
            }, settings);

            $optCapt = $element.find(settings.optCaptSelector);
            $optDlvrCapt = $element.find(settings.optDlvrCaptSelector);
            $reqCapt = $element.find(settings.reqCaptSelector);
            $desc = $element.find(settings.descSelector);
            $rangeDesc = $element.find(settings.rangeDescSelector);
            $dlvrDesc = $element.find(settings.dlvrDescSelector);
            $reqDesc = $element.find(settings.reqDescSelector);
            $sliderWrapper = $element.find(settings.sliderWrapperSelector);
            $slider = $element.find(settings.sliderSelector);
            $sliderMin = $element.find(settings.sliderMinSelector);
            $sliderMax = $element.find(settings.sliderMaxSelector);
            $valueText = $element.find(settings.valueTextSelector);

            $slider.slider({
                range: 'min',
                min: minValue,
                max: maxValue,
                value: minValue,
                slide: function (ev, ui) {
                    updateValue(ui.value);
                }
            });

            $valueText.on('focus', textFocus);
            $valueText.on('blur', textBlur);

            $optCapt.on('click', toggle);
            $optDlvrCapt.on('click', toggle);
        }
        
        function resetRange() {
            $sliderMin.text(minValue + ' руб.');
            $sliderMax.text(maxValue + ' руб.');

            $slider.slider('option', {
                min: minValue,
                max: maxValue,
                value: minValue,
            });

            updateValue(minValue, true);
        }

        function showControls() {
            $element.show();

            $optCapt[!isRequired && isRange ? 'show' : 'hide']();
            $optDlvrCapt[!isRequired && !isRange ? 'show' : 'hide']();
            $reqCapt[isRequired ? 'show' : 'hide']();

            $desc[isRequired || isSelected ? 'show' : 'hide']();
            $rangeDesc[!isRequired && isRange ? 'show' : 'hide']();
            $dlvrDesc[!isRequired && !isRange ? 'show' : 'hide']();
            $reqDesc[isRequired ? 'show' : 'hide']();

            $sliderWrapper[isRange && (isRequired || isSelected) ? 'show' : 'hide']();
        }

        function textFocus() {
            $valueText.val(value);
            textFocused = true;
        }

        function textBlur() {
            if (!textFocused) {
                return;
            }

            var newValue = $.trim($valueText.val()) != '' ? parseInt($valueText.val(), 10) : NaN;

            updateValue(newValue);

            textFocused = false;
        }
        
        function toggle() {
            if (isRequired) {
                return;
            }

            isSelected = !isSelected;

            showControls();

            $element.trigger('advance-changed');
        }

        function updateValue(newValue, noTrigger) {
            if (!isNaN(newValue)) {
                newValue = Math.min(Math.max(newValue, minValue), maxValue);
            }
            
            if (isNaN(newValue) || newValue == value) {
                updateValueInUi();
            } else {
                value = newValue;
                updateValueInUi();
                if (!noTrigger) {
                    $element.trigger('advance-changed');
                }
            }
        }
        
        function updateValueInUi() {
            $valueText.val(Globalize.format(value, 'n0'));
            $slider.slider('value', value);
        }
    }
});
