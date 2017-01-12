// для кнопки отметить все , снять выделение,
// для списка списка чекбоксов
function checkUncheck(){
    var selector, allcheckbox;
    $('.checked').on('click',function(){
        selector = $(this).data('rel');
        allcheckbox = $(selector).find('input:checkbox');
        allcheckbox.prop('checked',true);
    });
    $('.unchecked').on('click',function(){
        selector = $(this).data('rel');
        allcheckbox = $(selector).find('input:checkbox');
        allcheckbox.prop('checked',false);
    });
}checkUncheck();
