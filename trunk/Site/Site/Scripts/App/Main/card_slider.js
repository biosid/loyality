define('Main/card_slider', function () {

    return function (options) {
        var maxMoney = options.max || 1150000,
        	defaultMoney = options.def || 1150000,
        	$el = $(options.el),
            $controls = $el.find('.card-slider__controls'),
            $slider = $controls.find('.slider'),
            $sliderBubble = $slider.find('.points'),
            $sliderValueBonusText = $sliderBubble.find('.red .amount'),
            $sliderValueBonusUnit = $sliderBubble.find('.red small'),
            $sliderValueRurText = $sliderBubble.find('.blue .amount'),
            $sliderValueRurUnit = $sliderBubble.find('.blue small'),
            $dropdown = $controls.find('.card-slider__dropdown'),
            $dropdownImg = $dropdown.find('img'),
            $dropdownText = $dropdown.find('.card-slider__dropdown-text'),
            $cards = $el.find('.card-slider__cards'),
            rurRate = 25;

        $dropdown.on('click', toggleCards);

        $cards.on('click', '[data-moneyrate]', selectCard);

        // quickfix баннера (чтобы селектор автоматически закрывался при промотке баннеров)
        $cards.closest('.promo-block').on('clickoutside', hideCards);
        $el.on('carousel-hide', hideCards);

        $slider.slider({
            min: 0,
            max: maxMoney,
            value: defaultMoney,
            slide: function (ev, ui) {
                refreshSlider(ui.value);
            },
            change: function (ev, ui) {
                refreshSlider(ui.value);
            },
            create: function () {
                refreshSlider($slider.slider('value'));
            }
        });
        
        function toggleCards() {
            $cards.toggle();
            $controls.toggleClass('active');
        }
        
        function hideCards() {
            $cards.hide();
            $controls.removeClass('active');
        }
        
        function selectCard() {
            hideCards();

            var $card = $(this);

            var cardImgSrc = $.trim($card.find('img').attr('src')),
                cardText = $.trim($card.find('img').attr('alt'));

            $dropdownImg.attr('src', cardImgSrc);
            $dropdownImg.attr('alt', cardText);
            $dropdownText.text(cardText);

            rurRate = $card.data('moneyrate');
            refreshSlider($slider.slider('value'));
        }

        function refreshSlider(valueRur) {
            var valueBonus = Math.floor(valueRur / rurRate);

            updateSliderBubble(valueBonus, valueRur);

            if (options.onBonusChange) {
            	options.onBonusChange(valueBonus);
            }
        }

        function updateSliderBubble(valueBonus, valueRur) {
            $sliderValueBonusText.text(valueBonus);
            $sliderValueBonusUnit.text(Globalize.pluralize(valueBonus, 'бонус', 'бонуса', 'бонусов'));
            $sliderValueRurText.text(valueRur);
            $sliderValueRurUnit.text(Globalize.pluralize(valueRur, 'рубль', 'рубля', 'рублей'));
        }

        return {
        	hide: hideCards
        };
    };
});