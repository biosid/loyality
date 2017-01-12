define('Security/Users/points', ['Shared/dtinterval'], function (dti) {
    return function () {

        // --------------- Данные

        var navbar = $('[data-x="security/users/points/navbar"]');

        // --------------- Инициализация

        navbar.sticky({ topSpacing: 0, center: false });

        dti.setupDtInterval('#from', '#to');

        // --------------- События

    };
});
