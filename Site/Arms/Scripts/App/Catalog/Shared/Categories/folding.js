define('Catalog/Shared/Categories/folding', ['Catalog/Shared/Categories/helpers'], function (h) {
    return function(options) {

    	// --------------- Данные


        var tree = $(options.tree);


		// --------------- События
        

        // сворачивание/разворачивание ветвей
        tree.on('click', '[data-x="categories/toggler"]', function(){
            h.collapse($(this).closest('tr'));
        	return false;
        });

    };
});