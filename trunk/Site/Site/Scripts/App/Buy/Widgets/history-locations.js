define('Buy/Widgets/history-locations', ['Buy/Widgets/common'], function(common) {

    return function(selector) {
        return common.widget(selector, 'history-locations', create);
    };

    function create() {
        // приватные переменные

        var $element = this,
            locationIndex,
            location;

        // инициализация виджета
        init();

        // публичные переменные и методы

        return {
            $element: $element,

            location: function () {
                return location;
            },

            clear: function () {
                $element.select2('val', '');
                locationIndex = null;
                location = null;
            }
        };

        // приватные методы

        function init() {
            $element.on('change', function () {
                var newLocationIndex = $(this).val();

                if (newLocationIndex != locationIndex) {
                    locationIndex = newLocationIndex;
                    location = $(this).find('option').eq(locationIndex).data('json');
                    $element.trigger('location-changed');
                }
            });

            $element.select2({
                placeholderOption: 'first',
                allowClear: true,
                minimumResultsForSearch: -1
            });
        }
    }
});
