define('Security/CustomFields/index', [], function() {
    return function() {

        // --------------- Данные

        var navbar = $('[data-x="security/customfields/navbar"]');

        // --------------- Инициализация

        navbar.sticky({ topSpacing: 0, center: false });

    };
});
