define('Content/News/Edit/index', ['Shared/dtinterval'], function (dti) {
    return function () {
        // --------------- Данные

        var navbar = $('[data-x="news/edit/navbar"]'),
            form = $('[data-x="news/edit/form"]'),
            btnSave = $('[data-x="news/edit/save"]'),
            btnDelete = $('[data-x="news/edit/delete"]'),
            deleteUrl = btnDelete.attr('href'),
            delPicture = $('[data-x="news/edit/del_picture"]'),
            imgContainer = $('[data-x="news/edit/picture_container"]'),
            imgControl = $('[data-x="news/edit/picture_control"]'),
            imgUrl = $('[data-x="news/edit/pictureUrl"]');
        
        // --------------- Инициализация
        
        dti.setupDtInterval('[data-x="news/edit/dateFrom"]', '[data-x="news/edit/dateTo"]');

        navbar.sticky({ topSpacing: 0, center: false });
        $.datepicker.regional['ru'] = {
            closeText: 'Закрыть',
            prevText: '&#x3c;Пред',
            nextText: 'След&#x3e;',
            currentText: 'Сегодня',
            monthNames: ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь', 'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'],
            monthNamesShort: ['Января', 'Февраля', 'Марта', 'Апреля', 'Мая', 'Июня', 'Июля', 'Августа', 'Сентября', 'Октября', 'Ноября', 'Декабря'],
            dayNames: ['воскресенье', 'понедельник', 'вторник', 'среда', 'четверг', 'пятница', 'суббота'],
            dayNamesShort: ['вск', 'пнд', 'втр', 'срд', 'чтв', 'птн', 'сбт'],
            dayNamesMin: ['Вс', 'Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб'],
            dateFormat: 'd M yy',
            firstDay: 1,
            isRTL: false
        };

        delPicture.on('click', function() {
            imgContainer.remove();
            imgUrl.remove();
            imgControl.show();
        });

        btnSave.on('click', function() {
            form.submit();
        });

        btnDelete.on('click', function (e) {
            e.preventDefault();

            $.post(deleteUrl)
                .done(function(response) {
                    window.location = response.url;
                })
                .fail(function (response) {
                    alert('Ошибка при попытке удаления новости');
                    document.body.innerHTML = response.responseText;
                });
        });

        // --------------- Действия
    };
});
