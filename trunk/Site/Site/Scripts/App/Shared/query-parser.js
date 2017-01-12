define('Shared/query-parser', function(){

    /**
     * Парсер query string
     */
    return function parseQueryString(query)
    {
        if (typeof(query) != 'string') {
            return {};
        }

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
    };

});
    