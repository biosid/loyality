define('Shared/Header/search', function() {
    return function () {

        $('input[name="search-scope"]').on('change', updateSearch);

        $('[data-x="header/search-form-site"] form').on('submit', submitSearchSite);

        function updateSearch() {
            var $radio = $(this).closest('li').find('input:checked');
            
            if (!$radio.length) {
                return;
            }

            var $searchForms = $('[data-x^="header/search-form-"]'),
                searchFormSelector = '[data-x="' + $radio.data('target-search-form') + '"]',
                $searchForm = $(searchFormSelector);

            if (!$searchForm.length) {
                return;
            }

            $searchForms.hide();
            $searchForm.show();
        }
        
        function submitSearchSite(ev) {
            ev.preventDefault();

            var term = $.trim($('[data-x="header/search-site/term"]').val());

            if (term == '' || term == 'Поиск') {
                return false;
            }
            
            document.location = '/search?searchid=2210241&text=' + encodeURIComponent(term);

            return false;
        }
    };
});
