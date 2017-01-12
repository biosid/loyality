define('Shared/popup', [], function () {
    var popup = {
        open: function (text) {
            $('body').css({ 'position': 'fixed', 'overflow-y': 'scroll', 'width': '100%' });
            $('[data-modal="text"]').html(text);
            $('[data-modal="container"]').show();
        },
        close: function() {
            $('body').css({ 'position': 'static', 'overflow': 'auto' });
            $('[data-modal="container"]').hide();
        }
    };
    
    $('[data-modal="container"]').on('click', '[data-modal="close"], .modal-background', function () {
        popup.close();
    });

    return popup;
});