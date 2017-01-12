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

    var saveButtonClicked = false;
    $('input[name="addAccountNumberRangeButton"]').click(function () {
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
				"/AccountNumberRangeEdit/GetAddressElementsByParent",
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

})(jQuery);