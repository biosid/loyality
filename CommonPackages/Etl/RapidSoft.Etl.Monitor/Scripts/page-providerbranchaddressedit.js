(function ($) {
    var ddlRegion = $("#ddlRegion");
    var ddlDistrict = $("#ddlDistrict");
    var ddlCity = $("#ddlCity");
    var ddlTown = $("#ddlTown");
    var tFields = $("#tFields");

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
				"/ProviderBranchAddressEdit/GetAddressElementsByParent",
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

    $('.text, .text2, select, .text-box, .rbTriff, .rbTraffic, #Tariff_IsPromo, #Tariff_AvailableAllRegions').focus(function () {
        $(this).closest("p").addClass('focus');
        $(this).closest("div.p").addClass('focus');
    });

    $('.text, .text2, select, .text-box, .rbTriff, .rbTraffic, #Tariff_IsPromo, #Tariff_AvailableAllRegions').blur(function () {
        $(this).closest("p").removeClass('focus');
        $(this).closest("div.p").removeClass('focus');
    });

})(jQuery);