define('Main/hiw_card', ['Shared/request_anim_frame'], function(requestAnimFrame) {
    return function () {
        
        // --------------- Данные

        var stages = null,
            $card = $('.how-it-works__card'),
            $window = $(window),
            cardWidth = $card.width(),
            cardOffsetX = null,
            cardOffsetY = parseInt($card.css('top'), 10),
            overrideScroll = true,
            offset = 0,
            maxOffset = 0,
            scrollSpeed = 100,
            tweenSpeed = .1,
            tweened = -1,
            ignoreScrollEvent = false,
            stopped = true;



        // --------------- Классы

        function Stage(startCue, endCue) {
            this.start = startCue;
            this.end = endCue;
            //  кэшируем константы для линейной интерполяции
            this.oRange = this.end.o - this.start.o;
            this.xRange = this.end.x - this.start.x;
            this.yRange = this.end.y - this.start.y;
            this.sRange = this.end.s - this.start.s;
        }
        Stage.prototype = {
            // получить расчитаную позицию карты для отрезка
            calcPosition: function (offset) {
                if (offset < this.start.o || offset > this.end.o) {
                    return null; // не попали в текущий отрезок
                }

                var k = (offset - this.start.o) / this.oRange; // коэф. завершённости отрезка (от 0 до 1)

                return {
                    o: offset,
                    // линейная интерполяция
                    x: this.start.x + this.xRange * k,
                    y: this.start.y + this.yRange * k,
                    s: this.start.s + this.sRange * k
                };
            }
        };



        // --------------- Инициализация

        stages = parseStages();

        init();

        handleScrollEvent();

        (function animloop() {
            requestAnimFrame(animloop);

            if (!stopped && Math.ceil(tweened) !== Math.floor(offset)) { 
                tweened += tweenSpeed * (offset - tweened);
                ignoreScrollEvent = true;
                window.scrollTo(0, tweened);
                move(tweened);
            }
        })();



        // --------------- События

        $window.on('resize', init);

        $window.scroll(handleScrollEvent);

        $(document).on('mousewheel', function (e, delta) {
            e.preventDefault();

            // нормализация (огранчение) скорости прокрутки
            if (Math.abs(delta) > 2) {
                delta = 2 * (delta > 0 ? 1 : -1);
            }

            offset -= delta * scrollSpeed;

            if (offset < 0) {
                offset = 0;
            } if (offset > maxOffset) {
                offset = maxOffset;
            }
        });



        // --------------- Действия

        function init() {
            var scrollHeight = document.documentElement.scrollHeight ? document.documentElement.scrollHeight : document.body.scrollHeight,
                viewportHeight = document.documentElement.clientHeight ? document.documentElement.clientHeight : document.body.clientHeight;

            $card.show();
            cardOffsetX = $('.how-it-works__path:eq(0)').offset().left;
            maxOffset = scrollHeight - viewportHeight;
            offset = $window.scrollTop();
            stopped = false;
        }

        function parseStages() {
            var stages = [],
                cues = parseStageCues();
            for (var i = 1, cue, prevCue; i < cues.length; i++) {
                cue = cues[i];
                prevCue = cues[i - 1];
                stages.push(new Stage(prevCue, cue));
            }
            return stages;
        }

        function parseStageCues() {
            var cues = [];
            $('[data-cues]').each(function () {
                var raw = $(this).data("cues");
                for (var offset in raw) {
                    var rawCue = raw[offset],
                        cue = {
                            o: parseInt(offset, 10),
                            x: rawCue[0],
                            y: rawCue[1],
                            s: rawCue[2]
                        };
                    cues.push(cue);
                }
            });
            return cues;
        }

        function move(offset) {
            var pos = calcPosition(offset);

            $card.css({
                top: pos.o,
                left: pos.x + cardOffsetX,
                marginTop: pos.y,
                width: cardWidth * pos.s
            });
        }

        function handleScrollEvent(){
            if (ignoreScrollEvent) {
                ignoreScrollEvent = false;
                return;
            }
            offset = tweened = $window.scrollTop();
            move(tweened);
        }

        function calcPosition(offset) {
            for (var i = 0, stage, pos; i < stages.length; i++) {
                stage = stages[i];
                pos = stage.calcPosition(offset);
                if (pos) {
                    pos.o = cardOffsetY - stages[0].start.o;
                    return pos;
                }
            }

            var isBefore = offset < stages[0].start.o,
                edge = isBefore ? stages[0].start : stages[stages.length - 1].end,
                o = isBefore ? cardOffsetY - offset : cardOffsetY - stages[0].start.o - (offset - edge.o);

            return {
                o: o,
                x: edge.x,
                y: edge.y,
                s: edge.s
            };
        }
        
    };
});
