// стандартное описание для датапикера с двумя полями где указывает дату от и до.
$.datepicker.regional['ru'] = { 
closeText: 'Закрыть', 
prevText: '&#x3c;Пред', 
nextText: 'След&#x3e;', 
currentText: 'Сегодня', 
monthNames: ['Январь','Февраль','Март','Апрель','Май','Июнь', 'Июль','Август','Сентябрь','Октябрь','Ноябрь','Декабрь'], 
monthNamesShort: ['Января','Февраля','Марта','Апреля','Мая','Июня', 'Июля','Августа','Сентября','Октября','Ноября','Декабря'], 
dayNames: ['воскресенье','понедельник','вторник','среда','четверг','пятница','суббота'], 
dayNamesShort: ['вск','пнд','втр','срд','чтв','птн','сбт'], 
dayNamesMin: ['Вс','Пн','Вт','Ср','Чт','Пт','Сб'], 
dateFormat: 'dd.mm.yy', 
firstDay: 1, 
isRTL: false 
}; 
$.datepicker.setDefaults($.datepicker.regional['ru']);

$( "#from" ).datepicker({
    defaultDate: "+1w",	
	dateFormat: "d M, yy",
    onClose: function( selectedDate ) {
        $( "#to" ).datepicker( "option", "minDate", selectedDate );
    }
});
$( "#to" ).datepicker({
    defaultDate: "+1w",
	dateFormat: "d M, yy",
    onClose: function( selectedDate ) {
        $( "#from" ).datepicker( "option", "maxDate", selectedDate );
    }
});