define('Shared/process-command', ['Shared/query-parser'], function (parser) {
    return function (options) {

        $(window).on('message', handleMessage);

        function handleMessage(e) {
            if (!e.originalEvent.data) {
                return;
            }

            var data = parser(e.originalEvent.data);

            if (data.type == 'postcommand') {
                handlePostedCommand(data.command, data.query);
            }
        }

        function handlePostedCommand(command, query) {
            if (command == 'Refresh') {
                refresh();
                return;
            }

            if (command == 'ShowCardRegistrationError') {
                showCardRegistrationError();
                return;
            }
            
            if (command == 'ShowBuyFailed') {
                showBuyFailed();
                return;
            }

            if (command == 'ShowBuyConfirm') {
                showBuyConfirm(query);
                return;
            }

            if (command == 'ShowActivationRequired') {
                showActivationRequired();
                return;
            }

            if (command == 'ReloadCardRegisterFrame' && options.cardRegisterUrl) {
                reloadFrame(options.cardRegisterUrl, query);
                return;
            }

            if (command == 'ReloadCardSuccessFrame' && options.cardSuccessUrl) {
                reloadFrame(options.cardSuccessUrl, query);
                return;
            }
            
            if (command == 'ReloadOnlineOrderFrame' && options.onlineOrderUrl) {
                reloadFrame(options.onlineOrderUrl, query);
                return;
            }
            
            if (command == 'ReloadPayAdvanceFrame' && options.payAdvanceUrl) {
                reloadFrame(options.payAdvanceUrl, query);
                return;
            }
        }
        
        function refresh() {
            window.location.reload();
        }

        function showCardRegistrationError() {
            if (options.cardRegistrationErrorUrl) {
                window.location.assign(options.cardRegistrationErrorUrl);
            }
        }
        
        function showBuyFailed() {
            if (options.buyFailedUrl) {
                window.location.assign(options.buyFailedUrl);
            }
        }

        function showBuyConfirm(query) {
            if (options.buyConfirmUrl) {
                window.location.assign(options.buyConfirmUrl + query);
            }
        }

        function showActivationRequired() {
            if (options.activationRequiredUrl) {
                window.location.assign(options.activationRequiredUrl);
            }
        }

        function reloadFrame(url, query) {
            if (options.iframe) {
                $(options.iframe).attr('src', url + query);
            }
        }
    };
});
