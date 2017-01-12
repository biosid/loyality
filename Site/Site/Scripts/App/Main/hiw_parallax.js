define('Main/hiw_parallax', ['Shared/request_anim_frame'], function(requestAnimFrame) {
    return function () {
        
        // --------------- Данные

        var $window = $(window),
            $sections = $('section[data-type="background"]'),
            scrolled = false;



        // --------------- Инициализация

        init();

        move($window.scrollTop(), $window.height());

        (function animloop() {
            requestAnimFrame(animloop);

            if (scrolled) { 
                move($window.scrollTop(), $window.height());
                scrolled = false;
            }
        })();



        // --------------- События

        $window.on('scroll', function(){ scrolled = true; });



        // --------------- Действия

        function init() {
            $sections.each(function(){
                var $self = $(this),
                    $sprites = $self.find('[data-type="sprite"]'),
                    offsetCoords = $self.offset(),
                    topOffset = offsetCoords.top;

                $self
                    .data('sprites', $sprites)
                    .data('offsetTop', topOffset)
                    .data('height', $self.height())
                ;
                $sprites.each(function(){
                    var $sprite = $(this);
                    $sprite
                        .data('offsetY', parseInt($sprite.attr('data-offsetY')))
                        .data('Xposition', $sprite.attr('data-Xposition'))
                        .data('speed', $sprite.attr('data-speed'))
                    ;
                    var $spriteLink = $sprite.find('[data-type="sprite-link"]');
                    if ($spriteLink.length == 1) {
                        $spriteLink = $($spriteLink[0]);
                        $spriteLink.data('spriteLinkY', parseInt($spriteLink.attr('data-sprite-link-y')) || 0);
                        $sprite.data('spriteLink', $spriteLink);
                    }
                });

            });
        }

        function move (offset, windowHeight) {
            $sections.each(function () {
                var $self = $(this),
                    topOffset = $self.data('offsetTop'),
                    height = $self.data('height');

                    if (offset + windowHeight > (topOffset) && topOffset + height > offset) {						
                        var yPos = -(offset / $self.data('speed')),
                            coords = '0 ' + yPos + 'px';

                        if ($self.data('offsetY')) {
                            yPos += $self.data('offsetY');
                        }
                        $self.css({ backgroundPosition: coords });
                        $self.data('sprites').each(function () {
                            var $sprite = $(this),
                                $spriteLink = $sprite.data('spriteLink'),
                                yPos = -(offset / $sprite.data('speed')),
                                coords = $sprite.data('Xposition') + ' ' + (yPos + $sprite.data('offsetY')) + 'px';

                            $sprite.css({ backgroundPosition: coords });
                            
                            if ($spriteLink) {
                                var top = ($sprite.data('offsetY') + $spriteLink.data('spriteLinkY') + yPos) + 'px';
                                $spriteLink.css({ top: top });
                            }
                        });
                    };
            });
        }
    };
});
