define('Catalog/category-menu', function() {
    return function () {

        var $menu = $('[data-x="catalog/category-menu"]');

        $menu.menuAim({
            activate: expandSection,
            deactivate: collapseSection,
            exitMenu: function () { return true; },
            rowSelector: '[data-x="catalog/category-menu/section"]'
        });

        function expandSection(section) {
            var $section = $(section),
                $sectionTitle = $section.find('[data-x="catalog/category-menu/section-title"]'),
                $sectionMenu = $section.find('[data-x="catalog/category-menu/section-menu"]');

            $sectionTitle.addClass('catalog-menu__section-active');

            if ($sectionMenu.length) {
                $sectionTitle.addClass('catalog-menu__section-opened');
                $sectionMenu.show();
            }
        }

        function collapseSection(section) {
            var $section = $(section),
                $sectionTitle = $section.find('[data-x="catalog/category-menu/section-title"]'),
                $sectionMenu = $section.find('[data-x="catalog/category-menu/section-menu"]');

            $sectionTitle.removeClass('catalog-menu__section-active');

            if ($sectionMenu.length) {
                $sectionTitle.removeClass('catalog-menu__section-opened');
                $sectionMenu.hide();
            }
        }

    };
});
