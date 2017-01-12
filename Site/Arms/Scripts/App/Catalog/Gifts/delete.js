define('Catalog/Gifts/delete', ['Catalog/Shared/Categories/helpers'], function (h) {
    return function(options) {

        // --------------- Данные

        var selector = options.selector,
            modal = $('[data-x="gifts/delete/modal"]'),
            form = modal,
            url = form[0].action;

        // --------------- События

        $('[data-x="gifts/delete"]').on('click', function(e) {
            e.preventDefault();
            var ids = options.selector.selected();

            confirmation(ids);
        });

        form.on('submit', function(e){
            e.preventDefault();

            modal.modal('hide');

            var ids = options.selector.selected();

            deleteOnServer(ids)
                .done(deleteInUi)
                .fail(function(response){
                    h.error('Ошибка при попытке удаления товаров', response);
                });
        });

        // --------------- Действия

        function confirmation(ids) {
            var text = Globalize.pluralize(
                    ids.length,
                    'Действительно удалить {1:n0} вознаграждение?',
                    'Действительно удалить {2:n0} вознаграждения?',
                    'Действительно удалить {5:n0} вознаграждений?'
                );

            modal.find('[data-x="gifts/delete/confirm_text"]').text(text);
            modal.modal('show');
        }

        function deleteOnServer(ids) {
            return $.post(url, {Ids:ids});
        }

        function deleteInUi(ids, status) {
            selector.reset();
            window.location.reload();
        }
    };
});
