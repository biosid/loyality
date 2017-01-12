define('Security/Users/messages', [], function() {
    return function () {

        // --------------- Данные

        var navbar = $('[data-x="security/users/messgaes/navbar"]');

        // --------------- Инициализация

        navbar.sticky({ topSpacing: 0, center: false });

        // --------------- События

        $('[data-x="security/messages/expand"]').on('click', function (e) {
            toggleMessageText($(this).closest('tr'));

            e.preventDefault();
        });

        // --------------- Действия

        function toggleMessageText(headerRow) {
            var chevron = headerRow.find('[data-x="security/messages/chevron"]'),
                chevronCell = chevron.closest('td'),
                bodyRow = headerRow.next('tr');

            if (chevron.hasClass('icon-chevron-right')) {
                chevron.removeClass('icon-chevron-right').addClass('icon-chevron-down');

                bodyRow.show();
                chevronCell.attr('rowspan', 2);
            } else {
                chevron.removeClass('icon-chevron-down').addClass('icon-chevron-right');

                bodyRow.hide();
                chevronCell.attr('rowspan', 1);
            }

        }
    };
});
