(function () {
    var container = $('.banner_film');
    var nextButton = $('[data-move="next"]');
    var prevButton = $('[data-move="prev"]');
    var slides = $(container).find('.banner_slide');
    var currentSlide = 100;

    $(slides).css('left', '100%');
    $($(slides)[0]).css('left', '0%');
    //console.log(currentSlide);

    function bindButtons() {
        $(nextButton, this.element).on('click.stopped', function (e) {
            var next = currentSlide + 1;
            moveFilm(next, true);
        });
        $(prevButton).on('click.stopped', function (e) {
            var next = currentSlide - 1;
            moveFilm(next, false);
        });
    }

    function unbindButtons() {
        $(nextButton).unbind();
        $(prevButton).unbind();
    }

    function slideMapInArray(counter) {
        var counterInArray = Math.abs(counter - 1);
        var slidenumber = counterInArray % slides.length;
        return slidenumber;
    }

    function moveFilm(next, direction) {
        unbindButtons();
        setTimeout(bindButtons, 600);
        var translate = (function () { if (direction) { return -100; } else { return 100; } })();
        var invertTranslate = -1 * translate;
        $(slides).css({ 'left': invertTranslate + '%' });
        var nextInArray = slideMapInArray(next);
        $(slides[nextInArray]).css({ 'margin-left': '0px' });
        var curSlide = slideMapInArray(currentSlide);
        $(slides).css({ 'z-index': '1' });
        $(slides[nextInArray]).css({ 'z-index': '3' }).animate({ 'margin-left': '' + translate + '%' }, { duration: 500, complete: function () { } });
        $(slides[curSlide]).css({ 'z-index': '2', 'left': '0%', 'margin-left': '0%' }).animate({ 'margin-left': '' + translate + '%' }, { duration: 500, complete: function () { } });
        currentSlide = next;
        //console.log(currentSlide);
    }
    bindButtons();

})();