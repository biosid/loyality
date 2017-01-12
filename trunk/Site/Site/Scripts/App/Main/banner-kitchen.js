define('Main/banner-kitchen', ['Main/card_slider'], function(cardSlider) {
    return function(options) {
        var $banner = $(options.selector),
            $kitchen = $banner.find('.banner-kitchen__wrap'),
            $gifts = $kitchen.find('.banner-kitchen__gift'),
            $stars = $kitchen.find('.banner-kitchen__star'),
            $baloons = $kitchen.find('.banner-kitchen__baloon'),
            slider = cardSlider({
                el: $banner.find('.card-slider'),
                max: options.max,
                def: options.def,
                onBonusChange: showGifts
            });
        
        $banner.on('carousel-hide', slider.hide);

        $stars.on('click', toggleBaloon);

        $baloons.on('click', hideBaloon);

        function showGifts(valueBonus) {
            $gifts.each(function () {
                var $gift = $(this);
                if ($gift.data('bonus-value') <= valueBonus) {
                    $gift.show();
                } else {
                    $gift.hide();
                }
            });
        }
        
        function toggleBaloon(ev) {
            var $star = $(this),
                $baloon = $star.closest('.banner-kitchen__gift').find('.banner-kitchen__baloon');

            $baloon.toggle();

            ev.preventDefault();
        }
        
        function hideBaloon(ev) {
            $(this).hide();
            ev.preventDefault();
        }
    };
});
