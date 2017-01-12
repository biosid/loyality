define('Buy/Widgets/proceed-button', ['Buy/Widgets/common'], function (common) {

    return function (selector, settings) {
        return common.widget(selector, 'proceed-button', create, settings);
    };

    function create(settings) {
        // приватные переменные

        var $element = this,
            $button,
            $error,
            $errorText,
            errors = {};

        // инициализация виджета
        init();

        // публичные переменные и методы

        return {
            $element: $element,
            
            disable: function() {
                $button.attr("disabled", "disabled");
            },
            
            update: function () {
                if ($.isEmptyObject(errors)) {
                    $button.removeAttr("disabled");
                }
            },

            addError: function (owner, text) {
                errors[owner] = text;
                apply();
            },

            removeError: function (owner) {
                if (errors[owner]) {
                    delete errors[owner];
                    apply();
                }
            }
        };

        // приватные методы

        function init() {
            settings = $.extend({
                buttonSelector: ':submit',
                errorSelector: '#proceed-error',
                errorTextSelector: '#proceed-error-text'
            }, settings);

            $button = $element.find(settings.buttonSelector);
            $error = $element.find(settings.errorSelector);
            $errorText = $element.find(settings.errorTextSelector);
        }

        function apply() {
            $button.removeAttr("disabled");
            $errorText.empty();
            $error.hide();
            
            for (var owner in errors) {
                $button.attr("disabled", "disabled");
                $errorText.append('<div>' + errors[owner] + '</div>');
                $error.show();
            }
        }
    }
});
