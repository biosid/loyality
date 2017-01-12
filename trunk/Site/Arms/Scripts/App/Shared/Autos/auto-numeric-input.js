(function($) {
    $('[data-numeric]').each(function() {
        var options = $(this).data('numeric') || {};
        $(this).numeric(options);
        $(this).removeAttr('data-numeric');
    });
    
    $('[data-signed-numeric]').each(function () {
        var options = $.extend({ allow: '-' }, $(this).data('numeric') || {});
        $(this).numeric(options);
        $(this).removeAttr('data-signed-numeric');
    });

    $('[data-floatnumeric]').each(function () {
        var options = $.extend({ allow: ',.' }, $(this).data('numeric') || {});
        $(this).numeric(options);
        $(this).removeAttr('data-floatnumeric');
    });

    $('[data-signed-floatnumeric]').each(function () {
        var options = $.extend({ allow: ',.-' }, $(this).data('numeric') || {});
        $(this).numeric(options);
        $(this).removeAttr('data-signed-floatnumeric');
    });
})(jQuery);
