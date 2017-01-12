define('Catalog/Gifts/move', ['Catalog/Shared/Categories/helpers', 'Catalog/Shared/Categories/navigation'], function (h, nav) {
    return function (options) {

        // --------------- Данные


        var btnMove = $('[data-x="gifts/move/trigger"]'),
            modal = $('[data-x="gifts/move/modal"]'),
            categories = $('[data-x="gifts/move/categories"]'),
            navigator,
            form = modal,
            submitButton = form.find('button[type=submit]'),
            url = form[0].action;

        // --------------- Инициализация

        navigator = nav.categoryNavigation(categories, {});

        // --------------- События

        btnMove.on('click', function (e) {
            e.preventDefault();
            showModal();
        });

        categories.on('selectedChanged', function (e, selection) {
            if (selection)
                submitButton.removeAttr('disabled');
            else
                submitButton.attr('disabled', 'disabled');
        });

        form.on('submit', function () {
            submitButton.attr('disabled', 'disabled');
            navigator.enable(false);

            if (navigator.getSelection() != null) {
                var ids = options.selector.selected();

                moveOnServer(navigator.getSelection(), ids)
                    .done(moveInUi)
                    .fail(function (response) {
                        h.error('Ошибка при попытке переноса вознаграждений', response);
                    });
            }
            
            return false;
        });


        // --------------- Действия


        function showModal() {
            navigator.resetSelection();
            navigator.enable(true);
            modal.modal('show');
        }

        function moveOnServer(category, ids) {
            return $.post(url, {CategoryId:category, Ids:ids});
        }

        function moveInUi(){
            window.location.reload();
        }

    };
});
