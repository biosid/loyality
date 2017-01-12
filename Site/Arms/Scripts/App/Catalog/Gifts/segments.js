define('Catalog/Gifts/segments', ['Catalog/Shared/Categories/helpers'], function (h) {
    return function(options) {

        // --------------- Данные

        var btnSegments = $('[data-x="gifts/edit_segments"]'),
            modal = $('[data-x="gifts/modal_segments"]'),
            form = modal,
            submitButton = form.find('button[type=submit]'),
            url = form[0].action;

        // --------------- События

        btnSegments.on('click', function () {
            showModal();
            return false;
        });

        form.on('submit', function() {
            submitButton.attr('disabled', 'disabled');

            var ids = options.selector.selected(),
                segments = getSegments();

            $.post(url, { Ids: ids, Segments: segments })
                .done(function() {
                    window.location.reload();
                })
                .fail(function(response) {
                    h.error('Ошибка при попытке привязки вознаграждений', response);
                });

            return false;
        });

        // --------------- Действия
        
        function showModal() {
            submitButton.removeAttr('disabled');
            form.find('input[type="checkbox"]').prop('checked', false);
            modal.modal('show');
        }
        
        function getSegments() {
            var segments = [];
            form.find('input[type="checkbox"]:checked').each(function() {
                segments.push($(this).data('segment-id'));
            });
            return segments;
        }

    };
});
