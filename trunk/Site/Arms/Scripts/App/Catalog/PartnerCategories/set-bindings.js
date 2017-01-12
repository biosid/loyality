define('Catalog/PartnerCategories/set-bindings', ['Shared/modal-error'], function (merr) {
    return function (options) {

        // --------------- Данные

        var partnerIdInput = $('[data-x="partner_categories/partner_id"]');

        // --------------- Инициализация

        // теги(категории партнеров)
        $('[name="tags"]').tagsInput({
            'onAddTag': setPartnerCategories,
            'onRemoveTag': setPartnerCategories,
            'removeWithBackspace': false
        });

        // --------------- События

        // --------------- Действия

        function setPartnerCategories() {
            var tagsInput = $(this);

            tagsInput.siblings('.tagsinput').hide();
            tagsInput.siblings('.tags-binding-progress').show();

            var tags = tagsInput.val();
            if (tags.length) {
                tags = tags.split('|');
            } else {
                tags = [];
            }
            var supplierId = partnerIdInput.val();
            var categoryId = tagsInput.parents('tr').first().data('category').id;

            $.post(options.setBindingsUrl, { supplierId: supplierId, productCategoryId: categoryId, supplierCategories: tags })
                .done(function(response) {
                    if (response.success) {
                        updateBindings(tagsInput, response.data.bindings);
                    } else {
                        resetBindings(tagsInput);
                        var message = 'Не удалось установить привязку к категории "' + tagsInput.closest('tr').data('category').title + '"';
                        if (response.data)
                            message += ": " + response.data;
                        merr.modalError(message);
                    }
                })
                .fail(function() {
                    resetBindings(tagsInput);
                    merr.modalError('Не удалось установить привязку к категории "' + tagsInput.closest('tr').data('category').title + '"');
                })
                .always(function() {
                    tagsInput.siblings('.tags-binding-progress').hide();
                    tagsInput.siblings('.tagsinput').show();
                });
        }
        
        function updateBindings(tagsInput, tags) {
            tagsInput.importTags(tags.join('|'));
            tagsInput.closest('tr').data('category-bindings', tagsInput.val());
        }
        
        function resetBindings(tagsInput) {
            tagsInput.importTags(tagsInput.closest('tr').data('category-bindings'));
        }
    };
});