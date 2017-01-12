define('AdminSecurity/Users/edit', [], function () {
    return function () {

        // --------------- Данные

        var navbar = $('[data-x="adminsecurity/users/edit/navbar"]');

        // --------------- Инициализация

        navbar.sticky({ topSpacing: 0, center: false });

        $('[data-x="adminsecurity/users/edit/groups"]').select2({ closeOnSelect: false });

        // --------------- События

        // --------------- Действия

    };
});
