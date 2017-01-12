// предназначен для левой вертикальной менюшки и ее подкатегорий
function catOpenClose(){
    var subCat = $('#category .sub'),
        btn =  $('#category .btn.btn-mini');
    $('.catOpen').on('click',function(){
        subCat.addClass('open');
        btn.addClass('clicked');
    });
    $('.catClose').on('click',function(){
        subCat.removeClass('open');
        btn.removeClass('clicked');
    });
}catOpenClose();