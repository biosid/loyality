define('MyMessages/index', ['Shared/global', 'Shared/yesno'], function (global, yesno) {
    return function(options) {
        var messagesTable = $(options.el),
            deleteForm = $('[data-x="message/delete_form"]'),
            threadIdInput = deleteForm.find('input[name="threadId"]'),
            setReadUrl = options.setReadUrl;

        messagesTable.on('click', '[data-x="thread"]', function() {

            var url = $(this).parent().data('url');
            window.location.assign(url);
        });

        messagesTable.on('click', '[data-x="notification"]', function () {

            var messageHeader = $(this);
            var message = messageHeader.parent();
            var messageBody = message.find('[data-x="message-body"]');
            
            var threadId = message.data('thread-id');

            if (messageHeader.hasClass('not-read')) {
                $.post(setReadUrl, { threadId: threadId });
                decrementUnreads();
            }

            toggleMessage(messageHeader, messageBody);
        });

        messagesTable.on('click', '[data-x="message/delete"]', function(e) {
            yesno.open('Удаление сообщения', 'Вы действительно хотите удалить это сообщение?',
                createDeleteHandler($(this)));

            e.stopPropagation();
            e.preventDefault();
        });
        
        function toggleMessage(messageHeader, messageBody) {
            messageHeader.removeClass('not-read').addClass('read');
            messageHeader.find('.move').toggleClass('moved');
            messageBody.toggleClass('none');
        }

        function decrementUnreads() {
            global.unreads(global.unreads()-1);
        }
        
        function createDeleteHandler($button) {
            return function(result) {
                if (result != 'yes') {
                    return;
                }

                var threadId = $button.data('thread-id');

                threadIdInput.val(threadId);

                deleteForm.submit();
            };
        }
    };
});