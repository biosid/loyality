define('Main/banner-newyear', ['Main/card_slider'], function (cardSlider) {
    return function (options) {
        var $banner = $(options.selector),
            $kitchen = $banner.find('.banner-newyear__wrap'),
            $gifts = $kitchen.find('.banner-newyear__gift'),
            $stars = $kitchen.find('.banner-newyear__star'),
            $baloons = $kitchen.find('.banner-newyear__baloon'),
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
                $baloon = $star.closest('.banner-newyear__gift').find('.banner-newyear__baloon');

            $baloon.toggle();

            ev.preventDefault();
        }

        function hideBaloon(ev) {
            $(this).hide();
            ev.preventDefault();
        }
    };
});
