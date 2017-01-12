(function($){

	$.fn.alphanumeric = function(p) { 

		p = $.extend({
			ichars: "!@#$%^&*()+=[]\\\';,/{}|\":<>?~`.-№_ ",
			nchars: "",
			allow: ""
		  }, p);	

		return this.each
			(
				function() 
				{

					if (p.nocaps) p.nchars += "ABCDEFGHIJKLMNOPQRSTUVWXYZАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЬЪЫЭЮЯ";
					if (p.allcaps) p.nchars += "abcdefghijklmnopqrstuvwxyzабвгдеёжзийклмнопрстуфхцчшщьъыэюя";
					
					s = p.allow.split('');
					for ( i=0;i<s.length;i++) if (p.ichars.indexOf(s[i]) != -1) s[i] = "\\" + s[i];
					p.allow = s.join('|');
					
					var reg = new RegExp(p.allow,'gi');
					var ch = p.ichars + p.nchars;
					ch = ch.replace(reg,'');

					$(this).bind('keypress.alphanumeric', function (e) {
						if (!e.charCode) k = String.fromCharCode(e.which);
							else k = String.fromCharCode(e.charCode);

						if (!e.ctrlKey && ch.indexOf(k) != -1) { // разрешаем copy&pase (ctrl+c, ctrl+v, ctrl+x и т.д.)
							e.preventDefault();
						}
					});

					/**
					* Обрабатываем после copy&paste
					*/
					var filter = new RegExp('[' + ch.replace(/\[|\]|\\|\^|-/g, function(m) { return '\\' + m; }) + ']', 'gi');
					var that = $(this);
					$(this).bind('paste.alphanumeric', function() {
						window.setTimeout(function() {
							that.val(that.val().replace(filter, ''));
						},0);
					});
									
				}
			);

	};

	$.fn.numeric = function(p) {
	
		var az = "abcdefghijklmnopqrstuvwxyzабвгдеёжзийклмнопрстуфхцчшщьъыэюя";
		az += az.toUpperCase();

		p = $.extend({
			nchars: az
		  }, p);	
		  	
		return this.each (function()
			{
				$(this).alphanumeric(p);
			}
		);
			
	};
	
	$.fn.alpha = function(p) {

		var nm = "1234567890";

		p = $.extend({
			nchars: nm
		  }, p);	

		return this.each (function()
			{
				$(this).alphanumeric(p);
			}
		);	
	};

})(jQuery);