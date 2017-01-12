(function ($) {

    $("#tabs").tabs({ selected: $.query.get('t') });

    $('.text, .text2, select, .text-box, .rbTriff, .rbTraffic, #Tariff_IsPromo, #Tariff_AvailableAllRegions').focus(function () {
        $(this).closest("p").addClass('focus');
        $(this).closest("div.p").addClass('focus');
    });

    $('.text, .text2, select, .text-box, .rbTriff, .rbTraffic, #Tariff_IsPromo, #Tariff_AvailableAllRegions').blur(function () {
        $(this).closest("p").removeClass('focus');
        $(this).closest("div.p").removeClass('focus');
    });

    $('#tabs').delegate('a.submit', "click", function () {
        $(this).after('<input type="hidden" name="' + this.rel + '" value="1"/>').closest('form').submit();
        $(this).next(':hidden').remove();
        return false;
    });

    $('.providerBranchAddressCheckBox').click(function () {
        if ($('.providerBranchAddressCheckBox:checked').length > 0) {
            $('#deleteProviderBranchAddress').addClass('submit');
            $('#deleteProviderBranchAddress').removeClass('disabled');
        }
        else {
            $('#deleteProviderBranchAddress').removeClass('submit');
            $('#deleteProviderBranchAddress').addClass('disabled');
        }
    });

    $('.accountNumberRangeCheckBox').click(function () {
        if ($('.accountNumberRangeCheckBox:checked').length > 0) {
            $('#deleteAccountNumberRange').addClass('submit');
            $('#deleteAccountNumberRange').removeClass('disabled');
        }
        else {
            $('#deleteAccountNumberRange').removeClass('submit');
            $('#deleteAccountNumberRange').addClass('disabled');
        }
    });    

})(jQuery);