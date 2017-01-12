// скрипт который работает с чекбоксами в таблице
function checkboxInTable() {
    var tableCat = $('.table-category');

    tableCat.on('click',':checkbox',function(){
        var atrValue = $(this).prop('checked'),
            row = $(this).closest('tr');

        if (atrValue) {
            row.addClass('checked');
        } else {
            row.removeClass('checked');
        }
    });
    tableCat.on('click','.category :checkbox',function(){
        var atrValue = $(this).prop('checked'),
            container = $(this).closest('tbody'),
            cboxes = container.find(':checkbox'),
            rows = container.find('tr');

        if (atrValue) {
            cboxes.prop('checked',true);
            rows.addClass('checked');
        } else {
            cboxes.prop('checked',false);
            rows.removeClass('checked');
        }
    });
    tableCat.on('click','.subcategory :checkbox',function(){
        var atrValue = $(this).prop('checked'),
            container = $(this).closest('tbody'),
            cat = container.find('.category'),
            catChecked = container.find('.category.checked'),
            catCheckbox = container.find('.category :checkbox');

        if (atrValue) {
            cat.addClass('checked');
            catCheckbox.prop('checked',true);
        } else {
            var hasChecked = container.find('.subcategory.checked :checkbox').prop('checked');
            if(!hasChecked){
                catCheckbox.prop('checked',false);
                catChecked.removeClass('checked');
            }
        }
    });
    tableCat.on('click','thead th :checkbox',function(){
        var atrValue = $(this).prop('checked'),
            checkbox = $('.table-category tbody :checkbox'),
            row = $('.table-category tbody tr');
        if (atrValue) {
            checkbox.prop('checked',true);
            row.addClass('checked');
        } else {
            checkbox.prop('checked',false);
            row.removeClass('checked');
        }
    });
}checkboxInTable();