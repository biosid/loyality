define('Catalog/Shared/Categories/helpers', [], function () {

	// --------------- API


	var o = {

        // обновить отображение доступности категорий
		updateUnavailability: function (tree) {
            tree.find('.not-active').removeClass('not-active');
            tree.find('tr.disabled').each(function(){
                o.branch($(this)).addClass('not-active');
            });
        },

        // отобразить ошибку сервера
        error: function (message, response) {
            alert(message);
            document.body.innerHTML = response.responseText;
            // TODO: показать сообщение об ошибке и перезагрузить
        },

        // обновить отображении ноды по наличию подкатегорий
        updateIfEmpty: function(tr){
            var meta = o.meta(tr),
                nextMeta = o.meta(tr.next('tr'));

            if (!nextMeta || nextMeta.parent != meta.id) {
                tr.addClass('empty');
                return true;
            } else {
                tr.removeClass('empty');
                return false;
            }
            
        },

        // снять выделение с категории
        unselect: function(tr) {
            tr.find('[name="category"]:checked').click();
        },

        // получить ноды категории со всеми подкатегориями
        branch: function(tr){
            return getBranchNodes(tr, true);
        },

        // получить ноды подкатегорий
        subnodes: function(tr) {
            return getBranchNodes(tr, false);
        },

        // получить полный путь категории из DOM
        path: function (tr) {
            var parent = o.node(o.meta(tr).parent),
                path = [o.title(tr)];

            while(parent.length) {
                path.push(o.title(parent));
                parent = o.node(o.meta(parent).parent);
            }

            return path.reverse().join('/');
        },

        // получить ноду категории по id
        node: function(id) {
            return $('#cat-'+id);
        },

        // получить метаданные категории из ноды
        meta: function(tr) {
            return tr.data('category');
        },

        // получить имя категории из DOM
        title: function(tr) {
            return tr.data('title') || tr.find('[name=title]').val();
        },

        // сворачивание/разворачивание ветви
        collapse: function(tr, show) {
            var branch = getBranchNodes(tr);
            show = show == null ? tr.hasClass('closed') : show;
            if (show) {
                tr.removeClass('closed');
                // откроем всё
                branch.each(function(){ $(this).show(); });
                // лишнее снова скроем :)
                branch.filter('.closed').each(function(){
                    o.collapse($(this), false);
                });
            } else {
                tr.addClass('closed');
                branch.each(function(){ $(this).hide(); });
            }
        }
	};

    return o;


	// --------------- Методы

	// получить ноды дочерних категорий
    function getBranchNodes(tr, includeParent) {
    	var depth = o.meta(tr).depth,
    		targets = $(),
    		target = tr.next();

    	while(target.length && o.meta(target).depth > depth) {
    		targets = targets.add(target);
    		target = target.next();
    	}
    	return includeParent ? tr.add(targets) : targets;
    }
    
});