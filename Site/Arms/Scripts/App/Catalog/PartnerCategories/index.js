define('Catalog/PartnerCategories/index', ['Catalog/Shared/Categories/helpers'], function (h) {
    return function (options) {

        // --------------- Данные

        var tree = $(options.tree),
            unavailableToggler = $('[data-x="partner_categories/unavailable_toggler"]');

        // --------------- Инициализация

        // sticky-заголовок
        $('.sticky-save').sticky({ topSpacing: 0, center: false });
        // помечаем и схлопываем недоступные категории
        h.updateUnavailability(tree);

        // --------------- События

        // фильтр недоступных категорий
        unavailableToggler.on('change', function () {
            toggleUnavailableNodes(this.checked);
            //return false;
        });

        // --------------- Действия

        // показ/скрытие недоступных категорий
        function toggleUnavailableNodes(show) {
            tree[show ? 'removeClass' : 'addClass']('hide-not-active');
        }
    };
});
