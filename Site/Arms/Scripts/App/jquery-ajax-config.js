$.ajaxSetup({
    traditional: true,
    data: {
        __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
    }
});
