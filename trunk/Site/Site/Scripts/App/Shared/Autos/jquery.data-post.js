/**
 * Отправить ссылку как POST
 */
$('body').on('click', 'a[data-post]', function (e) {
    if (!this.href) {
        return;
    }
    
    var tokenSelector = 'input[name=__RequestVerificationToken]:eq(0)',
        includes = $( $(this).data('post') ),
        form = $('<form action="' + this.href + '" method="post"></form>'),
        token = $(tokenSelector);

    if (includes.length) {
        form.append(includes.clone());
    }

    var formHasToken = false;
    if (includes.filter(tokenSelector).length || includes.find(tokenSelector).length) {
        formHasToken = true;
    }

    if (token.length && !formHasToken) {
        form.append(token.clone());
    }
    
    form.appendTo('body').submit();
    
    e.preventDefault();
});