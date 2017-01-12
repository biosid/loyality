define('Catalog/PartnerCategories/change-availability', ['Catalog/Shared/Categories/helpers'], function (helpers) {
    return function (options) {

        // --------------- Данные

        var tree = $(options.tree),
            actionsPanel = $('[data-x="partner_categories/actions_panel"]'),
            btnSaveChanges = $('[data-x="partner_categories/save_changes"]'),
            btnCancelChanges = $('[data-x="partner_categories/cancel_changes"]'),
            partnerIdInput = $('[data-x="partner_categories/partner_id"]')
        ;

        // --------------- События

        tree.on('change', '[name=permission]', function() {
            actionsPanel.show();

            //return false;
        });

        // сохранение изменений
        btnSaveChanges.on('click', function () {
            saveChanges();
            return false;
        });

        // отмена изменений
        btnCancelChanges.on('click', function () {
            cancelChanges();
            return false;
        });

        // --------------- Действия

        function cancelChanges() {
            window.location.reload();
        }
        
        function saveChanges() {
            var checkedCategories = tree.find('[type="checkbox"]:checked');
            var categories = [];
            checkedCategories.each(function () {
                var id = $(this).closest('tr').data('category').id;
                categories.push(id);
            });

            var supplierId = partnerIdInput.val();
            $.post(options.changeUrl, { supplierId: supplierId, categoriesIds: categories })
                .fail(function (response) {
                    helpers.error('Ошибка при изменении доступности категорий', response);
                })
                .done(function (data) {
                    if (data.updated != length) {
                        window.location.reload();
                    }
                });
        }
    };
});
