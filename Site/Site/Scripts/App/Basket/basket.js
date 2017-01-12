define('Basket/basket', function(){

    return function(options) {

        // --------------- Данные

        var $el = $('.gifts-grid'),
            $form = $el.closest('form'),
            token = $('input[name=__RequestVerificationToken]:eq(0)').val();


        // --------------- События

        $el.on('click', '.gifts-grid__delete', function(){
            $(this).find('.gifts-grid__delete-btn').click();
        });

        $el.on('click', '.gifts-grid__delete-btn', function(){
            var $item = $(this).closest('tbody'),
                id = $item.find('[name="id"]').val(),
                count = $el.find('tbody').length - 1;

            removeItemNode($item);
            removeItem(id);
            updateHeader(count);

            updateButton();
            handleNotAvailableList();
            

            handleEmptyList(count);
        });

        $el.on('click', '.gift-quantity__update', onUpdate);
        $el.on('keypress', '.gift-quantity__count', function(e){
            if (e.which == 13) {
                onUpdate.call(this);
                return false;
            }
        });
        function onUpdate (argument) {

            if ($(this).is(':disabled')) {
                return;
            }

            var $item = $(this).closest('tbody'),
                id = $item.find('[name="id"]').val(),
                quantity = $item.find('[name=quantity]').val() || 0;

            if (quantity == 0) {
                $item.find('.gifts-grid__delete-btn').trigger('click');
                return;
            }

            var promise = updateQuantity(id, quantity);
            handleUpdateQuantityUi($item, quantity, promise);
        }

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
            $('#header-basket-num').text(count).css('visibility', count ? 'visible' : 'hidden');
        }

        function handleEmptyList (count) {
            if (count <= 0) {
                $('[data-hide-on="empty-list"]').hide();
                $('[data-show-on="empty-list"]').show()
            }
        }

        function updateButton(){
            var total = 0;

            $el.find('tbody').each(function(){
                var $this = $(this),
                    available = $this.data('available'),
                    price = $this.data('price');

                if (available) {
                    total += price;
                }
            });

            $('.gifts-grid__total-text').text(Globalize.format(total, 'n0'));

            if (total > options.maxOrderCost) {
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

        function handleNotAvailableList() {
            var hasAvailable = $el.find('[data-available="true"]').length > 0;

            if (!hasAvailable) {
                $('[data-show-on="no-available-gifts"]').show();
                $('[data-disable-on="not-enouth-money"]').attr('disabled', 'disabled');
            }
        }

        function updateQuantity(id, quantity) {
            return $.post(options.updateUrl, {
                    ajax: true,
                    __RequestVerificationToken: token,
                    id: id,
                    quantity: quantity 
                })
                .fail(function(){
                        window.location.reload();
                });
        }

        function handleUpdateQuantityUi ($item, quantity, promise) {
            update({
                itemPrice: $item.data('itemprice'),
                itemTotal: $item.data('itemprice') * quantity
            });

            // отключим актуализацию, чтобы избежать проблем с асинхронными вызовами
            //promise.done(update); 

            function update(data) {
                var itemPrice = Globalize.format(data.itemPrice, 'n0'),
                    itemTotal = Globalize.format(data.itemTotal, 'n0');

                $item.data('price', data.itemTotal);

                $item.find('.gifts-grid__itemtotal').text(itemPrice);
                $item.find('.gifts-grid__itemtotal').text(itemTotal);

                updateButton();
            }
        }

    };
});