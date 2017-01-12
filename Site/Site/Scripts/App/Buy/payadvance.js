define('Buy/payadvance', function () {
    return function() {

        $(window).on('message', handleMessage);

        function handleMessage(e) {
            if (e.originalEvent.origin != 'https://wpay.uniteller.ru') {
                return;
            }

            var height = e.originalEvent.data;
            if (typeof height != 'number') {
                return;
            }

            $('iframe#pay-advance').attr('height', height);
        }
    };
});
