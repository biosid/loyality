// подстановка имени и логина пользователя во всплывающие окна
// источник, классы "loginUser","nameUser"
// место приземления, классы "userName","userLogin"
function user(){
    var userLogin = $('.loginUser').text();
    var userName = $('.nameUser').text();
    $('.userName').text(userName);
    $('.userLogin').text(userLogin);
}user();