define('Wishlist/wishlist', function(){
	return function(options){

        // --------------- Данные

		var $el = $('.gifts-grid'),
            $form = $el.closest('form');


        // --------------- События

        $el.on('click', '.gifts-grid__delete', function(){
            $(this).find('.gifts-grid__delete-btn').click();
        });

        $el.on('click', '.gifts-grid__delete-btn', function(){
            var $item = $(this).closest('tbody'),
            	id = $item.find('[name="product"]').val(),
            	count = $el.find('tbody').length - 1;

            removeItemNode($item);
            removeItem(id);
            updateHeader(count);
            
            updateButton();
            handleNotAvailableList();

            handleEmptyList(count);
        });

        $el.on('change', '[name="product"]', function(){ 
        	var checked = $(this).is(':checked'),
        		$item = $(this).closest('tbody');

        	updateSelection($item, checked);
        	updateButton();
        });

        $form.on('submit', function(){
            $('[data-disable-on="not-enouth-money"]')
                .attr('disabled', "disabled");
        });


        // --------------- Инициализация

        updateButton();
        handleNotAvailableList();


        // --------------- Действия

        function removeItemNode($item) {
        	$item.remove();
        }

        function removeItem(id) {
        	 $.get(options.removeUrl, { id: id, ajax:true})
	            .fail(function(){
	                window.location.reload();
	            });
        }

        function updateHeader(count) {
        	$('#header-wishlist-num').text(count).css('visibility', count ? 'visible' : 'hidden');
        }

        function handleEmptyList (count) {
        	if (count <= 0) {
        		$('[data-hide-on="empty-list"]').hide();
        		$('[data-show-on="empty-list"]').show()
        	}
        }

        function updateButton(){
        	var total = 0,
        		selectAll = $el.find('[name="product"]:checked').length == 0;

        	$el.find('tbody').each(function(){
        		var $this = $(this),
        			checked = selectAll || $this.data('checked'),
        			available = $this.data('available'),
        			price = $this.data('price');

        		if (checked && available) {
        			total += price;
        		}
        	});

        	if (total > options.balance) {
                var isWarned = $('[data-show-on="not-enouth-money"]').is(':visible');
                if (isWarned) {
                    return;
                }

                $.scrollTo(
                    $('[data-show-on="not-enouth-money"]').show(),
                    300
                );
        		$('[data-disable-on="not-enouth-money"]').attr('disabled', 'disabled');
        	} else {
        		$('[data-show-on="not-enouth-money"]').hide();
        		$('[data-disable-on="not-enouth-money"]').removeAttr('disabled');
        	}
        }

        function updateSelection($item, checked) {
        	$item.data('checked', checked);
        }

        function handleNotAvailableList() {
            var hasAvailable = $el.find('[data-available="true"]').length > 0;

            if (!hasAvailable) {
                $('[data-show-on="no-available-gifts"]').show();
                $('[data-disable-on="not-enouth-money"]').attr('disabled', 'disabled');
            }
        }
	};
});