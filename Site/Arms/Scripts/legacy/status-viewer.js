$('#goodsPay, #goodsDelivery').on('change',function(){
    var selectVal = $(this).val(),
    container = $(this).closest('div');
    container.find('.label').hide();

    if(selectVal == "ok"){
        container.find('.label-success').show();
    }else{
        if(selectVal == "attention"){
            container.find('.label-important').show();
        }else{
            if(selectVal == "warning"){
            container.find('.label-warning').show();
            }
        }
    }
});

