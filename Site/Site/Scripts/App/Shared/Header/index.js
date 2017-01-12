define('Shared/Header/index', ['Shared/global'], function (global) {
    return function () {
        global.unreads(global.unreads());
        
        /*$('.search-block input').focus(function () {
            $(this).parents('.search-block').animate({ width: "100%" }, 500);
        });
        $('.search-block input').focusout(function () {
            $(this).parents('.search-block').animate({ width: "60%" }, 500);
        });*/

        // выпвдвющее меню
        $('.top-menu').on('click', '.top-menu__item-with-dropdown', function(){
        		var $item = $(this),
        			$ddn = $item.find('.small-menu');

        		if ($item.hasClass('opened')) {
        			return;
        		}

        		$ddn
        			.show()
        			.position({
	        			my:'center top',
	        			at: 'center bottom+2',
	        			of: $(this),
	        			collision: 'fit'
	        		})
	        		.on('clickoutside.popup', function(){
						if ($item.hasClass('opened')) {
							$ddn.slideToggle(300, function(){
								// fix css PIE для IE8
								$ddn[0].style.behavior = '';
							});
							$item.removeClass('opened');
							$ddn.off('.popup');
						}
    				})

					// fix css PIE для IE8
					$ddn[0].style.behavior = 'url(/Content/PIE.htc)';
	        		$item.find('[_pieId]').each(function(){
	        			var $el = $(this),
	        				pos = $el.position();
	        			
	        			$el
	        				.prev('css3-container')
	        				.css({
		        				left: pos.left + ( parseInt($el.css('margin-left')) || 0 ),
		        				top: pos.top + ( parseInt($el.css('margin-top')) || 0 ),
		        			});
	        		});

	        	window.setTimeout(function(){
	        		$item.addClass('opened');
	        	}, 0);
        });
	};
});