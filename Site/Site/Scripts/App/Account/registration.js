define('Account/registration', function() {
    return function (options) {
        var form = $(options.el),
            submitButton = form.find('[type="submit"]'),
            refreshCaptchBtn = $(options.refreshCaptcha),
            captchaImage = $(options.captchaImage);
            
        
        var yearsDropDown = form.find('[name="dob_yyyy"]'),
            monthsDropDown = form.find('[name="dob_mm"]'),
            daysDropDown = form.find('[name="dob_dd"]'),
            daysLabelOptions = daysDropDown.find('option:eq(0)');

        // нужна валидация скрытых элементов (у нас там день, месяц и год рождения)
        $(function(){ // инициализация отложена до события DomReady (здравствуй, Опера)
            var formValidator = form.data('validator');
            formValidator.settings.ignore = '';
        });

        $([yearsDropDown, monthsDropDown, daysDropDown]).select2({ minimumResultsForSearch: -1, placeholderOption:'first' });

        if (refreshCaptchBtn) {
            refreshCaptchBtn.click(function() {
                captchaImage.attr('src', captchaImage.attr('src') + '&rnd=' + new Date().getMilliseconds());
            });
        }


        function populateDaysDropDown() {
            var year = yearsDropDown.val() ? parseInt(yearsDropDown.val()) : 1992;
            var month = parseInt(monthsDropDown.val());
            var selectedDate = daysDropDown.val() ? parseInt(daysDropDown.val()) : null;

            if (month) {
                var days = [];
                var daysInMonth = new Date(year, month, 0).getDate();

                for (var i = 1; i <= daysInMonth; i++) {
                    days.push(i);
                }

                daysDropDown.empty().append(daysLabelOptions);

                $(days).each(function() {
                    $("<option />", {
                        val: this,
                        text: this
                    }).appendTo(daysDropDown);
                });
                
                if (selectedDate) {
                    selectedDate = Math.min(Math.max(selectedDate, 1), daysInMonth);
                    daysDropDown.val(selectedDate);
                }

                $(daysDropDown).select2({ minimumResultsForSearch: -1, placeholderOption: 'first' });
            }
        }
        
        function copyValuesFromCombo() {
            form.find('[name="BirthYear"]').val(yearsDropDown.val());
            form.find('[name="BirthMonth"]').val(monthsDropDown.val());
            form.find('[name="BirthDate"]').val(daysDropDown.val());
        }
        
        function onDateDropDownChange() {
            populateDaysDropDown();
            copyValuesFromCombo();
        }

        yearsDropDown.on('change', function () {
            onDateDropDownChange();
        });
        
        monthsDropDown.on('change', function () {
            onDateDropDownChange();
        });

        daysDropDown.on('change', function() {
            copyValuesFromCombo();
        });

        $('.checkbox-label .check-box-image').show();

        $('input[type="checkbox"]').each(function () {
            updateCheckbox($(this));
        });

        checkAgreement();

        $('input[type="checkbox"]').on('change', function () {
            updateCheckbox($(this));
            checkAgreement();
        });

        $('.checkbox-label').on('mouseenter', function() {
            $(this).toggleClass('checkbox-hover', true);
        });
        $('.checkbox-label').on('mouseleave', function() {
            $(this).toggleClass('checkbox-hover', false);
        });

        function updateCheckbox(checkbox) {
            var checked = checkbox.is(':checked');
            checkbox
                .closest('.checkbox-label')
                .find('.check-box-image')
                .toggleClass('checkbox-checked', checked);
        }
        
        function checkAgreement() {
            var submitDisabled = $('[data-x="account/registartion/required_checkbox"]').not(':checked').length > 0;
            submitButton.prop('disabled', submitDisabled);
        }
    };
});