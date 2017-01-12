(function ($) {

    $("#tabs").tabs({ selected: $.query.get('t') });

    $('#tabs').delegate('a.submit', "click", function () {
        $(this).after('<input type="hidden" name="' + this.rel + '" value="1"/>').closest('form').submit();
        $(this).next(':hidden').remove();
        return false;
    });

    $('.accountNumberRangeAddressCheckBox').click(function () {
        if ($('.accountNumberRangeAddressCheckBox:checked').length > 0) {
            $('#deleteAccountNumberRangeAddress').addClass('submit');
            $('#deleteAccountNumberRangeAddress').removeClass('disabled');
        }
        else {
            $('#deleteAccountNumberRangeAddress').removeClass('submit');
            $('#deleteAccountNumberRangeAddress').addClass('disabled');
        }
    });

})(jQuery);