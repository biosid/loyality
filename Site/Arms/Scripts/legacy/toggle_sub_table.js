$('.table-category').on('click','.click',function(){
    $(this).closest('tbody').toggleClass('active-category');
});
$('.table-category tbody').addClass('active-category');