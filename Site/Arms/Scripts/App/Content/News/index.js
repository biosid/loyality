define('Content/News/Index', ['Shared/dtinterval'], function (dti) {
    return function (options) {
        // --------------- Данные

        var navbar = $('[data-x="news/navbar"]'),
            hideUnpublished = $('[data-x="news/hideUnpublished"]');

        // --------------- Инициализация

        dti.setupDtInterval('[data-x="news/dateFrom"]', '[data-x="news/dateTo"]');
        
        navbar.sticky({ topSpacing: 0, center: false });

        hideUnpublished.on('change', function () {
            if (hideUnpublished.prop("checked")) {
                window.location = options.hideUnpublishedUrl;
            } else {
                window.location = options.showUnpublishedUrl;
            }
        });

        // --------------- Действия
    };
});
