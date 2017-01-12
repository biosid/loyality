define('Shared/bonus_calc_v1', function () {
    return function (options) {

    	// --------------- Данные

    	var $el = options.el,
    		$target = $el.find('#bonus-calc__target'),
    		$card = $el.find('#bonus-calc__card'),
    		$spendings = $el.find('.bonus-calc__points'),
    		$purchases = $el.find('.bonus-calc__rubles'),
    		$estimate = $el.find('#bonus-calc__estimate'),
    		$button = $el.find('button[type="submit"]');
    		$close = $el.find('.shared__modal-close-btn'),
    		spendings = null;


    	// --------------- События

    	$el.on('submit', function(e) {
    		e.preventDefault();
    	});

    	$close.on('click', function(){
    		$el.hide();
    	});

    	$button.on('click', function(){
    		var target = getNum($target),
    			card = $card.val();

    		spendings = !card || !target ? null : getSpendings(target, card);
    		displaySpendings();
    		updatePurchases();
    	});

    	$estimate.on('keyup change paste', function(){
    		var estimate = getNum($estimate);
    		$estimate.next('span').text(estimate ? Globalize.pluralize(estimate, 'месяц', 'месяца', 'месяцев') : "месяцев");
    	});


    	// --------------- Инициализация

		$.validator.unobtrusive.parse($el);

        $('.html-select').select2({
            placeholderOption: 'first',
            allowClear: true,
            minimumResultsForSearch: -1,
            formatResult: format,
            formatSelection: format,
            escapeMarkup: function(m){return m;}
        });

        function format (state) {
            if (!state.id) {
                return state.text;
            }

            return '<div class="select2__card-option"><i class="icon__card -is-' + state.id + '"></i> ' + state.text + '</div>';
        }

        // валидация для select2
        $.each($(".select2-container"), function (i, n) {
            $(n).next().show().fadeTo(0, 0).height("0px").css("left", "auto");
            $(n).prepend($(n).next());
            $(n).delay(500).queue(function () {
                $(this).removeClass("validate[required]");
                $(this).dequeue();
            });
        });


    	// --------------- Действия

    	function getNum ($input) {
    		var raw = $input.val();
    		if (!$.trim(raw)) {
    			return null;
    		}
    		return parseInt(raw.replace(/\D/g, ''), 10);
    	}

    	function getSpendings (target, card) {
    		var rate = parseInt($card.find('option[value="'+card+'"]').data('rate'), 10);
    		return target * rate;
    	}

    	function displaySpendings () {
    		if (spendings) {
    			$spendings.text(Globalize.pluralize(spendings, "{1:n0} рублей", "{2:n0} рублей"));
    		} else {
    			$spendings.text(" ");
    		}
    	}

    	function updatePurchases() {
    		var estimate = getNum($estimate);

    		if (!spendings || !estimate) {
    			$purchases.text(" ");
    			return;
    		}

			var purchases = Math.ceil(spendings/estimate),
				formated = Globalize.pluralize(purchases, '{1:n0} рубль', '{2:n0} рубля', '{5:n0} рублей');
			$purchases.text(formated);
    	}
    };
});