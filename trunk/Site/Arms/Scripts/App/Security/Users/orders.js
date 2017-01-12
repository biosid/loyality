define('Security/Users/orders', ['Shared/dtinterval'], function (dti) {
    return function() {

        // --------------- Данные

        var navbar = $('[data-x="security/users/orders/navbar"]'),
            list = $('[data-x="security/users/orders/list"]');

        // --------------- Инициализация

        navbar.sticky({ topSpacing: 0, center: false });

        dti.setupDtInterval('#from', '#to');

        // --------------- События

        // переход на заказ по щелчку на соответствующей сточке в таблице
        list.on('click', '[data-x="security/users/orders/row"]', function () {
            var href = $(this).find('a[href]').attr('href');
            window.location = href;
        });
        
        list.on('click', '[data-x="security/users/orders/order-item-row"]', function () {
            var href = $(this).prev('[data-x="security/users/orders/row"]').find('a[href]').attr('href');
            window.location = href;
        });

    };
});
