$.validator.setDefaults({
    highlight: function(element) {
        $(element).closest('.form-group').addClass('has-error');
    },
    unhighlight: function(element) {
        $(element).closest('.form-group').removeClass('has-error');
    }
});

$(function() {
    $('span.field-validation-error').closest('.form-group').addClass('has-error');
    $('form').on('changeDate', 'input', function () {
        var $form = $(this).closest('form');
        if ($form.data('validator')) {
            $form.validate().element(this);
        }
    });
});
