(function (exports) {
    
    if (!exports) {
        return;
    }

    /**
     *  Склонение по числам, пример: Globalize.decline(15, 'рубль', 'рубля', 'рублей')
     */
    exports.decline = function (num, singular, dual, plural) {
        if (!plural) plural = dual;
        var x = (num % 100);
        if (x > 4 && x < 21) {
            return plural;
        }
        else {
            switch (num % 10) {
                case 1:
                    return singular;
                case 2:
                case 3:
                case 4:
                    return dual;
                default:
                    return plural;
            }
        }
    };

    /**
     *  Склонение по числам, пример: Globalize.pluralize(15, '{1:n0} рубль', '{2:n0} рубля', '{5:n0} рублей')
     */
    exports.pluralize = function (num, sungular, dual, plural) {
        var template = Globalize.decline(num, sungular, dual, plural),
            parsed = template.match('{\\d(:(.+))?}');

        if (!parsed) {
            return template;
        }

        var placeholder = parsed[0],
            format = parsed[2],
            substitute = format == null ? num : Globalize.format(num, format);

        return template.replace(placeholder, substitute);
    };
})(window.Globalize);