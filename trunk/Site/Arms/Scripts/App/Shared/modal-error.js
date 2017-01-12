define('Shared/modal-error', [], function () {

    $('[data-x="shared/modal_ajax_error"]').on('keyup', function(e) {
        if (e.which == 13 || e.which == 27) {
            $(this).modal('hide');
        }
    });

    // --------------- API
    
    var o = {
        modalError: function(message, closeHandler) {
            var modal = $('[data-x="shared/modal_ajax_error"]');
            
            modal.find('span').text(message);
            modal.modal('show');

            var hideHandler = function(e) {
                if (e.which == 13 || e.which == 27) {
                    modal.modal('hide');
                }
            };

            $(document).on('keydown', hideHandler);

            modal.on('hide', function() {
                $(document).off('keydown', hideHandler);
                closeHandler && closeHandler();
            });
        }
    };

    return o;
});
