define('Main/obj-banner', ['Main/card_slider'], function (cardSlider) {

    var opacitySupport = typeof document.body.style.opacity != "undefined";

    return function (options) {
        var $banner = $(options.selector),
            $items = $banner.find('[data-value]'),
            $baloons = $banner.find('.obj-banner__baloon'),
            slider = cardSlider({
                el: $banner.find('.card-slider'), 
                max: options.max, 
                def: options.def, 
                onBonusChange: filter
            });

        $banner.on('carousel-hide', slider.hide);
        $banner.on('mouseenter', '[data-value]', $.debounce(100, function(){
            var $baloon = $(this).find('.obj-banner__baloon');
            baloon($baloon);
        }));
        $banner.on('mouseleave', '[data-value]', $.debounce(100, function(){
            var $baloon = $(this).find('.obj-banner__baloon');
            baloon();
        }));

        function baloon ($baloon) {
            if (opacitySupport) {
                $baloons.fadeOut(100);
            } else {
                $baloons.hide();
            }

            if (!$baloon) {
                return;
            }

            if (opacitySupport) {
                $baloon.fadeIn(100);
            } else {
                $baloon.show();
            }
        }

        function filter (val) {
            $items.each(function () {
                var $toy = $(this);
                if ($toy.data('value') <= val) {
                    $toy.show();
                } else {
                    $toy.hide();
                }
            });
        }
    };
});
