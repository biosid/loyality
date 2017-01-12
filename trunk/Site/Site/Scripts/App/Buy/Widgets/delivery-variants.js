define('Buy/Widgets/delivery-variants', ['Buy/Widgets/common'], function (common) {

    return function (selector, settings) {
        return common.widget(selector, 'delivery-variants', create, settings);
    };

    function create(settings) {
        // приватные переменные

        var $element = this,
            $preface,
            $controls,
            $summary,
            $values,
            $variant,
	        $type,
            $pickup,
            deliveryVariant;

        // инициализация виджета
        init();

        // публичные переменные и методы

        return {
            $element: $element,

            variant: function () {
                return deliveryVariant;
            },

            hide: function () {
                $preface.show();
                $controls.hide();
                $values.hide();
                setDeliveryVariant(null);
            },

            load: load
        };

        // приватные методы

        function init() {
            settings = $.extend({
                prefaceSelector: '#delivery-variants-preface',
                controlsSelector: '#delivery-variants-controls',
                summarySelector: '#delivery-variants-summary',
                valuesSelector: '#delivery-variants-values',
                variantSelector: '#DeliveryVariantId',
                typeSelector: '#DeliveryType',
                pickupPointSelector: '#PickupPointId',
                descriptionsSelector: '#delivery-variants-descriptions',
                descriptionSelector: '[id^=description-]'
            }, settings);

            $preface = $(settings.prefaceSelector).first();
            $controls = $(settings.controlsSelector).first();
            $summary = $(settings.summarySelector).first();
            $values = $(settings.valuesSelector).first();
            $variant = $(settings.variantSelector).first();
            $type = $(settings.typeSelector).first();
            $pickup = $(settings.pickupPointSelector).first();

            $controls.on('click select2-focus', ':radio, select', selectVariant);

            window.setTimeout(initControls, 0);
        }

        function selectVariant() {
            var $selected = $(this).closest('fieldset'),
                $radioButton = $selected.find(':radio');

            $radioButton.prop('checked', true);

            var data = {
                variantId: $radioButton.val(),
                isPickup: $selected.data('pickup'),
                isEmail: $selected.data('email'),
                deliveryType: $selected.data('deliverytype'),
                name: $selected.data('name')
            };

            if (data.isPickup) {
                var $select = $selected.find('select'),
                    $option = $select.find('option').eq($select[0].selectedIndex);

                $.extend(data, {
                    pickupId: $select.val(),
                    description: $option.data('description'),
                    priceBonus: +$option.data('price'),
                    priceRur: +$option.data('price-rur')
                });
            } else {
                $.extend(data, {
                    pickupId: null,
                    description: $selected.data('description'),
                    priceBonus: +$selected.data('price'),
                    priceRur: +$selected.data('price-rur'),
                });
            }

            setDeliveryVariant(data);
        }

        function isSameVariant(variant1, variant2) {
            if (variant1 == null && variant2 == null) {
                return true;
            }

            if (variant1 == null || variant2 == null) {
                return false;
            }

            if (variant1.variantId != variant2.variantId) {
                return false;
            }

            if (variant1.isPickup != variant2.isPickup) {
                return false;
            }

            if (variant1.isPickup && variant1.pickupId != variant2.pickupId) {
                return false;
            }

            return true;
        }

        function setDeliveryVariant(data) {
            if (isSameVariant(deliveryVariant, data)) {
                return;
            }

            deliveryVariant = data;

            if (data) {
            	$variant.val(data.variantId);
            	$type.val(data.deliveryType);
                $pickup.val(data.pickupId);
                updateDescription(data.description);
            } else {
            	$variant.val('');
            	$type.val('');
                $pickup.val('');
                updateDescription(null);
            }

            $summary.hide();

            $element.trigger('delivery-variants-changed');
        }

        function updateDescription(descritionSelector) {
            if (!descritionSelector) {
                $(settings.descriptionsSelector).hide();
                return;
            }

            var $description = $(descritionSelector);

            if ($description.length) {
                $(settings.descriptionsSelector)
                    .show()
                    .find(settings.descriptionSelector)
                    .hide();
                $description.show();
            } else {
                $(settings.descriptionsSelector).hide();
            }
        }

        function initControls() {
            $controls
                .find('select')
                .select2({
                    allowClear: false,
                    minimumResultsForSearch: -1
                });
            $controls
                .find(':radio:checked')
                .trigger('click');
        }

        function load(zipcodeValue, kladrValue, basketItemIds) {
            setDeliveryVariant(null);
            $preface.hide();

            var loader = window.setTimeout(function () {
                $controls
                    .html('<center><img width="40" height="40" src="/Content/images/loader_40x40.gif" alt="Загрузка..."/></center>')
                    .show();
            }, 300);

            $
                .ajax({
                    type: 'POST',
                    url: settings.deliveryVariantsUrl,
                    traditional: true,
                    data: {
                        postCode: zipcodeValue,
                        kladrCode: kladrValue,
                        basketItems: basketItemIds,
                        showRurPrice: settings.showRurPrice
                    }
                })
                .done(function (data) {
                    window.clearTimeout(loader);
                    $controls.html(data).show();
                    $values.show();
                    initControls();
                });
        }
    }
});
