(function ($) {

    var ddlRegion = $("#ddlRegion");
    var ddlDistrict = $("#ddlDistrict");
    var ddlCity = $("#ddlCity");
    var ddlTown = $("#ddlTown");
    var tFields = $("#tFields");
    var lastChoiceElement;

    function DropDownList_Chanhed(sender, level) {
        var query = { level: level };
        if (level == "Region" || level == "District" || level == "City") {
            query.region = ddlRegion.val();
        }
        if (level == "District" || level == "City") {
            query.district = ddlDistrict.val();
        }
        if (level == "City") {
            query.city = ddlCity.val();
        }

        $.getJSON
			(
				"/TariffEdit/GetAddressElementsByParent",
				query,
				function (data) {
				    if (data != null) {
				        BindOptions(ddlDistrict, data.districts);
				        BindOptions(ddlCity, data.cities);
				        BindOptions(ddlTown, data.towns);
				    }
				}
			);

        $(sender).effect("highlight", {}, 1000);
    }

    function BindOptions(dropDownList, data, selectedIndex) {

        if (data == null) {
            return;
        }

        if (data.length == 0) {
            dropDownList
					.find("option")
					.remove()
					.end();
            dropDownList.attr("disabled", "disabled");
        }
        else {
            dropDownList.removeAttr("disabled");
            dropDownList
					.find("option")
					.remove()
					.end();
            $.each
				(
					data,
					function (key, value) {
					    var option = "<option value='" + value.Value + "' ";
					    if ((key + 1) == selectedIndex) {
					        option += " selected='selected' "
					    }
					    option += " >" + value.Text + "</option>";
					    dropDownList.append(option)
					}
				);
        }
        //dropDownList.ufd("changeOptions");
        dropDownList.prev().prev().effect("highlight", {}, 1000);
    }

    //$('select').ufd();
    $('#addBindingDiv').hide();
    $('#addBindingDiv').dialog(
    {
        title: $('#addBindingDiv').attr('title'),
        modal: true,
        width: '40%',
        autoOpen: false,
        zIndex: 50,
        draggable: false
    });

    $('#addBinding').click(function () {
        $('#addBindingDiv').dialog('open');
        return false;
    });

    ddlRegion.change(function () {
        DropDownList_Chanhed(this, 'Region');
        lastChoiceElement = ddlRegion.val();
    });

    ddlDistrict.change(function () {
        DropDownList_Chanhed(this, 'District');
        lastChoiceElement = ddlDistrict.val();
    });

    ddlCity.change(function () {
        DropDownList_Chanhed(this, 'City');
        lastChoiceElement = ddlCity.val();
    });


    function GetMostDetailedElementId() {
        if (ddlTown.val() != null && ddlTown.val() != '')
            return '#ddlTown'
        if (ddlCity.val() != null && ddlCity.val() != '')
            return '#ddlCity'
        if (ddlDistrict.val() != null && ddlDistrict.val() != '')
            return '#ddlDistrict'
        if (ddlRegion.val() != null && ddlRegion.val() != '')
            return '#ddlRegion'
    }

    $("#addBindingButton").click(function () {
        var newElementId = GetMostDetailedElementId();

        if (newElementId == null)
            return;

        var newElemenValue = $(newElementId).val();
        var newElementText = $(newElementId).find('option:selected').text();

        if ($('#bindingsListDiv ol li input[value=' + newElemenValue + ']').length != 0) {
            $('#addBindingDiv').dialog('close');
            return;
        }

        var index = $("#bindingsListDiv ol li input").length;

        $('#addBindingDiv').dialog('close');

        $("#bindingsListDiv>ol").append('<li><input type="hidden" value="' + newElemenValue +
        '" name="CityBindings[' + index + '].Value" id="CityBindings_' + index + '__Value">' + newElementText +
        '&nbsp;<a class="deleteBinding" href="#">Удалить</a></li>');
    });

    $("#bindingsListDiv").delegate(".deleteBinding", "click", function () {
        $(this).parent().remove();

        $("#bindingsListDiv ol li input").each(function (index, elem) {
            var elemenValue = $(elem).attr('value');
            $(elem).replaceWith('<input type="hidden" value="' + elemenValue + '" name="CityBindings[' + index + '].Value" id="CityBindings_' + index + '__Value">')
        })
        return false;
    });

    $("#sendToArchive").click(function () {
        return confirm("Тарифы отправленные в архив недоступны для редактирования. Отправить тариф в архив?");
    });

    $('.text, .text2, select, .text-box, .rbTriff, .rbTraffic, #Tariff_IsPromo, #Tariff_AvailableAllRegions').focus(function () {
        $(this).closest("p").addClass('focus');
        $(this).closest("div.p").addClass('focus');
    });

    $('.text, .text2, select, .text-box, .rbTriff, .rbTraffic, #Tariff_IsPromo, #Tariff_AvailableAllRegions').blur(function () {
        $(this).closest("p").removeClass('focus');
        $(this).closest("div.p").removeClass('focus');
    });

    $('#blockMessage').dialog({
        title: $('#blockMessage').attr('title'),
        modal: true,
        width: '50%',
        autoOpen: true,
        zIndex: 50,
        draggable: false
    });

    $('#blockMessageClose').click(function () {
        $('#blockMessage').dialog('close');
        return;
    });

    $("textarea[maxlength]").bind('paste', function (event) {
        if (window.clipboardData && clipboardData.getData) {
            var maxLength = $(this).attr("maxlength");
            var length = $(this).val().length + clipboardData.getData("text").length;
            if (length >= maxLength) {
                event.preventDefault();
                var len = maxLength - $(this).val().length;
                var val = $(this).val() + $(this).val().substring(0, len);
                $(this).val(val);
            }
        }
    });
    $("textarea[maxlength]").keypress(function (event) {
        var key = event.which;

        //all keys including return.
        if (key >= 33 || key == 13) {
            var maxLength = $(this).attr("maxlength");
            var length = this.value.length;
            if (length >= maxLength) {
                event.preventDefault();
            }
        }
    });

})(jQuery);