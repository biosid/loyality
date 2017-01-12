define('Shared/post-command', function () {
    return function (options) {
        
        if (window.parent && window.parent.postMessage) {

            var data = 'type=postcommand';
            data += '&command=' + encodeURIComponent(options.command);
            data += '&query=' + encodeURIComponent(options.query);

            window.parent.postMessage(data, '*');
        }
    };
});
