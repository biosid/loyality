define('Actions/index', ['Shared/dtinterval'], function (dti) {
    return function () {

        // --------------- Данные

        var form = $('[data-x="actions/filter_form"]');

        // --------------- Инициализация

        dti.setupDtInterval('#from', '#to');

        // --------------- События

        $(document).on('keydown', function(e) {
            if (e.which == 13)
                form.submit();
        });
    };
});
