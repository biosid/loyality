define('MyPoints/index', function () {
    
    return function() {
        function toggleDateRangeButton() {
            if ($('#from').val() === "" ||
                $('#to').val() === "") {
                $('.date-picker input[type="submit"]').attr('disabled', 'disabled');
            } else {
                $('.date-picker input[type="submit"]').removeAttr('disabled');
            }
        }

        $("#from").datepicker({
            dateFormat: "dd.mm.yy",
            changeMonth: false,
            numberOfMonths: 1,
            maxDate: $('#to').val() === "" ?
                        new Date() : $('#to').val(),
            onClose: function (selectedDate) {
                $("#to").datepicker("option", "minDate", selectedDate);
                toggleDateRangeButton();
            }
        });

        $("#to").datepicker({
            dateFormat: "dd.mm.yy",
            changeMonth: false,
            numberOfMonths: 1,
            minDate: $('#from').val() === "" ?
                        null : $('#from').val(),
            maxDate: new Date(),
            onClose: function (selectedDate) {
                $("#from").datepicker("option", "maxDate", selectedDate);
                toggleDateRangeButton();
            }
        });
    };

});

