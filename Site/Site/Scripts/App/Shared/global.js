define('Shared/global', function(){
	var unreads,
		wished,
		basket,
		balance;

	var global = { 
		init: function(options){
			options = options || {};
			
			unreads = options.unreadsCount;
			wished = options.wishedCount;
			basket = options.basketCount;
			balance = options.balance;
		},

		unreads: function(count){
			if (count === undefined) {
				return unreads;
			}
			unreads = Math.max(count, 0);
			updateBadges('unreads', unreads > 99 ? '!' : unreads);
		},

		wished: function(count) {
			if (count === undefined) {
				return wished;
			} 
			wished = Math.max(count, 0);
		},

		basket: function(count) {
			if (count === undefined) {
				return basket;
			}
			total = Math.max(count, 0);
		},

		balance: function(amount) {
			if (amount === undefined) {
				return balance;
			}
			balance = amount;
		}
	};

	return global;

	function updateBadges(name, value) {
		var badges = $('[data-badge=' + name + ']');
		badges.text(value).css('visibility', value ? 'visible' : 'hidden');
	}
});