(function ($) {
    var elements = $('.field-title');

    elements.on('mouseenter', function () {
        $(this).toggleClass('title-hovered', true);
    });

    elements.on('mouseleave', function () {
        $(this).toggleClass('title-hovered', false);
    });
})(jQuery);