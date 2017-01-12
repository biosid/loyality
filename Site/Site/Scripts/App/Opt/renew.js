define('Otp/renew', function () {
    return function (options) {

        // --------------- Данные


        var form = $(options.el),
            renewBlock = form.find('[data-x="otp/renew"]'),
            renewTrigger = renewBlock.find('a'),
            renewUrl = renewTrigger[0].href,
            tokenField = form.find('[name=OtpToken]');


        // --------------- Инициализация

        renewCountdown(options.expires || 0);


        // --------------- События

        // обновление токена
        renewTrigger.on('click', function (e) {
            e.preventDefault();
            renewOtp();
        });

        form.on('submit', function () {
            onSizeChange();
        });

        // --------------- Действия


        function renewOtp() {
            $.post(renewUrl)
                .success(function (response) {
                    if (!response.Error) {
                        tokenField.val(response.Token);
                        renewCountdown(response.ExpiresInSeconds || 0);
                    } else {
                        form.showErrors({ "Otp": response.Error });
                    }
                });
            renewBlock.hide();
            onSizeChange();
        }

        function renewCountdown(seconds) {
            window.setTimeout(function () {
                renewBlock.show();
                onSizeChange();
            }, seconds * 1000);
        }

        function onSizeChange() {
            options.onsizechange && options.onsizechange();
        }
    };
});