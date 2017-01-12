(function($){
	var INVALID_FIELD_CLASS = 'input-validation-error',
        VALID_MESSAGE_CLASS = 'field-validation-valid',
        INVALID_MESSAGE_CLASS = 'field-validation-error';

    /**
    * Показать ошибки, переданные через AJAX.
    */
    $.fn.showErrors = function (errors, callback) {
	    if (!errors) {
		    errors = {'': 'Произошла ошибка'};
	    }

		var self = $(this),
			validator = self.data('validator');

		window.setTimeout(function(){
			var formErrors = [];

			// удаляем из ответа сообщение, не привязанное к полям формы;
			// мы обработаем его отдельно
			if (errors['']){
				formErrors.push(errors['']);
				delete errors[''];
			} 
			
			// обрабатываем ошибки скрытых полей и кастомных виджетов
			for (var name in errors) {
				var input = self.find('[name="' + name + '"]');

				// неизвестное поле -> показываем как огибку формы
				if (!input.length) {
					formErrors.push(errors[name]);
					delete errors[name];
					continue;
				}

				// скрытое поле -> либо ошибка в кастомном виджете, либо отобразим как ошибку формы
				if (input.is(':hidden')) {
					var container = input.data('val-element'); // кастомные виджеты могут указать класс контейнера
					if (container) { 
						// ошибка виджета -> присвоим класс контейнеру
						input.closest(container).addClass(INVALID_FIELD_CLASS);
					} else { 
						// ошибка формы
						formErrors.push(errors[name]);
					}
					continue;
				}
			}

			// "регистрируем" ошибки в валидаторе (небольшой хак)
			if (validator) {
				for (var key in errors) {
					validator.submitted[key] = errors[key];
				}
				// показываем ошибки стандартным способом
				validator.showErrors(errors);
			}

			// обработка сообщений, не привязанных к полям
			var formErrContainer = self.find('span[data-valmsg-for=]');
			if (formErrors.length) {
				formErrContainer
					.attr('class', INVALID_MESSAGE_CLASS)
					.html($.fn.showErrors.formErrorsFormatter(formErrors))
					.css('display', '');
			} else {
				formErrContainer
					.attr('class', VALID_MESSAGE_CLASS)
					.hide();
			}

			if (callback) {
				callback();
			}

		}, 200);
    };
    $.fn.showErrors.formErrorsFormatter = function(messages){
    	return messages.join('<br/>');
    };

    /**
	 *  Сбросить форму и спрятать ошибки
	 */
	$.fn.reset = function () {
	    $(this).filter('form').each(function(){
	    	var form = $(this),
	    		validator = form.data('validator');

	    	form[0].reset();

		    if (!validator) {
		    	return;
		    }
		    	
		    validator.resetForm();
		    validator.submit = {};
		    $('input, textarea, select', form)
		    	.trigger('validatorsuccess');

		    // финальная очистка
		    form.find('.' + INVALID_MESSAGE_CLASS + ',.' + VALID_MESSAGE_CLASS)
		    	.html('')
		    	.attr('class', VALID_MESSAGE_CLASS);
		    form.find('.' + INVALID_FIELD_CLASS)
		    	.removeClass(INVALID_FIELD_CLASS);
	    });
	};

})(jQuery);