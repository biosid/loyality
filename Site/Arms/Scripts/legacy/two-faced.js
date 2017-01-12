$('.two-faced').on('focusin',function(){
    var InputWidth = $(this).outerWidth();
    $(this).attr('type','text').removeClass('btn btn-danger');
    $(this).outerWidth(InputWidth);
});
$('.two-faced').on('focusout',function(){
    var InputWidth = $(this).outerWidth();
    $(this).attr('type','button').addClass('btn').removeAttr('style');
});