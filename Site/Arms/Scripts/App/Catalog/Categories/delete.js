define('Catalog/Categories/delete', ['Catalog/Shared/Categories/helpers'], function (h) {
	return function(options){

		// --------------- Данные


	    var tree = $(options.tree),
        	modalConfirmation = $('[data-x="categories/modal_confirm_delete"]')
            ;


        // --------------- События


        // удаление категории шаг 1. кнопка в таблице
        tree.on('click', '[data-x="categories/remove"]', function(){
        	var tr = $(this).closest('tr'),
        		category = tr.data('category');

        	if (!category.empty) {
        		return true;
        	}

            showConfirmation(tr);

        	return false;
        });

        // удаление категории шаг 2. кнопка в подтверждении
        $(modalConfirmation).on('click', '[data-x="categories/confirm_delete"]', function(){
            var category = modalConfirmation.data('category');
            modalConfirmation.modal('hide'); // скрытие
            deleteOnServer(category.id); // удаление на сервере
            deleteInUi($('#cat-' + category.id)); // удаление с экрана

            return false;
        });


        // --------------- Действия


        // удаление шаг 1. окно подтверждения
        function showConfirmation(tr) {
            var text = 'Действительно удалить категорию "' + h.path(tr) + '"?';
            modalConfirmation
                .find('[data-x="categories/delete/confirm_text"]').text(text)
                .end()
                .data('category', tr.data('category'))
                .modal('show');
        }

        // удаление шаг 2а. отправка команды на удаления
        function deleteOnServer(id){
        	$.post(options.deleteUrl, {id: id})
            	.fail(function(response){
                    h.error('Ошибка при удалении категории', response);
            	});
        }

        // удаление шаг 2б. удаление всей ветви с экрана
        function deleteInUi(tr){
            var meta = h.meta(tr),
                parent = h.node(meta.parent);

            h.branch(tr)
                .each(function(){ 
                    h.unselect($(this)); // снимаем выбор с удаляемых категорий
                    $(this).remove(); 
                });

            h.updateIfEmpty(parent);

            $(document).trigger('categoryDeleted', [meta.id]);
        }
	};
});