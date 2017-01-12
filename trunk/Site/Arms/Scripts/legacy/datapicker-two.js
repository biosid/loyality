// так как бываю случаи когда на странице бывает более двух полей с датапикером
// пришлось сдублировать id
$( "#from-data" ).datepicker({
    defaultDate: "+1w",
    onClose: function( selectedDate ) {
        $( "#to" ).datepicker( "option", "minDate", selectedDate );
    }
});
$( "#to-data" ).datepicker({
    defaultDate: "+1w",
    onClose: function( selectedDate ) {
        $( "#from" ).datepicker( "option", "maxDate", selectedDate );
    }
});