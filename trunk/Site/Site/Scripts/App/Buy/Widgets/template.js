// Шаблон виджета
// необходимо заменить везде "template" на название виджета
define('Buy/Widgets/template', ['Buy/Widgets/common'], function (common) {

    return function (selector, settings) {
        return common.widget(selector, 'template', create, settings);
    };

    function create(settings) {
        // приватные переменные

        var $element = this;

        // инициализация виджета

        init();

        // публичные переменные и методы

        return {
            $element: $element,
        };

        // приватные методы

        function init() {
            settings = $.extend({
            }, settings);
        }
    }
});
