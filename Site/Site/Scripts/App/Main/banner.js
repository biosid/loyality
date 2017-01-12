define('Main/banner', function () {
    var MAX_MONEY = 700000,
        DEFAULT_MONEY = 700000;
        
    return function (options) {
        var selectedCard = $(options.selectedCard),
            cardSelector = $(options.cardSelector),
            sliderContainer = $(options.slider);

        $('.snowflake').click(function () {
            $(this).toggleClass('open');
        });
       
        selectedCard.on('click', function () {
            cardSelector.toggle();
            selectedCard.toggleClass('open');
        });

        selectedCard.find('input[readonly]').focus(function () {
            this.blur();
        });

        selectedCard.find('input[readonly]').mousedown(function (e) {
            e.preventDefault();
            $(this).blur();
            return false;
        });

        cardSelector.on('click', '[data-cardtype]', function() {
            cardSelector.hide();
            selectedCard.removeClass('open');

            updateSelectedCard($(this));
        });
        

        // quickfix баннера (чтобы селектор автоматически закрывался при промотке баннеров)
        cardSelector.closest('.promo-block').on('clickoutside', function(){ 
            cardSelector.hide(); 
            selectedCard.removeClass('open'); 
        });

        sliderContainer.slider({
            min: 0,
            max: MAX_MONEY,
            value: DEFAULT_MONEY,
            slide: function(ev, ui) {
                refresh(ui.value);
            },
            change: function(ev, ui) {
                refresh(ui.value);
            },
            create: function () {
                refresh($(options.slider).slider('value'));
            }
        });

        function updateSelectedCard(card) {
            var valImgSrc = card.find('img').attr('src');
            valImgSrc = $.trim(valImgSrc);
            var valCardTypeName = card.find('img').attr('alt');
            valCardTypeName = $.trim(valCardTypeName);
            var valCardType = card.attr('data-cardtype');
            var valMoneyRate = card.attr('data-moneyrate');
            
            selectedCard.find('img').attr('src', valImgSrc);
            selectedCard.find('input[type=text]').val(valCardTypeName);
            selectedCard.attr('data-cardtype', valCardType);
            selectedCard.attr('data-moneyrate', valMoneyRate);

            refresh($(options.slider).slider('value'));
        }

        function refresh(currentSliderValueInRubls) {
            var cardMoneyRate = selectedCard.attr('data-moneyrate');

            var currentSliderValueInPoints = Math.floor(currentSliderValueInRubls / cardMoneyRate);

            updateBubbleValues(currentSliderValueInPoints, currentSliderValueInRubls);
            updateGiftsVisibility(currentSliderValueInPoints);
        }
        
        function updateBubbleValues(points, rubls) {
            var pointsContainer = sliderContainer.find(".points");
            pointsContainer.find('.red .amount').text(points);
            pointsContainer.find('.red small').text(Globalize.pluralize(points, 'бонус', 'бонуса', 'бонусов'));

            pointsContainer.find('.blue .amount').text(rubls);
            pointsContainer.find('.blue small').text(Globalize.pluralize(rubls, 'рубль', 'рубля', 'рублей'));
        }
        
        function updateGiftsVisibility(points) {
            $('.utoy, .btoy, .center-toy').each(function() {
                if ($(this).find('.points').attr('data-amount') > points) {
                    $(this).css('visibility', 'hidden');
                } else {
                    $(this).css('visibility', 'visible');
                }
            });
        }
    };
});