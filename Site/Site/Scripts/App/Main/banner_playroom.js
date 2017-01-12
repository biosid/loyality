define('Main/banner_playroom', ['Main/card_slider'], function (cardSlider) {

    var opacitySupport = typeof document.body.style.opacity != "undefined";

    return function (options) {
        var $banner = $(options.selector),
            $playroom = $banner.find('.banner-playroom__wrap'),
            $toys = $playroom.find('.banner-playroom__toy'),
            $baloons = $playroom.find('.banner-playroom__baloon'),
            slider = cardSlider({
                el: $banner.find('.card-slider'), 
                max: options.max, 
                def: options.def, 
                onBonusChange: showGifts
            });

        $banner.on('carousel-hide', slider.hide);

        $playroom.on('mousemove', function(e) {
            $baloons.each(function() {
                var $baloon = $(this);
                $baloon.data('baloon-hover-prev', $baloon.data('baloon-hover'));
                $baloon.data('baloon-hover', false);
            });

            $toys.filter(function() {
                var $toy = $(this),
                    offset = $toy.offset(),
                    width = $toy.width(),
                    height = $toy.height();

                return $toy.is(':visible') &&
                    e.pageX >= offset.left &&
                    e.pageY >= offset.top &&
                    e.pageX < offset.left + width &&
                    e.pageY < offset.top + height;
            }).each(function() {
                $('#' + $(this).data('baloon')).data('baloon-hover', true);
            });

            $baloons.each(function() {
                var $baloon = $(this);
                if ($baloon.data('baloon-hover-prev') && !$baloon.data('baloon-hover')) {
                    $baloon.trigger('baloon-hide');
                } else if (!$baloon.data('baloon-hover-prev') && $baloon.data('baloon-hover')) {
                    $baloon.trigger('baloon-show');
                }
            });
        });

        $baloons.on('baloon-show', function() {
            var $baloon = $(this);
            clearTimeout($baloon.data('hidedelay'));
            $baloon.data('showdelay', setTimeout(function() {
                if (opacitySupport) {
                    $baloon.fadeIn(100);
                } else {
                    $baloon.show();
                }
            }, 100));
        });

        $baloons.on('baloon-hide', function() {
            var $baloon = $(this);
            clearTimeout($baloon.data('showdelay'));
            $baloon.data('hidedelay', setTimeout(function () {
                if (opacitySupport) {
                    $baloon.fadeOut(100);
                } else {
                    $baloon.hide();
                }
            }, 100));
        });

        $playroom.on('mouseleave', function() {
            $baloons.each(function() {
                $(this).trigger('baloon-hide');
            });
        });

        function showGifts(valueBonus) {
            $toys.each(function () {
                var $toy = $(this);
                if ($toy.data('bonus-value') <= valueBonus) {
                    $toy.show();
                } else {
                    $toy.hide();
                }
            });
        }
    };
});
