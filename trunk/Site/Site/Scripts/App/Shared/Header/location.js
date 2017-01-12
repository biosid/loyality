define('Shared/Header/location', function() {
    return function(options){
        var button = $(options.button),
            popup = $(options.popup),
            form = popup.find('form'),
            regions = popup.find('[data-location=regions]'),
            cities = popup.find('[data-location=cities]'),
            popular = popup.find('[data-location=popular]'),
            search = popup.find('[data-location=search]'),
            searchUrl = options.searchUrl,
            searchSelect = $(options.searchSelect);

        // открытие
        button.on('click', '.click', function (e) { e.stopPropagation(); toggle(); });
        popup.bind('clickoutside', function (e) {
            if (button.hasClass('open') && !isSelect2Activation(e)) {
                toggle();
            }

            // хак для IE: при клике на select2 через некоторый интервал 
            // отыгрывает событие click на body,
            // которое необосновано подхватывается clickoutside
            function isSelect2Activation (e) {
                return e.target == document.body;
            }
        });

        // выбор по ссылке
        popup.on('click', 'a.location', function () {
            var name = $(this).text(),
                code = $(this).attr('rel');
            update(code, name);
        });

        // загрузка городов
        regions.on('click', 'a', function () {
            listCities($(this).attr('rel'));
        });

        // возврат к регионам
        popup.on('click', '.js-link', (function () {
            showRegions();
        }));

        // поиск
        searchSelect.select2({
            placeholder: 'Выберите населенный пункт',
            minimumInputLength: 2,
            ajax: {
                url: searchUrl,
                quietMillis: 300,
                dataType: 'json',
                data: function (term, page) {
                    return { term: term  };
                },
                results: function (data, page) {
                    var results = [];

                    for (var i = 0; i < data.length; i++) {
                        results.push({ id: data[i].value, text: data[i].label });
                    }

                    return { results: results };
                }
            },
            initSelection: function (element, callback) {
                var kladr = element.val(),
                    text = element.data('text');
                if (kladr) {
                    callback({
                        id: kladr,
                        text: text
                    });
                }
            }
        });

        form.on('submit', function() {
            var value = searchSelect.val();
            return !!(value && value.length);
        });

        searchSelect.on('change', function () {
            form.submit();
        });
       
        //------------------------------------------------------------------------------------------

        function toggle() {                        
            button.toggleClass('open');
            popup.slideToggle();
        }

        function update(location, title) {
            form.find('[name=locationKladr]').val(location);
            form.find('[name=locationTitle]').val(title);
            form.submit();
        }

        function listCities(region) {
            cities.load(options.citiesUrl, { regionKladr: region }, function () {
                popular.hide();
                regions.hide();
                cities.show();
            });
        }

        function showRegions() {
            popular.show();
            regions.show();
            cities.hide();
        }
    };
});