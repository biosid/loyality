define('Buy/Widgets/address', ['Buy/Widgets/common'], function (common) {

    return function (selector, settings) {
        return common.widget(selector, 'address', create, settings);
    };

    function create(settings) {
        // приватные переменные

        var $element = this,
            $region,
            $location,
            $street,
            $house,
            $flat,
            kladrs = {
                region: null,
                location: null
            };

        // инициализация виджета

        init();

        // публичные переменные и методы

        return {
            $element: $element,
            
            kladrs: function() {
                return kladrs;
            },
            
            clear: function() {
                $street.val('');
                $house.val('');
                $flat.val('');
            },
            
            set: function(address) {
                for (var name in address) {
                    $('#Address_' + name).val(address[name]);
                }

                kladrs.region = address.RegionKladr;
                kladrs.location = address.LocationKladr;

                $region.select2('val', address.RegionKladr);
                $location.select2('data', { id: address.LocationKladr, text: address.Location });
            },
        };

        // приватные методы

        function init() {
            settings = $.extend({
                regionSelector: '#Address_RegionKladr',
                locationSelector: '#Address_LocationKladr',
                streetSelector: '#Address_Street',
                houseSelector: '#Address_House',
                flatSelector: '#Address_Flat'
            }, settings);

            $region = $(settings.regionSelector);
            $location = $(settings.locationSelector);
            $street = $(settings.streetSelector);
            $house = $(settings.houseSelector);
            $flat = $(settings.flatSelector);

            $region.on('change', common.debounce(function () {
                kladrs.region = $region.val();
                
                if (kladrs.region == '7700000000000' || kladrs.region == '7800000000000') {
                    var data = $region.select2('data');
                    $location.select2('data', data);
                    kladrs.location = kladrs.region;

                } else {
                    $location.select2('val', '');
                    kladrs.location = null;
                }

                $element.trigger('address-changed');
            }));

            $location.on('change', common.debounce(function () {
                kladrs.location = $location.val();
                
                if (kladrs.location) {
                    kladrs.region = kladrs.location.substring(0, 2) + '00000000000';
                    $region.select2('val', kladrs.region);
                }

                $element.trigger('address-changed');
            }));

            $region
                .select2({
                    placeholder: 'Выберите регион',
                    allowClear: true,
                })
                .on('change', function() {
                    $(this).valid();
                });

            $location
                .select2({
                    placeholder: 'Выберите населенный пункт',
                    allowClear: true,
                    minimumInputLength: 2,
                    ajax: {
                        url: settings.locationUrl,
                        quietMillis: 300,
                        dataType: 'json',
                        data: function(term) {
                            return { term: term, regionKladr: $region.val() || null };
                        },
                        results: function(data) {
                            var results = [];

                            for (var i = 0; i < data.length; i++) {
                                results.push({ id: data[i].value, text: data[i].label });
                            }

                            return { results: results };
                        }
                    },
                    initSelection: function(element, callback) {
                        var kladr = element.val(),
                            text = element.data('text');
                        if (kladr) {
                            callback({
                                id: kladr,
                                text: text
                            });
                        }
                    }
                })
                .on('change', function() {
                    $(this).valid();
                });
        }
    }
});
