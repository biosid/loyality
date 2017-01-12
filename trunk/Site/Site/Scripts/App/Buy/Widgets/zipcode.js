define('Buy/Widgets/zipcode', ['Buy/Widgets/common'], function (common) {

    return function (selector, settings) {
        return common.widget(selector, 'zipcode', create, settings);
    };

    function create(settings) {
        // приватные переменные

        var $element = this,
            zip;

        // инициализация виджета
        init();

        // публичные переменные и методы

        return {
            $element: $element,

            zip: function () {
                return zip;
            }
        };

        // приватные методы

        function init() {
            settings = $.extend({
                track: false,
                length: 6
            }, settings);

            if (settings.track) {
                var isInputEventSupported = 'oninput' in document.body;

                $element.on(isInputEventSupported ? 'input' : 'keyup paste', common.debounce(inputChanged));
            }
        }

        function inputChanged() {
            var val = $element.val();

            if (val && val.length == settings.length) {
                if (val == zip) {
                    return;
                }
                zip = val;
            } else {
                if (zip == null) {
                    return;
                }
                zip = null;
            }

            $element.trigger('zipcode-changed');
        }
    }
});
