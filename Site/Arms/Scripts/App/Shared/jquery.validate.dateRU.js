(function($) {

    $.validator.addMethod('dateru', dateru, 'Неверный формат даты');
    $.validator.unobtrusive.adapters.addBool('dateru');
    
    function dateru(value, element) {
        return this.optional(element) ||
            checkForMask(value, element) ||
            tryParseDateRu(value);
    }
    
    function checkForMask(value, element) {
        console.info('mask: ' + $(element).mask());
        return false;
    }
    
    function tryParseDateRu(value) {
        if (value == '__.__.____')
            return true;
        try {
            $.datepicker.parseDate('dd.mm.yy', value);
            return true;
        } catch(e) {
            return false;
        }
    }
    
})(jQuery);
