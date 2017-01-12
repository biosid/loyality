(function($) {

    // ����������� ����� ��� ����������� �����������.
    // ���������� ��������� �� ����� ������, �.�. ����� ��� �� ���� ������ ������������ ��������
    
    var old = $.validator.unobtrusive.parse;
    $.validator.unobtrusive.parse = function(selector) {
        $(selector).find('input[data-val-mask-pattern]').each(function() {
            $(this).mask($(this).data('val-mask-pattern'));
        });
        old.apply(this, arguments);
    };

})(jQuery);