define('Shared/captcha', function () {
	return function(options){

		$(options.refreshButton).on('click', function(){
			$(options.image).attr('src', $(options.image).attr('src') + '&rnd=' + new Date().getMilliseconds());
		});
	}
});