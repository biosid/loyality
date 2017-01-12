define('Main/index', function() {
    return function (options) {
        // новости
        $(options.newsBlock).tinycarousel({ display: 1, duration: 200 });
        // популярные товары
        $(options.popularBlock).tinycarousel({ controls: false, pager: true, animation: false, start:3 });
    };
});