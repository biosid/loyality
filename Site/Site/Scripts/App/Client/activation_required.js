define('Client/activation_required', ['Shared/popup'], function (popup) {
    return function (options) {

        // защищаемся от повторной обработки клика,
        // так как данный скрипт может быть вызван не один раз
        $('[data-x="client/activation_required"]').each(function() {
            var el = $(this);
            if (!el.data('activation_required_handled')) {
                el.data('activation_required_handled', true);
                el.on('click', activationRequiredHandler);
            }
        });

        function activationRequiredHandler(e) {
            e.preventDefault();
            popup.open('<p>Для того чтобы заказывать вознаграждения, нужно пройти <a href="' + options.hiwUrl + '">активацию</a>. Вы можете добавить вознаграждение в «Мои желания».</p>');
        }
    };
});
