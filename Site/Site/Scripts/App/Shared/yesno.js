define('Shared/yesno', [], function () {
    var resultHandler;

    var yesno = {
        open: function (title, text, handler) {
            resultHandler = handler;

            var $body = $('body'),
                scrollTop = $body.scrollTop(),
                top = '-' + scrollTop + 'px';

            $body.data('saved-scroll-top', scrollTop);

            $body.css({
                'position': 'fixed',
                'overflow-y': 'scroll',
                'width': '100%',
                'top': top
            });
            $('[data-modal="title"]').text(title);
            $('[data-modal="text"]').html(text);
            $('[data-modal="yesno-container"]').show();
        },

        close: function (result) {
            var $body = $('body'),
                scrollTop = $body.data('saved-scroll-top');
                
            $body.css({
                'position': 'static',
                'overflow': 'auto',
                'width': 'auto',
                'top': 'auto'
            });

            $body.scrollTop(scrollTop);

            $('[data-modal="yesno-container"]').hide();
            
            if (resultHandler) {
                resultHandler(result);
            }
        }
    };

    $('[data-modal="yesno-container"]').on('click', '[data-modal="close"], [data-modal="close-no"], .modal-background', function () {
        yesno.close('no');
        return false;
    });

    $('[data-modal="yesno-container"]').on('click', '[data-modal="close-yes"]', function () {
        yesno.close('yes');
        return false;
    });

    return yesno;
});