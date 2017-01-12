define('Main/banner_carousel', function () {
    return function(options) {
        var $carousel = $(options.selector),
            $pages = $carousel.find('.banner-carousel__content').first(),
            pages = $pages.children('.banner-carousel__item').toArray(),
            $pager = $carousel.find('.banner-carousel__pager'),
            $pageButtons,
            $pageButtonTemplate = $carousel.find('.banner-carousel__page'),
            $prevButton = $carousel.find('.banner-carousel__prev'),
            $nextButton = $carousel.find('.banner-carousel__next'),
            pagesCount = pages.length,
            activePageIndex = -1,
            controlsLock = false,
            autoswitchTimer = null;
        
        // скрываем управление, если страниц меньше 2
        if (pagesCount < 2) {
            $pager.hide();
            $prevButton.hide();
            $nextButton.hide();
            return;
        }

        // иначе инициализируем карусель
        initialize();
        
        //----------------------------------------
        // инициализация
        
        function initialize() {
            // создаем кнопки перехода по страницам
            $pageButtons = $(
                $.map(pages, function(page, pageIndex) {
                    var $page = $(page);

                    // активна должна быть только одна страница
                    if ($page.hasClass('active')) {
                        if (activePageIndex == -1) {
                            activePageIndex = pageIndex;
                        } else {
                            $page.removeClass('active');
                        }
                    }

                    return $pageButtonTemplate
                        .clone()
                        .data('page-index', pageIndex)
                        .insertBefore($pageButtonTemplate)
                        .show()[0];
                }));

            // активируем страницу, если ни одна не была активна
            if (activePageIndex == -1) {
                activePageIndex = 0;
                $(pages[activePageIndex]).addClass('active');
            }

            // активируем кнопку страницы
            $($pageButtons[activePageIndex]).addClass('active');

            // привязываем переход на страницу по кнопке
            $pageButtons.on('click', showPage);

            // привязываем перемотку страниц назад/вперед
            $prevButton.on('click', prevPage);
            $nextButton.on('click', nextPage);

            // автоперелистывание
            autoSwitchOn();
            // сбрасываем таймер по активности пользователя
            $pages.on('mousedown', function(){
                autoSwitchOff();
                autoSwitchOn();
            });
        }
        
        //----------------------------------------
        // обработчики событий

        function showPage() {
            var pageIndex = $(this).data('page-index');
            
            if (activePageIndex < pageIndex) {
                switchPage(pageIndex, 1);
            } else if (activePageIndex > pageIndex) {
                switchPage(pageIndex, -1);
            }
        }

        function prevPage() {
            switchPage((activePageIndex + pagesCount - 1) % pagesCount, -1);
        }

        function nextPage() {
            switchPage((activePageIndex + 1) % pagesCount, 1);
        }

        //----------------------------------------
        // перемотка страниц
        
        function switchPage(index, dir, speed) {
            if (controlsLock) {
                return;
            }

            autoSwitchOff();
            controlsLock = true;

            $carousel.trigger('carousel-change');

            var $newPage = $(pages[index]),
                $oldPage = $(pages[activePageIndex]);

            // новая страница поверх остальных
            $pages.children().last().after($newPage);

            // опеределяем параметры перемотки
            var left1, left2;
            if (dir > 0) {
                left1 = '100%';
                left2 = '-=100%';
            } else {
                left1 = '-100%';
                left2 = '+=100%';
            }

            $oldPage.trigger('carousel-hide');

            // перемотка старой страницы
            $oldPage.css('left', '0')
                .animate({ left: left2 }, speed || 500);

            // перемотка новой страницы
            $newPage.css('left', left1)
                .show()
                .animate({ left: left2 }, speed || 500, complete);
            
            function complete() {
                // деактивируем старую
                $oldPage.removeClass('active').hide().stop();
                
                // активируем новую
                $newPage.addClass('active');

                // активируем/деактивируем кнопки страниц
                $pageButtons.removeClass('active');
                $($pageButtons[index]).addClass('active');

                // сохраняем новый индекс активной страницы
                activePageIndex = index;

                $newPage.trigger('carousel-show');

                controlsLock = false;
                autoSwitchOn();
            }
        }

        function autoSwitchOn () {
            autoswitchTimer = window.setTimeout(function(){
                switchPage((activePageIndex + 1) % pagesCount, 1, 800);
            }, 20*1000);
        }

        function autoSwitchOff () {
            window.clearTimeout(autoswitchTimer);
        }
    };
});
