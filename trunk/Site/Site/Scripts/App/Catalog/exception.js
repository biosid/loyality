define('Catalog/exception', ['Shared/popup'], function (popup) {
    return function (text) {
        popup.open(text);
    };
});