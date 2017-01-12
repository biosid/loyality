(function ($) {
    //$('.tariffsLoadMessage').hide();

    $('.sessionInfo')
	.one('click', function () {
	    var info = $(this).next();
	    info.dialog({ title: info.attr('title'), modal: false, width: '60%', autoOpen: false, height:400 });
	    $(this).click(function () { info.dialog('open'); return false; });
	    info.dialog('open');
	    return false;
	});

})(jQuery);