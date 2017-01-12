// для кнопочки отсортировать по ....
$('#sortby').on('click',function(){
    $(this).toggleClass('up down');
    var title = $('#sortby').attr('class');
    if(title=="btn up"){
        $(this).attr('title','По возрастанию')
    }else{
        $(this).attr('title','По убыванию')
    }
});