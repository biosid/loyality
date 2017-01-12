(function ($) {
    $('#ddlProviders, #ddlStatus, #PageSize').change(function () { this.form.submit() });

    $('#tariffList .marketInfo')
		.one('click', function () {
		    var info = $(this).next();
		    info.dialog({ title: info.attr('title'), modal: false, width: '70%', autoOpen: false });
		    $(this).click(function () { info.dialog('open'); return false; });
		    info.dialog('open');
		    return false;
		});

	$('#tariffList .tariffBlockDescriptionLink')
	.one('click', function () {
		var info = $(this).next();
		info.dialog({ title: info.attr('title'), modal: false, width: '40%', autoOpen: false });
		$(this).click(function () { info.dialog('open'); return false; });
		info.dialog('open');
		return false;
	});

    $("#tariffList>tbody>tr:odd").addClass("even");
    $("#tariffList>tbody>tr:even").addClass("odd");

	$('.sorting').click(function () {
		$('#sortColumnName').val(this.id);

		if ($(this).hasClass('sorting_asc')) {
		    $('#sortDirection').val('desc');
		}
		else {
		    $('#sortDirection').val('asc');
		}

		$(this).closest('form').submit();
		return false;
	});

})(jQuery);