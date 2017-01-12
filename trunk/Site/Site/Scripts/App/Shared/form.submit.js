define('Shared/form.submit', [], function() {
    return function (options) {
        var form = $(options.form), 
            submitBtn = form.find('[type="submit"]');

        form.on('submit', function () {
            if ($(this).valid()) {
                disableSubmit();
            }
        });

        function disableSubmit() {
            submitBtn.prop('disabled', true);
        }
    };
});