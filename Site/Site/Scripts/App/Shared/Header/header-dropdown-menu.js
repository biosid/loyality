define('Shared/Header/header-dropdown-menu', function () {
    return function() {
        $('header nav').on({
            mouseenter: function () {
                $(this).find('.sub-nav').attr('style', '').addClass('active').stop().slideDown();
            },
            mouseleave: function () {
                $(this).find('.sub-nav').removeClass('active').stop().slideUp();
            }
        }, '.nav .item');

        $('header nav').on({
            mouseenter: function () {
                $(this).addClass('active').siblings().removeClass('active');
            },
            mouseleave: function () {
                $(this).removeClass('active');
            }
        }, '.sub-nav-ul > li');
    };
});