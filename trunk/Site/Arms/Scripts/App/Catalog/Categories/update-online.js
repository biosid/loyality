define('Catalog/Categories/update-online', ['Catalog/Shared/Categories/helpers'], function (h) {
    return function(options) {

        // --------------- Данные


        var tree = $(options.tree),
            modal = $('[data-x="categories/online/modal"]'),
            form = modal,
            submitButton = form.find('button[type=submit]'),
            url = form.attr('action');


        // --------------- События


        // редактирование URL онлайн категории шаг 1. кнопка в дереве
        tree.on('click', '[data-x="categories/online"]', function(){
            var tr = $(this).closest('tr');

            showForm(tr);
            return false;
        });

        // редактирование URL онлайн категории шаг 2. сохранение
        form.on('submit', function(){
            if (form.valid()) {
                submitButton.attr('disabled', 'disabled');

                updateOnServer()
                    .done(updateInUi)
                    .fail(function(response){
                        h.error('Ошибка при редактировании online категории', response);
                    });
            }
            return false;
        });


        // --------------- Действия


        function showForm(tr){
            var meta = h.meta(tr),
                properties = getOnlineCategoryProperties(tr);

            form.reset();

            form.find('h4').text('Категория «' + h.title(tr) + '»');
            form.find('[name="Id"]').val(meta.id);
            form.find('[name="OnlineCategoryUrl"]').val(properties.onlineUrl);
            form.find('[name="NotifyOrderStatusUrl"]').val(properties.notifyUrl);
            form.find('[name="OnlineCategoryPartnerId"]').val(properties.partnerId);

            submitButton.removeAttr('disabled');
            modal.modal('show');
        }

        function updateOnServer(){
            return $.post(url, form.serialize());
        }

        function updateInUi(response){
            if (!response || response.success) {
                modal.modal('hide');

                var id = form.find('[name="Id"]').val(),
                    properties = {
                        onlineUrl: form.find('[name="OnlineCategoryUrl"]').val(),
                        notifyUrl: form.find('[name="NotifyOrderStatusUrl"]').val(),
                        partnerId: form.find('[name="OnlineCategoryPartnerId"]').val()
                    },
                    tr = h.node(id);
                    
                setOnlineCategoryProperties(tr, properties);
                modal.modal('hide');
                return;
            }

            submitButton.removeAttr('disabled');
            form.showErrors(response.errors);
        }

        // получить свойства онлайн категории
        function getOnlineCategoryProperties(tr) {
            var element = tr.find('[data-x="categories/online"]');
            return {
                onlineUrl: element.attr('href'),
                notifyUrl: element.data('category-notifyurl'),
                partnerId: element.data('category-partnerid')
            };
        }

        // установить свойства онлайн категории
        function setOnlineCategoryProperties(tr, properties) {
            var element = tr.find('[data-x="categories/online"]');
            element.attr('href', properties.onlineUrl);
            element.data('category-notifyurl', properties.notifyUrl);
            element.data('category-partnerid', properties.partnerId);
        }
    };
});