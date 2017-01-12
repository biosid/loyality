(function(){
	
	// ----------------- API

	if (typeof Vtb24Collection == 'undefined') { Vtb24Collection = {}; }

	Vtb24Collection.OnlineCategory = {
		syncFrameSize : syncSize,
		setFrameHeight: setHeight,
		scrollTo: scroll
	};



	// ----------------- Implementation

	function syncSize() {
        var height = document.body.offsetHeight;
        setHeight(height);
	}

	function setHeight(height) {
		post('height', 'v='+height);
	}

	function scroll(where) {
		post('scroll', 'v='+where);
	}

	function post(type, params) {
		if (window.parent && window.parent.postMessage) {
			var message = 'from=onlinecategory&t=' + type;
			if (params) {
				message += '&' + params;
			}
			window.parent.postMessage(message, '*');
		}
	}


})();