$('body').on('focus', '[data-numeric]', function () {
    var options = $(this).data('numeric') || {};
    $(this).numeric(options);

    $(this).removeAttr('data-numeric');
});