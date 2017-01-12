define('Content/Files/index', [], function () {
    return function (options) {
        // --------------- Данные

        var elfinderBox = $('[data-x="site/files/elfinder"]'),
            urlBox = $('[data-x="site/files/urlbox"]');

        // --------------- Инициализация

        elfinderBox.elfinder({
            url: '/site/files/connector',
            lang: 'ru',
            resizable: false,
            height: 600,
            uiOptions: {
                toolbar: [
                    ['back', 'forward'],
                    ['reload'],
                    ['home', 'up'],
                    ['mkdir', 'mkfile', 'upload'],
                    ['open', 'download'],
                    ['info'],
                    ['quicklook'],
                    ['copy', 'cut', 'paste'],
                    ['rm'],
                    ['duplicate', 'rename', 'edit'],
                    ['view', 'sort']
                ]
            },
            handlers: {
                select: selectFile
            }
        });

        // --------------- События

        urlBox.on('click', function() {
            urlBox.select();
        });

        // --------------- Действия

        function selectFile(e) {
            if (e.data.selected.length == 1) {
                var item = $('#' + e.data.selected[0]);
                if (!item.hasClass('directory')) {
                    var selectedFile = e.data.selected[0];
                    $.get(options.fileUrl + '/' + selectedFile)
                        .done(function(response) {
                            urlBox.val(response);
                        })
                        .fail(function() {
                            urlBox.val('');
                        });
                }
            }
            urlBox.val('');
        }

    };
});
