$.ajaxSetup({
    data: {
        __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
    },
});