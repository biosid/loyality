/**
 * Добавить значение контрола в query string страницы
 */
(function($) {
    /**
     * Фильтр
     */
     $('body').on('change', 'select[data-filter]', function (e) {
        var data = $(this).data('filter') || { },
            options = $.extend(
                { 
                    name: this.name
                }, 
                data
            );

        var search = window.location.search.substring(1),
            params = parseQueryString(search);

        params[options.name] = $(this).val();
        delete params.page;
        window.location.search = $.param(params);

        e.preventDefault();
    });

    /**
     * Парсер query string
     */
    function parseQueryString(query)
    {
        query = query.replace(/\+/g, ' ');
        var params = {},
            rx = /([^&=]+)=?([^&]*)/g,
            match, 
            name, 
            value;

        while (match = rx.exec(query)) {
            name = decodeURIComponent((match[1]));
            value = decodeURIComponent((match[2]));
            params[name] = value;
        }

        return params;
    }
})(jQuery);