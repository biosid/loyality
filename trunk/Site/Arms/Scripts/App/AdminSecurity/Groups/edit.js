define('AdminSecurity/Groups/edit', [], function () {
    return function () {

        // --------------- Данные

        var navbar = $('[data-x="adminsecurity/groups/edit/navbar"]');

        // --------------- Инициализация

        navbar.sticky({ topSpacing: 0, center: false });

        $('[data-x="adminsecurity/groups/edit/users"]').select2({ closeOnSelect: false });

        // --------------- События

        // --------------- Действия
        
    };
});
