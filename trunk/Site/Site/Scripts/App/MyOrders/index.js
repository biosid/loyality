define('MyOrders/index', function () {
    return function (options) {
        var ordersTable = $(options.el);

        ordersTable.on('click', '[data-href]', function (e) {
            if ($(e.target).closest('a').length == 0) {

                window.location.href = $(this).data('href');
            }
        });
    };
});