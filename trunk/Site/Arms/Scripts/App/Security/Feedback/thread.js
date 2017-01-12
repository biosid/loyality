define('Security/Feedback/thread', [], function () {

    return function (container) {

        var form = container.find('form'),
            submitBtn = form.find('button[type="submit"]');

        form.on('submit', function() {
            submitBtn.attr('disabled', 'disabled');
        });

        var fileUploadProto = container.find('.fileupload:eq(0)').clone();

        container.on('change', '.fileupload input', function() {
        	var widget = $(this).closest('.fileupload');
            if ($(this).val() && widget.next('.fileupload').length == 0) {
                fileUploadProto.clone().insertAfter(widget);
            }
        });
    };

});