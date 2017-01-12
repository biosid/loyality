(function($) {

    // Навешивание маски при навешивании валидаторов.
    // Клиентская валидация не имеет смысла, т.к. маска уже не даст ввести некорректные значения
    
    var old = $.validator.unobtrusive.parse;
    $.validator.unobtrusive.parse = function(selector) {
        $(selector).find('input[data-val-mask-pattern]').each(function() {
            $(this).mask($(this).data('val-mask-pattern'));
        });
        old.apply(this, arguments);
    };

})(jQuery);