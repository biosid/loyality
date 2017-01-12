define('Catalog/Gifts/index', ['Catalog/Shared/Categories/navigation'], function (nav) {
    return function() {
        // --------------- Данные

        var navbar = $('[data-x="gifts/navbar"]'),
            breadCrumbs = $('[data-x="gifts/bread_crumb"]'),
            categories = $('[data-x="gifts/categories"]');

        // --------------- Инициализация

        navbar.sticky({ topSpacing: 0, center: false });

        nav.categoryNavigation(categories, { autoCollapse: true, withSelectableItems: false});

        // --------------- События

        breadCrumbs.on('click', function (e) {
            e.stopPropagation();
        });
    };
});
