(function ($) {    

    $("#tabs").tabs({ selected: $.query.get('t') });

    $("#tabs-3").delegate('#deleteTariffArticleRule.submit', "click", function (e) {
        if (!confirm("Удалить отмеченные правила?")) {
            e.stopImmediatePropagation();
            return false;
        } 
    });

    $("#tabs-2").delegate('#deleteProviderBranch.submit', "click", function (e) {
        if (!confirm("Удалить отмеченные юрлица?")) {
            e.stopImmediatePropagation();
            return false;
        }
    });    

    $('#tabs').delegate('a.submit', "click", function () {
        $(this).after('<input type="hidden" name="' + this.rel + '" value="1"/>').closest('form').submit();
        $(this).next(':hidden').remove();
        return false;
    });

    $('.ruleCheckBox').click(function () {
        if ($('.ruleCheckBox:checked').length > 0) {
            $('#deleteTariffArticleRule, #applyTariffArticleRule').addClass('submit');
            $('#deleteTariffArticleRule, #applyTariffArticleRule').removeClass('disabled');
        }
        else {
            $('#deleteTariffArticleRule, #applyTariffArticleRule').removeClass('submit');
            $('#deleteTariffArticleRule, #applyTariffArticleRule').addClass('disabled');
        }
    });

    $('.providerBranchCheckBox').click(function () {
        if ($('.providerBranchCheckBox:checked').length > 0) {
            $('#deleteProviderBranch').addClass('submit');
            $('#deleteProviderBranch').removeClass('disabled');
        }
        else {
            $('#deleteProviderBranch').removeClass('submit');
            $('#deleteProviderBranch').addClass('disabled');
        }
    });

})(jQuery);