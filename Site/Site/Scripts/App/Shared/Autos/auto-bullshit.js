$('input.spinner').spinner({ min: 1, max: 9 });
$('input.spinner').numeric();

// left menu
$('.left-menu .sub .icon').click(function () {
    $(this).siblings('.sub-left-menu').slideToggle();
    $(this).parent().toggleClass('active');
});