(function ($) {

    $("#tabs").tabs({
        selected: $.query.get('t'),
        select: function (event, ui) {
            $("#BackTabIndex").val(ui.index);
        }
    });

    $('.text, .text2, select, .text-box, .rbTriff, .rbTraffic, #Tariff_IsPromo, #Tariff_AvailableAllRegions').focus(function () {
        $(this).closest("p").addClass('focus');
        $(this).closest("div.p").addClass('focus');
    });

    $('.text, .text2, select, .text-box, .rbTriff, .rbTraffic, #Tariff_IsPromo, #Tariff_AvailableAllRegions').blur(function () {
        $(this).closest("p").removeClass('focus');
        $(this).closest("div.p").removeClass('focus');
    });

    var saveButtonClicked = false;
    $('input[name="saveProviderBranchButton"], input[name="addProviderBranchButton"]').click(function () {
        saveButtonClicked = true;
    });

    $("#BackTabIndex").attr('disabled', true);
    var state = $("form").serialize();
    $("#BackTabIndex").attr('disabled', false);
    
    window.onbeforeunload = function () {
        $("#BackTabIndex").attr('disabled', true);
        var newState = $("form").serialize();
        $("#BackTabIndex").attr('disabled', false);

        if (!saveButtonClicked && state != newState) {
            return 'Изменения не сохранены.\nДля сохранения нажмите кнопку "Сохранить".\nЕсли вы покините страницу изменения будут утеряны.';
        }
        window.unload = false;
    };

    $('#ProviderBranch_Name').keyup(function () {
        $('#providerBranchName').text($('#ProviderBranch_Name').val());
    });

    $('#radioFlatNumeric1, #radioFlatText1').change(function () {
        $('#radioFlatNumeric2').attr('checked', $('#radioFlatText1').attr('checked'));
        $('#radioFlatText2').attr('checked', $('#radioFlatText1').attr('checked'));
    });

    $('#radioFlatNumeric2, #radioFlatText2').change(function () {
        $('#radioFlatNumeric1').attr('checked', $('#radioFlatNumeric2').attr('checked'));
        $('#radioFlatText1').attr('checked', $('#radioFlatText2').attr('checked'));
    });

})(jQuery);