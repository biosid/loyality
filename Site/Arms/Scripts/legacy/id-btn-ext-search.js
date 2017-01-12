// показывает дополнительные характеристики используется просто слайд ап даун с проверкой
$('#btn-ext-search').on('click',function(){
    var wasSlide = $('#block-ext-search').attr('style');
    if(wasSlide == "display: none;"){
        $('#block-ext-search').slideDown();
    }else{
        $('#block-ext-search').slideUp();
    }
});