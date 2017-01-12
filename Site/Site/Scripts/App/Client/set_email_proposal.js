define('Client/set_email_proposal', ['Shared/popup'], function (popup) {
    return function (options) {

        setTimeout(function() {
            popup.open('<p>Уважаемый клиент! Пожалуйста, укажите Ваш адрес электронной почты в разделе «<a href="' + options.myinfoUrl + '">Мои данные</a>», это позволит нам оперативно отправлять Вам важную информацию. Спасибо!</p>');
        }, 100);
    };
});
