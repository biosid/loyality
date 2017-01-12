define('Shared/video-button', function(){
	return function(options) {

		$(options.trigger).magnificPopup({
			disableOn: 700,
			type: 'iframe',
			tLoading: 'Загрузка...',
            tClose: 'Закрыть',
            closeOnContentClick: true,
			mainClass: 'mfp-fade',
			removalDelay: 300,
			preloader: false,
			fixedContentPos: false
		});
		
	}
});