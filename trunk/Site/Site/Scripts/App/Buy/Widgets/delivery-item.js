define('Buy/Widgets/delivery-item', ['Buy/Widgets/common'], function (common) {

    return function (selector, settings) {
        return common.widget(selector, 'delivery-item', create, settings);
    };

    function create(settings) {
        // приватные переменные

        var $element = this,
            $caption,
            $price;

        // инициализация виджета

        init();

        // публичные переменные и методы

        return {
            $element: $element,
            
            set: function(caption, price) {
                $caption.text(caption);
                $price.text(Globalize.format(price, 'n0'))
            }
        };

        // приватные методы

        function init() {
            settings = $.extend({
                captionSelector: '[data-x="/buy/items/delivery-caption"]',
                priceSelector: '[data-x="/buy/items/delivery-price"]'
            }, settings);

            $caption = $(settings.captionSelector);

            $price = $(settings.priceSelector);
        }
    }
});
