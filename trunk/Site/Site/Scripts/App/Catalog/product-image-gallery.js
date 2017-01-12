define('Catalog/product-image-gallery', function() {
    return function(options) {
        var container = $(options.el);
        var fancyBoxElem = $(options.fancy);
        var nextBtn = $("[data-x='Catalog/product-image-gallery/next']");
        var prevBtn = $("[data-x='Catalog/product-image-gallery/prev']");
        var image = container.find('[data-product=image]');
        
        fancyBoxElem.magnificPopup({
            type:'image',
            tLoading: 'Загрузка...',
            tClose: 'Закрыть',
            closeOnContentClick: true,
            image: {
                tError: 'Невозможно загрузить <a href="%url%">изображение</a>'
            },
            removalDelay: 300,
            mainClass: 'mfp-fade'
        });

        container.on('click', options.zoomer, function () {
            fancyBoxElem.magnificPopup('open');
        });

        var gallery = container.find('[data-product=gallery]');

        gallery.find('li:first-child').addClass('active');

        container.on('click', '.prod-card .prev', function () {
            var activeLi = gallery.find('li.active');
            var prevLi = activeLi.prev('li');
            
            if (prevLi.length == 0) {
                activeLi.siblings(':last').find('a').click();
            } else {
                prevLi.find('a').click();
            }
        });

        container.on('click', '.prod-card .next', function () {
            var activeLi = gallery.find('li.active');
            var nextLi = activeLi.next('li');
            
            if (nextLi.length == 0) {
                activeLi.siblings(':first').find('a').click();
            } else {
                nextLi.find('a').click();
            }
        });

        gallery.on('click', 'li a', function (e) {
            var li = $(this).parent();
            li.addClass('active').siblings().removeClass('active');

            var span = $(this).find('span');
            var productImg = span.attr('data-product');
            var productFullSizeImg = span.attr('data-product-fullsize');
            image.css("background-image", "url(" + productImg + ")");
            fancyBoxElem.prop('href', productFullSizeImg);

            e.preventDefault();
        });
    };    
});
