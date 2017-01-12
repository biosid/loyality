define('Content/PlainPages/index', [], function () {
    return function(options) {
        // --------------- Данные

        var navbar = $('[data-x="content/plainpages/navbar"]'),
            hideBuiltin = $('[data-x="content/plainpages/hide_builtin"]');

        // --------------- Инициализация

        navbar.sticky({ topSpacing: 0, center: false });

        // --------------- События

        hideBuiltin.on('change', function() {
            if (hideBuiltin.is(':checked'))
                window.location = options.hideBuiltinUrl;
            else
                window.location = options.showBuiltinUrl;
        });

        // --------------- Действия
    };
});
