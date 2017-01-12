// Страница выбора тарифа

(function ($) {

    $.datepicker.setDefaults($.datepicker.regional['ru']);
    var datepickerDefaults = {
        buttonImageOnly: true,
        buttonImage: '/Content/images/calendar.png',
        buttonText: 'Calendar',
        showOn: 'button',
        minDate: new Date(2010, 0, 1),
        maxDate: new Date(2900, 0, 1)
    }
    $.datepicker.setDefaults(datepickerDefaults);

    $('#DateFrom, #DateTo').datepicker({
        onClose: function (dateText, inst) { this.form.submit(); }
    });

    //$("#DateFrom, #DateTo").mask("99.99.9999");

//    $('#DateFrom, #DateTo').change(function () {
//        try {
//            var date = $.datepicker.parseDate('dd.mm.yy', $(this).val());
//            if (datepickerDefaults.minDate > date || datepickerDefaults.maxDate < date) {
//                throw true;
//            }
//            this.form.submit();
//        }
//        catch (e) {
//            $(this).val('');
//        }
//    });

    $('#provider, #providerBranches, #status, #ddlRegion, #ddlLocality, #PageSize, #DateFrom, #DateTo').change(function () { this.form.submit() });

    $('.errorinfo')
		.one('click', function () {
		    info = $(this).next();
		    info.dialog({ title: info.attr('title'), modal: false, width: '70%', autoOpen: false });
		    $(this).click(OpenDialog);
		    OpenDialog.call(this);
		    return false;
		});

    function OpenDialog() {
        var orderId = $(this).attr('href');
        $.getJSON(
			'/Order/GetErrorInfo',
			{ orderId: orderId },
			function (data) {
			    info.find('span').html(data.text);
			    info.dialog('open');
			}
		);
        return false;
    }

    $('#orderItems>thead>tr>th').click(function () {
        $('#sortColumnName').val(this.id);

        if ($(this).hasClass('sorting_asc')) {
            $('#sortDirection').val('desc');
        }
        else {
            $('#sortDirection').val('asc');
        }

        $(this).closest('form').submit();
    });

    $('#excelExportLink').click(function () {
        //$('#excelExportHidden').remove();
        $(this).after('<input type="hidden" id="excelExportHidden" name="' + this.rel + '" value="1"/>').closest('form').submit();        
        $(this).next(':hidden').attr('disabled', 'disabled');
        return false;
    });

})(jQuery);