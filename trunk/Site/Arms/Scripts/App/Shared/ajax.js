define("Catalog/Shared/ajax", function () {
    return {
        serverError: function (message) {
            return function(response) {
                alert(message);
                document.body.innerHTML = response.responseText;
                // TODO: показать сообщение об ошибке и перезагрузить
            };
        },
        withValidation: function (success, error) {
            return function (response) {
                
            };
        }
    };
})