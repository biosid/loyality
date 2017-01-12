define('Content/News/delete', [], function () {
    return function (options) {

        // --------------- Данные

        var deleteUrl = options.deleteUrl,
            modalConfirmation = $('[data-x="news/modal_confirm_delete"]'),
            selector = options.selector;

        // --------------- События

        // удаление категории шаг 1. кнопка в таблице
        $('[data-x="news/delete"]').on('click', function (e) {
            var tr = $(this).closest('tr');

            showConfirmation(tr);
        });
        
        // удаление категории шаг 2. кнопка в подтверждении
        $(modalConfirmation).on('click', '[data-x="news/confirm_delete"]', function () {
            var newsId = modalConfirmation.data('newsId');
            modalConfirmation.modal('hide'); // скрытие
            deleteOnServer(newsId); // удаление на сервере
            deleteInUi($('[data-news="' + newsId + '"]')); // удаление с экрана

            return false;
        });

        // --------------- Действия


        // удаление шаг 1. окно подтверждения
        function showConfirmation(tr) {
            var text = 'Действительно удалить новость?';
            modalConfirmation
                .find('[data-x="news/delete/confirm_text"]').text(text)
                .end()
                .data('newsId', tr.data('news'))
                .modal('show');
        }

        function deleteOnServer(id) {
            return $.post(deleteUrl, { id: id })
                .fail(function (response) {
                    h.error('Ошибка при удалении новости', response);
            });
        }

        function deleteInUi(row) {
            selector.reset();
            row.remove();
            
            if (!selector.length) {
                window.location.reload();
            }
        }
    };
});
