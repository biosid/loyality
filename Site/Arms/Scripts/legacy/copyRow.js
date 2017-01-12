// копирует строку с характеристиками в таблице спепцификаций товара
function copyRow(){    
    $('.add-ost', this.element).on('click',function(){
        var content = $(this).parents('tr').clone({withDataAndEvents:true});
		$(content).find('.clearable-input').text('').val('');
        $(this).after(content);
    });
}copyRow();
$('.remove-char', this.element).on('click', function(){
	var alltr = $(this).closest('tbody').find('tr');
	if (alltr.length == 1) { $(this).closest('tr').find('.clearable-input').text('').val('');  }
	else { $(this).closest('tr').remove(); }
});

/*картинки*/
var countImages = 0;
var maxImages = 22;
function imageFieldSet(button) {
	var self = $(button);
	var fieldset = $(button).closest('fieldset');
	
	var clone = $(fieldset).clone({withDataAndEvents:true});
	$(self).parent().hide();
	countImages += 1;
	if (countImages == maxImages) {
		$(clone).find('.add-image').remove();
	}
	$(clone).find('.input-image-url').val('');
	$(clone).find('.goodimage').remove();
	$(clone).find('.remove-image').parent().show();
	$(clone).find('.add-image').parent().hide();
	$(clone).removeClass('well');
	$(fieldset).after(clone).find('.add-image').parent().hide();
}
$('.add-image', this.element).on('click', function(){
	if (countImages < maxImages) imageFieldSet(this);
});
$('.input-image-url', this.element).on('keypress change', function(){
	var self = this;
	var imagecontainer = $(this).closest('fieldset').find('.imagecontainer');
	var fieldset = $(this).closest('fieldset');
	var legend = $(this).closest('fieldset').find('legend');
	var fieldtitle = $(this).closest('fieldset').find('.image-control-title');
	
	$(fieldset).removeClass('well');
	var imgurlinput = $(this);
	var imgurlvalue = $(imgurlinput).val();
	$(imagecontainer).html('<img/>');
	var image = $(imagecontainer).find('img');
	$(image).attr('src',imgurlvalue).addClass('goodimage');
	$(image).load( function(){ $(fieldset).addClass('well'); });
	if (imgurlvalue !='') { $(fieldset).find('.add-image').parent().show(); }
	else { $(fieldset).find('.add-image').parent().hide(); }
});
$('.image-control .clear').on('click', function(){
	$(this).prev().val('');
});
$('.remove-image', this.element).on('click', function(){
	var self = this;
	var fieldsets = $(this).closest('fieldset').parent().find('fieldset');
	var countt = $(fieldsets).length;
	console.log( countt );
	if (countt == 2) { $(this).closest('fieldset').prev().find('.add-image').parent().show(); }
	$(self).closest('fieldset').remove();
});