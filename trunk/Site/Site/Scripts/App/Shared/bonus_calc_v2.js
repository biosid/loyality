define('Shared/bonus_calc_v2', function () {
    return function (options) {

    	// --------------- Данные

    	var $el = options.el,
    		$target = $el.find('#bonus-calc__target'),
    		$card = $el.find('#bonus-calc__card'),
    		$spendings = $el.find('.bonus-calc__points'),
    		$purchases = $el.find('#bonus-calc__purchases'),
    		$estimate = $el.find('#bonus-calc__estimate'),
    		$button = $el.find('button[type="submit"]');
    		$close = $el.find('.shared__modal-close-btn'),
    		spendings = null,
    		lastTouched = 'purchases';


    	// --------------- События

    	$close.on('click', function(){
    		$el.hide();
    	});

    	$target.add($card).on('keyup paste change', handleButton);

    	$button.on('click', function(){
    		var target = getNum($target),
    			card = $card.val();
    		if (!card || !target) {
    			displaySpendings(null);
    			return;
    		}
    		spendings = getSpendings(target, card);
    		displaySpendings();
    		updateDynamics();
    	});

    	$purchases.on('focus', function(){
    		lastTouched = 'purchases';
    		switchDependent()
    	});

    	$estimate.on('focus', function(){
    		lastTouched = 'estimate';
    		switchDependent();
    	});

    	$purchases.add($estimate).on('keyup change paste', updateDynamics);


    	// --------------- Инициализация

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

    	function handleButton () {
    		if (!$.trim($target.val()) || !$card.val()) {
    			$button.attr('disabled', 'disabled');
    		} else {
    			$button.removeAttr('disabled');
    		}
    	}

    	function updateDynamics() {
    		if (!spendings) {
    			return;
    		}

    		var purchases = getNum($purchases),
    			estimate = getNum($estimate),
    			formated;

    		if (lastTouched == 'purchases') {
    			estimate = purchases ? Math.ceil(spendings/purchases) : null;
    			formated = estimate ? Globalize.format(estimate, 'n0') : "";
    			$estimate.val(formated);
    		}

    		if (lastTouched == 'estimate') {
    			purchases = estimate ? Math.ceil(spendings/estimate) : null;
    			formated = purchases ? Globalize.format(purchases, 'n0') : "";
    			$purchases.val(formated);
    		}

    		$purchases.next('span').text(purchases ? Globalize.pluralize(purchases, 'рубль', 'рубля', 'рублей') : "рублей");
    		$estimate.next('span').text(estimate ? Globalize.pluralize(estimate, 'месяц', 'месяца', 'месяцев') : "месяцев");
    	}

    	function switchDependent () {
    		(lastTouched == 'purchases' ? $purchases : $estimate).removeClass('bonus-calc__dependent');
    		(lastTouched == 'purchases' ? $estimate : $purchases).addClass('bonus-calc__dependent');
    	}
    };
});