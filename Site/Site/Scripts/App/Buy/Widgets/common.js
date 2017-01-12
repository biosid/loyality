define('Buy/Widgets/common', function () {
    return {

        widget: function (selector, name, factory, settings) {
            var $element = $(selector).first();

            var componentSlotName = 'widget-' + name,
                component = $element.data(componentSlotName);

            if (!component) {
                component = factory.call($element, settings);
                $element.data(componentSlotName, component);
            }

            return component;
        },

        debounce: function(callback) {
            return $.debounce(300, callback);
        }

    };
});
