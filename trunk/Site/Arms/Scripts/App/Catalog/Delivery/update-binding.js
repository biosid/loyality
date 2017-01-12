define('Catalog/Delivery/update-binding', ['Catalog/Shared/Categories/helpers'], function (h) {
    return function (options) {

        // --------------- Данные


        var modal = $('[data-x="catalog/delivery/modal"]'),
            form = modal,
            submitButton = form.find('button[type=submit]'),
            resetButton = form.find('button[data-x="catalog/delivery/modal/reset"]'),
            resetUrl = options.resetUrl,
            locationByKladrUrl = options.locationByKladrUrl,
            locationSearchUrl = options.locationSearchUrl,
            selectedKladr = $("#KladrCode"),
            actionUrl = form.attr('action'),
            editButtons = $('[data-x="catalog/delivery/edit"]'),
            addButtons = $('[data-x="catalog/delivery/add"]'),
            regionSelect = $('[name="Region"]'),
            districtSelect = $('[name="District"]'),
            citySelect = $('[name="City"]');

        regionSelect.select2(getSelect2Options('Выберите область', 1, 'Region', null));

        regionSelect.on('change', function () {
            districtSelect.select2('data', null);
            citySelect.select2('data', null);
        });

        districtSelect.select2(getSelect2Options('Выберите район', 2, 'District', function() { return regionSelect.val() || null; }));

        districtSelect.on('change', function () {
            var data = $(this).select2('data');
            if (data) {
                regionSelect.select2('data', { id: data.regionValue, text: data.regionLabel });
            }

            citySelect.select2('data', null);
        });

        citySelect.select2(getSelect2Options('Выберите населенный пункт', 2, 'City', function() { return districtSelect.val() || regionSelect.val() || null; }));

        citySelect.on('change', function() {
            var data = $(this).select2('data');

            if (data) {
                regionSelect.select2('data', { id: data.regionValue, text: data.regionLabel });
                districtSelect.select2('data', { id: data.districtValue, text: data.districtLabel });
            }
        });

       // --------------- События

        editButtons.on('click', function() {
            var tr = $(this).closest('tr');

            form.find('h3').text('Изменение привязки');
            resetButton.show();

            showForm(tr);
            return false;
        });

        addButtons.on('click', function() {
            var tr = $(this).closest('tr');

            form.find('h3').text('Добавление привязки');
            resetButton.hide();

            showForm(tr);
            return false;
        });

        resetButton.on('click', function () {
            $.post(resetUrl, form.serialize())
                    .done(updateInUi)
                    .fail(function (response) {
                        h.error('Ошибка при сбросе привязки', response);
                    });

            return false;
        });

        form.on('submit', function () {
            if (form.valid()) {
                disableButtons();

                updateOnServer()
                    .done(updateInUi)
                    .fail(function (response) {
                        h.error('Ошибка при изменении привязки', response);
                    });
            }
            return false;
        });
        
        // --------------- Действия

        function getSelect2Options(placeholder, minimumInput, type, parent) {
            return {
                placeholder: placeholder,
                allowClear: true,
                minimumInputLength: minimumInput,
                ajax: {
                    url: locationSearchUrl,
                    quietMillis: 300,
                    dataType: 'json',
                    data: function(term, page) {
                        return {
                            term: term,
                            type: type,
                            parentKladr: parent != null ? parent() : null
                        };
                    },
                    results: function(data, page) {
                        var results = [];

                        for (var i = 0; i < data.length; i++) {
                            results.push(
                                {
                                    id: data[i].selectionValue,
                                    text: data[i].selectionLabel,
                                    fullText: data[i].selectionFullLabel,
                                    regionLabel: data[i].regionLabel,
                                    regionValue: data[i].regionValue,
                                    districtLabel: data[i].districtLabel,
                                    districtValue: data[i].districtValue
                                });
                        }

                        return { results: results };
                    }
                },
                formatResult: function(object, container, query) {
                    return object.fullText;
                },
                formatSelection: function(object, container) {
                    return object.text;
                }
            };
        }

        function disableButtons() {
            submitButton.attr('disabled', 'disabled');
            resetButton.attr('disabled', 'disabled');
        }
        
        function enableButtons() {
            submitButton.removeAttr('disabled');
            resetButton.removeAttr('disabled');
        }

        function showForm(tr) {
            var data = tr.data('binding');
            
            form.reset();
            regionSelect.select2('data', null);
            districtSelect.select2('data', null);
            citySelect.select2('data', null);

            form.find('h4').text('Населенный пункт «' + data.title + '»');
            form.find('[name="Id"]').val(data.id);
            form.find('[name="KladrCode"]').val(data.kladrCode);
            if (data.kladrCode != null) {
                $.get(locationByKladrUrl, { kladrCode: data.kladrCode }, function (returnData) {

                    switch (returnData.type) {
                        case 'City':
                        case 'Town':
                            citySelect.select2('data', { id: returnData.selectionValue, text: returnData.selectionLabel });
                            regionSelect.select2('data', { id: returnData.regionValue, text: returnData.regionLabel });
                            districtSelect.select2('data', { id: returnData.districtValue, text: returnData.districtLabel });
                            break;
                        
                        case 'District':
                            districtSelect.select2('data', { id: returnData.selectionValue, text: returnData.selectionLabel });
                            regionSelect.select2('data', { id: returnData.regionValue, text: returnData.regionLabel });
                            break;
                            
                        case 'Region':
                            regionSelect.select2('data', { id: returnData.selectionValue, text: returnData.selectionLabel });
                            break;
                    }
                });
            }

            enableButtons();
            modal.modal('show');
        }

        function updateOnServer() {
            var value = citySelect.val() || districtSelect.val() || regionSelect.val();
            selectedKladr.val(value);

            return $.post(actionUrl, form.serialize());
        }

        function updateInUi(response) {
            if (!response || response.success) {
                modal.modal('hide');

                window.location = window.location;
                
                return;
            }

            enableButtons();
            
            form.showErrors(response.errors);
        }
    };
});