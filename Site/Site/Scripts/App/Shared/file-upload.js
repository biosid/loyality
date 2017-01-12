define('Shared/file-upload', function(){
	return function(options) {

		/**********
		*  Инициализация
		*/

		var items = {},
			$fileInput = $(options.fileInput).remove().removeAttr('id').clone(),
			sequence = 0,
			$button = $(options.button),
			$list = $(options.list),
			template = options.template 
				|| '<li class="shared-dwnlds__file shared-dwnlds__ext-{extension}"><a>{filename}</a> <span class="shared-uplds__remove" data-remove="{id}"></span></li>'
			;

			add();


		/**********
		*  События
		*/

		$button.on('click', function(){
			var item = getCurrentItem();
			item.$input.click();
		});

		$list.on('click', '[data-remove]', function() {
			var id = $(this).data('remove');
			remove(items[id]);
		});


		/**********
		*  Действия
		*/

		function add () {
			var item = { id: ++sequence, $input: $fileInput.clone() };

			item.$input.insertBefore($button);
			item.$input.on('change', function(e){ handleChange(item); });

			items[item.id] = item;
		}

		function handleChange (item) {
			var path = item.$input.val();

			if (path) {
				var data = {
					id: item.id,
					path: path,
					filename: extractFilename(path),
					extension: extractExtension(path)
				};

				item.$view = $( interpolate(template, data) );
				$list.append( item.$view ).show();
				add();
			}
		}

		function remove (item) {
			delete item.id in items;
			item.$input.remove();
			item.$view.remove();
			if ($list.is(':empty')) {
				$list.hide();
			}
		}

		function interpolate (tpl, data) {
			return tpl.replace(/\{(\w+)\}/g, function(str, field) {
				return (field in data) ? data[field] : str;
			});
		}

		function getCurrentItem () {
			var cur = null;

			for(var id in items) {
				cur = items[id];
			}

			return cur;
		}

		function extractFilename (path) {
			var pos = path.replace('/', '\\').lastIndexOf('\\') + 1,
				name = path.substring(pos);

			return name;
		}

		function extractExtension (path) {
			var name = extractFilename(path),
				pos = path.lastIndexOf('.') + 1,
				extension = pos ? path.substring(pos) : 'unknown';

			return extension;
		}

	};
});