define('Buy/buy', ['Buy/widgets'], function(widgets) {
    return function (options) {
        //----------------------------------------
        // виджеты
        var historyLocations = widgets.historyLocations('#delivery-addresses'),
            zipcode = widgets.zipcode('#Address_PostCode', {
                track: options.isDeliveryVariantsSupported
            }),
            deliveryVariants = widgets.deliveryVariants('#variants', {
                showRurPrice: isAdvancePaymentSupported(),
                deliveryVariantsUrl: options.deliveryVariantsUrl
            }),
            address = widgets.address('#address', {
                locationUrl: options.locationUrl
            }),
            deliveryItem = widgets.deliveryItem('[data-x="/buy/items/delivery"]'),
            advance = widgets.advance('[data-x="/buy/advance"]'),
            proceedButton = widgets.proceedButton('#delivery-form');

        //----------------------------------------
        // данные
        var $form = $('#delivery-form'),
            $postcode = $('#postcode'),
            $totalBonusText = $('#total-bonus-text'),
            $totalRur = $('#total-rur'),
            $totalRurText = $('#total-rur-text'),
            $bonusPayment = $('#BonusPayment'),
            $advancePayment = $('#AdvancePayment'),
            advanceValue = $advancePayment.val(),
            basketItemIds = [];

        //----------------------------------------
        // события

        $form.on('invalid-form', function () {
            window.setTimeout(updateValidationSummary, 0);
        });

        $form.on('submit', function() {
            if ($form.valid()) {
                proceedButton.disable();
            }
        });

        historyLocations.$element.on('location-changed', setHistoryLocation);

        address.$element.on('address-changed', setLocation);

        zipcode.$element.on('zipcode-changed', setZip);

        deliveryVariants.$element.on('delivery-variants-changed', setDeliveryVariant);

        advance.$element.on('advance-changed', updateTotals);

        //----------------------------------------
        // инициализация

        $('[name=BasketItemIds]').each(function () {
            basketItemIds.push(this.value);
        });

        updateValidationSummary();

        setTimeout(restoreAdvance, 0);

        //----------------------------------------
        // действия
        
        // поддерживается ли доплата картой
        function isAdvancePaymentSupported() {
            return options.advancePaymentSupport == 'Delivery' || options.advancePaymentSupport == 'Full';
        }
        
        // восстановить значение аванса (при постбэке)
        function restoreAdvance() {
            advance.set(advanceValue);
        }

        // обновить форму в соответсвие с выбранным историческим адресом
        function setHistoryLocation() {
            var location = historyLocations.location();

            if (!location) {
                return;
            }

            // обновляем поля адреса доставки
            address.set(location);

            // обновляем варианты доставки
            if (options.isDeliveryVariantsSupported) {
                deliveryVariants.load(location.PostCode, null, basketItemIds);
            } else {
                deliveryVariants.load(null, location.LocationKladr, basketItemIds);
            }
        }

        // обновить форму в соответствие с выбранным населенным пунктом
        // (вызывается, только если партнер не поддерживает онлайн варианты доставки)
        function setLocation() {
            var locationKladr = address.kladrs().location;

            // очищаем выбранный исторический адрес
            historyLocations.clear();

            // обновляем варианты доставки
            if (locationKladr) {
                deliveryVariants.load(null, locationKladr, basketItemIds);
            }
        }

        // обновить форму в соответствие с выбранным почтовым индексом
        // (вызывается, только если партнер поддерживает онлайн варианты доставки)
        function setZip() {
            var zip = zipcode.zip();

            // прячем варинаты доставки, если почтовый индекс отсутствует
            if (zip == null) {
                deliveryVariants.hide();
                return;
            }

            // обновляем варианты доставки
            deliveryVariants.load(zip, null, basketItemIds);

            // очищаем адрес
            var location = historyLocations.location();
            if (!location || location.PostCode != zip) {
                historyLocations.clear();
                address.clear();
            }
        }

        // обновить форму в соответствие с выбранным вариантом доставки
        function setDeliveryVariant() {
            var variant = deliveryVariants.variant();

            // скрыть почтовый индекс, если вариант доставки выбран и является доставкой по e-mail, иначе показать
            $postcode[variant && variant.isEmail ? 'hide' : 'show']();

            // показать адрес доставки, если вариант доставки выбран и является курьерской доставкой, иначе скрыть
            address.$element[variant && !(variant.isPickup || variant.isEmail) ? 'show' : 'hide']();
            
            // показать доставку в списке вознаграждений, если вариант доставки выбран, иначе скрыть
            if (variant) {
                deliveryItem.set(variant.name, variant.priceBonus);
            }
            deliveryItem.$element[variant ? 'show' : 'hide']();
            
            if (variant && isAdvancePaymentSupported()) {
                resetAdvance(variant.priceBonus, variant.priceRur);
            } else {
                advance.hide();
            }
        }
        
        function resetAdvance(deliveryPriceBonus, deliveryPriceRur) {
            var totalPriceBonus = options.itemsPriceBonus + deliveryPriceBonus,
                totalPriceRur = options.itemsPriceRur + deliveryPriceRur,
                maxAdvance = Math.floor(totalPriceRur * options.maxAdvanceFraction * 0.01);

            if (deliveryPriceRur == 0 && options.advancePaymentSupport == 'Delivery') {

                advance.hide();

            } else if (deliveryPriceRur >= maxAdvance || options.advancePaymentSupport == 'Delivery') {

                if (options.balance >= totalPriceBonus) {

                    // 1. доплата только за доставку
                    // 2. баланса хватает на все
                    // => можно доплатить за доставку
                    advance.resetToOptionalDelivery(deliveryPriceBonus, deliveryPriceRur);

                } else if (options.balance >= options.itemsPriceBonus) {

                    // 1. доплата только за доставку
                    // 2. баланса не хватает на все
                    // => необходимо доплатить за доставку
                    advance.resetToRequired(deliveryPriceRur);

                } else {

                    advance.hide();

                }
            } else {

                if (options.balance >= totalPriceBonus) {

                    // 1. доплата возможно не только за доставку
                    // 2. баланса хватает на все
                    // => можно доплатить минимум за доставку
                    advance.resetToOptionalRange(deliveryPriceRur, maxAdvance);

                } else if (options.balance >= options.itemsPriceBonus) {

                    // 1. доплата возможно не только за доставку
                    // 2. баланса хватает на все позиции, но не хватает на доставку
                    // => необходимо доплатить минимум за доставку
                    advance.resetToRequiredRange(deliveryPriceRur, maxAdvance);

                } else {

                    // 1. доплата возможно не только за доставку
                    // 2. баланса не хватает даже на позиции
                    // => необходимо доплатить сумму минимум за доставку + часть позиций
                    var minAdvance = totalPriceRur - Math.floor(totalPriceRur * options.balance / totalPriceBonus);
                    
                    if (minAdvance < maxAdvance) {
                        advance.resetToRequiredRange(minAdvance, maxAdvance);
                    } else if (minAdvance == maxAdvance) {
                        advance.resetToRequired(maxAdvance);
                    } else {
                        advance.hide();
                    }
                }
            }
        }

        // проверить, хаватает ли у клиента бонусов, и показать/скрыть сообщение об этом
        function validateBalance(totalBonus) {

            if (totalBonus > options.balance) {
                var errorText = 'К сожалению, у Вас недостаточно бонусов для оплаты данного заказа.' +
                    ' Пожалуйста, выберите другой способ доставки или измение количество вознраграждений в <a href="/basket">корзине</a>.';
                if (options.advancePaymentSupport == 'Full') {
                    errorText += ' До ' + options.maxAdvanceFraction + '% стоимости всего заказа Вы сможете оплатить рублями по Вашей карте ВТБ24.';
                }

                proceedButton.addError('nomoney', errorText);
            } else {
                proceedButton.removeError('nomoney');
            }
        }

        // показать/скрыть валидационные сообщения
        function updateValidationSummary() {
            $form.find('[data-validation-summary-for]').each(function () {
                var target = $($(this).data('validation-summary-for'));
                $(this)[target.find('.input-validation-error').length ? 'show' : 'hide']();
            });
        }

        // обновить итоговые цены
        function updateTotals() {
            var totalBonus = options.itemsPriceBonus,
                totalRur = 0,
                variant = deliveryVariants.variant();

            if (variant) {

                if (isAdvancePaymentSupported()) {

                    totalRur = advance.get();

                    if (totalRur > 0) {
                        var itemsRur = totalRur - variant.priceRur;
                        if (itemsRur <= 0) {
                            totalRur = variant.priceRur;
                        } else if (options.itemsPriceRur > 0) {
                            totalBonus -= Math.round(options.itemsPriceBonus * itemsRur / options.itemsPriceRur);
                        }
                    } else {
                        totalBonus += variant.priceBonus;
                    }

                } else {

                    totalBonus += variant.priceBonus;

                }
            }

            // обновляем тексты "Итого"
            $totalBonusText.text(Globalize.format(totalBonus, 'n0'));

            if (totalRur > 0) {
                $totalRurText.text(Globalize.format(totalRur, 'n0'));
                $totalRur.show();
            } else {
                $totalRur.hide();
            }

            // обновляем поля с итоговыми платежами
            $bonusPayment.val(totalBonus);
            $advancePayment.val(totalRur);
            
            // проверяем наличие бонусов на балансе клиента
            validateBalance(totalBonus);

            // активируем/деактивируем кнопку "продолжить"
            if (variant) {
                proceedButton.update();
            } else {
                proceedButton.disable();
            }
        }
    };
});
