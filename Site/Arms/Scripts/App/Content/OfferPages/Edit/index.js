define('Content/OfferPages/Edit/index', ['Content/OfferPages/Edit/codemirror'], function (cm) {
    return function () {
        // --------------- Данные

        var cmArea = $('[data-x="content/offerpages/edit/cm_area"]');

        // --------------- Инициализация

        cmArea.each(function () {
            cm.setupTextArea(this);
        });

        // --------------- Действия
    };
});
