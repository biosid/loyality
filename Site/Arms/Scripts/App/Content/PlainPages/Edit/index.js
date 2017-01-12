define('Content/PlainPages/Edit/index', ['Content/PlainPages/Edit/codemirror'], function (cm) {
    return function () {
        // --------------- Данные

        var cmArea = $('[data-x="content/plainpages/edit/cm_area"]');

        // --------------- Инициализация

        cmArea.each(function() {
            cm.setupTextArea(this);
        });

        // --------------- Действия
    };
});
