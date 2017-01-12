define('Feedback/index', ['Shared/captcha'], function (captcha) {
    return function (options) {
        var form = $(options.el),
            popup = $(options.popup),
            submitBtn = form.find('#submit-button'),
            initialType = $('#SelectedType').val();
        
        captcha(options.captcha);
        handleEmailValidation(initialType);
        handleOrdersToggle(initialType);

        popup.dialog({
            autoOpen: false,
            modal: true,
            closeOnEscape: false,
            closeText: "",
            minWidth: 600
        });

        form.find('#SelectedType').select2({
            minimumResultsForSearch: -1,
            allowClear: false
        });

        form.find('#OrderId').select2({
            placeholderOption: 'first',
            minimumResultsForSearch: -1,
            allowClear: false
        });
        if (!options.hasOrders) {
            $('#OrderId').select2("readonly", true);
        }


        $('#SelectedType').on('change', function(){
            var type = $(this).val();
            handleEmailValidation(type);
            handleOrdersToggle(type);

        });

        submitBtn.on('click', function () {
            if (form.find('#SelectedType').val() === 'Unsubscribe' && form.valid()) {
                popup.dialog('open');

                return false;
            }

            return true;
        });

        popup.find('#yes-button').on('click', function() {
            form.submit();
            
            popup.dialog('close');
        });
        
        popup.find('#no-button').on('click', function () {
            popup.dialog('close');
        });

        function handleEmailValidation(selectedType){
            // email нужен только для гостей и для отключения от программы
            $('#Email').rules('remove', 'required');
            var validate = !options.isClient || selectedType == 'Unsubscribe';
            if (validate) {
                $('#Email').rules('add', 'required');
            }
        }

        function handleOrdersToggle (selectedType) {
            var ordersAction = selectedType == 'OrderIncident' ? 'show' : 'hide';
            form.find('#OrdersDropdown')[ordersAction]();
        }
    };
});