/**
 * Перейти по URL, указанному в value селекта
 */
(function ($) {
    $('body').on('change', 'select[data-urlselect]', function (e) {

        var val = $(this).val();
        if (val) {
            window.location = val;
        }
    });
})(jQuery);