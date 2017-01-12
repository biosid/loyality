define('Catalog/Gifts/Edit/index', [], function () {
    return function () {
        // --------------- Данные

        var navbar = $('[data-x="gifts/edit/navbar"]'),
            form = $('[data-x="gifts/edit/form"]'),
            addUrlImageButtons = $('[data-x="gifts/edit/add_image"]'),
            addFileImageButtons = $('[data-x="gifts/edit/file_input"]'),
            removeImageButtons = $('[data-x="gifts/edit/remove_image"]'),
            addParameterButtons = $('[data-x="gifts/edit/add_parameter"]'),
            removeParameterButtons = $('[data-x="gifts/edit/remove_parameter"]'),
	        isDeliveredByEmailCheckbox = $('[data-x="gifts/edit/is_delivered_by_email"]'),
	        weightInput = $('[data-x="gifts/edit/weight_input"]');

        // --------------- Инициализация

        navbar.sticky({ topSpacing: 0, center: false });
        
        $('.goodimage').on('error', function () {            
            $(this).prop('src', '/Content/images/no-image.png');            
        });

        form.on('submit', function() {
            recalculateIndexes();
        });

        addUrlImageButtons.on('click', function () {            
            var row = $(this).closest('[data-x="gift/edit/image_row"]');
            addUrlImageRow(row);
            
            return false;
        });

        addFileImageButtons.on('change', function() {
            var row = $(this).closest('[data-x="gift/edit/image_row"]');
            addFileImageRow(row);

            return false;
        });

        removeImageButtons.on('click', function() {
            var row = $(this).closest('[data-x="gift/edit/image_row"]');
            removeImageRow(row);
            
            return false;
        });
        
        addParameterButtons.on('click', function() {
            var row = $(this).closest('[data-x="gifts/edit/parameter"]');
            addParametersRow(row);
        });

        removeParameterButtons.on('click', function() {
            var row = $(this).closest('[data-x="gifts/edit/parameter"]');
            removeParametersRow(row);
        });

	    isDeliveredByEmailCheckbox.on('change', function() {
		    adjustWigthInput($(this).is(':checked'));
	    });

	    adjustWigthInput(isDeliveredByEmailCheckbox.is(':checked'));


        // --------------- Действия
        
        function recalculateIndexes() {
            var params = $('[data-x="gifts/edit/parameter"]');
            params.each(function (index) {
                var inputs = $(this).find('input,textarea');
                inputs.each(function () {
                    var name = $(this).attr('name');
                    var fixedName = name.replace(new RegExp('\\[\\d+\\]'), '[' + index + ']');
                    $(this).attr('name', fixedName);
                });
            });
        }
        
        function addUrlImageRow(row) {
            var imageUrlInput = row.find('[data-x="gift/edit/image_url_input"]');
            
            if (imageUrlInput.val() != "") {
                var clone = $(row).clone({ withDataAndEvents: true });

                setImagePreview(imageUrlInput);

                row.find('[data-x="gift/edit/image_url_input"]').attr('readonly', 'readonly');
                row.find('[data-x="gifts/edit/add_image"]').remove();
                row.find('[data-x="gifts/edit/remove_image"]').show();
                row.find('[data-x="gifts/edit/file_upload_widget"]').remove();
                
                clone.find('[data-x="gift/edit/image_url_input"]').val("");
                
                row.after(clone);

            } else {
                imageUrlInput.focus();
            }
        }
        
        function addFileImageRow(row) {
            var imageFileInput = row.find('[data-x="gifts/edit/file_input"]');
            
            if (imageFileInput.val()) {
                var clone = $(row).clone({ withDataAndEvents: true });

                row.find('[data-x="gifts/edit/remove_image"]').show();

                row.find('[data-x="gifts/edit/url_image_widget"]').remove();
                row.find('[data-x="gifts/edit/image_container"]').remove();

                row.find('[data-x="gifts/edit/file_input_button"]').hide();
                
                row.find('[data-x="gifts/edit/file_icon"]').removeClass('none');
                row.find('[data-x="gifts/edit/file_name"]').html(imageFileInput.val().match(/[^\/\\]+$/));

                row.addClass('well');
                
                row.after(clone);

                clone.find('[data-x="gift/edit/image_url_input"]').val("");
                clone.find('[data-x="gifts/edit/file_input"]').val("");
            }
            else {
                imageFileInput.focus();
            }
        }
        
        function setImagePreview(input) {
            var imgurlinput = input;
            var imgurlvalue = $(imgurlinput).val();

            var row = input.closest('[data-x="gift/edit/image_row"]');
            var imagecontainer = row.find('[data-x="gifts/edit/image_container"]');

            $(row).removeClass('well');

            $(imagecontainer).html('<img/>');
            var image = $(imagecontainer).find('img');
            $(image).attr('src', imgurlvalue).addClass('goodimage');
            $(image).load(function () {
                $(row).addClass('well');
            });
        }
        
        function removeImageRow(row) {
            var rowsCount = form.find('[data-x="gift/edit/image_row"]').length;

            if (rowsCount > 1) {
                row.remove();
            }
        }
        
        function addParametersRow(row) {
            var paramValue = row.find('[data-x="gifts/edit/parameter_value"]');

            // Проверяем, что поля текущего параметра заполнены
            var isEmpty = (paramValue.val() == "");

            if (!isEmpty) {
                var clone = $(row).clone({ withDataAndEvents: true });

                row.after(clone);
                
                var cloneInputs = clone.find('input,textarea');
                cloneInputs.val("");
            }
        }
        
        function removeParametersRow(row) {
            var rowsCount = form.find('[data-x="gifts/edit/parameter"]').length;
            
            if (rowsCount > 1) {
                row.remove();
            } else {
                row.find('input,textarea').val("");
            }
        }
	    
		function adjustWigthInput(isDeliveredByEmail) {
			if (isDeliveredByEmail) {
				weightInput.attr('disabled', 'disabled');
			} else {
				weightInput.removeAttr('disabled');
			}
		}
    };
});
