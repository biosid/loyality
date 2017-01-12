$('#btnDownloadFile').on('click',function(){
    var value = $(this).next('[type="file"]').click();
});