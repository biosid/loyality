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
$.datepicker.setDefaults($.datepicker.regional['ru']);

$("#from").datepicker({
    defaultDate: "+1w",
    onClose: function (selectedDate) {
        $("#to").datepicker("option", "minDate", selectedDate);
    }
});
$("#to").datepicker({
    defaultDate: "+1w",
    onClose: function (selectedDate) {
        $("#from").datepicker("option", "maxDate", selectedDate);
    }
});

$('.sticky-save, .sticky-filter').sticky({ topSpacing: 0, center: false });

$(function () {
    var allCategories = [
        "Акции!",
        "Акция \"школьные каникулы\"",
        "АКЦИЯ DJ Hero скидки 70%",
        "АКЦИЯ Guitar Hero скидки 50%",
        "Выгодные комплекты!",
        "Скоро в продаже",
        "Игровые приставки",
        "Comfy",
        "Dendy (8 bit)",
        "DVTech",
        "Emote",
        "EXEQ",
        "Func",
        "Game Boy Advance",
        "MegaDrive",
        "Microsoft Xbox 360",
        "Nintendo 3DS",
        "Nintendo Wii",
        "PGP AIO",
        "SEGA (16 bit)",
        "Sony PlayStation 2",
        "Sony PlayStation 3",
        "Sony Playstation Portable (PSP)",
        "Sony PlayStation Vita",
        "Магистр",
        "Компьютерные игры",
        "3DS",
        "MMORPG",
        "MS Kinect",
        "PS Move",
        "PSP Essentials",
        "Аркады (Arcade. Platformer)",
        "Боевик (Shooter)",
        "Гонки (Racing)",
        "Действие (Action)",
        "Драки (Fighting)",
        "Квесты (Quest)",
        "Киноигры (Movies & TV)",
        "Логические (Logic & Puzzle)",
        "Мобильные (Mobile games)",
        "Музыкальные (Music & Party)",
        "Приключения (Adventure)",
        "Развивающие (Educational)",
        "Ролевые (RPG)",
        "Семейные. Детские (Family)",
        "Спортивные (Sports)",
        "Стратегии (Strategy)",
        "Ужасы (Horror)",
        "Обучающие программы",
        "Для бизнеса",
        "Для детей",
        "Для школы и института",
        "Иностранные языки",
        "Компьютер и интернет",
        "ЕГЭ. Экзамены",
        "Энциклопедии и справочники",
        "Софт для дома и бизнеса",
        "PRO Аудио программы",
        "Антивирусы",
        "Обработка аудио",
        "Обработка видео",
        "Обработка фото",
        "Операционные системы",
        "Офисные программы",
        "Программы для мобильных устройств",
        "Словари и переводчики",
        "Утилиты и средства разработки",
        "Акции",
        "Новинки"
    ];

    $("#categories").autocomplete({
        source: allCategories,
        appendTo: '#categoryautocompleteresults',
        messages: {
            noResults: '',
            results: function () { }
        }
    });
});
