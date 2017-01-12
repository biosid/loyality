define('Client/advertisement', function () {
	return function (options) {

		var $popup = $(options.popup);

		$popup.dialog({
			autoOpen: false,
			modal: true,
			closeOnEscape: true,
			width: 'auto',
			height: 'auto',
			dialogClass: options.dialogClass,
			position: { my: "center" }
		});

		$(function () {
			// http://bugs.jqueryui.com/ticket/4053
			setTimeout(function () { $popup.dialog('open'); }, 100);
		});
	};
});
