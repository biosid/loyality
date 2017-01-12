(function ($) {

    $("#tabs").tabs({
        selected: $.query.get('t'),
        select: function (event, ui) {
            $("#CurrentTabIndex").val(ui.index);
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
    $('input[name="saveProviderButton"]').click(function () {
        saveButtonClicked = true;
    });

    var state = $("form").serialize();
    window.onbeforeunload = function () {
        if (!saveButtonClicked && state != $("form").serialize()) {
            return 'Изменения не сохранены.\nДля сохранения нажмите кнопку "Сохранить".\nЕсли вы покините страницу изменения будут утеряны.';
        }
        window.unload = false;
    };

    $('#Provider_Name').keyup(function () {
        $('#providerName').text($('#Provider_Name').val());
    });
        
})(jQuery);