define('Shared/frame-resizer', ['Shared/query-parser'], function (parser) {
    return function (options) {

        // ---------------- события

        $(window).on('message', handleMessage);

        // ---------------- действия

        function handleMessage(e) {
            if (!options.iframe || !e.originalEvent.data) {
                return;
            }

            var data = parser(e.originalEvent.data);

            switch(data.t) {
                case 'height':
                    setHeight( parseInt(data.v, 10) );
                    break;
                case 'scroll':
                    scroll( data.v );
                    break;
            }
        }

        function setHeight (height) {
            $(options.iframe).attr('height', height);
        }

        function scroll (where) {
            switch (where) {
                case 'top':
                    window.scrollTo(0, 0);
                    break;
                case 'bottom':
                    window.scrollTo(0, document.body.scrollHeight);
                    break;
            }
        }
    };

});
