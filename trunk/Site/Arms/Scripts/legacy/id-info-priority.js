// подсказка
$('#info-priority').tooltip({
    animation: 'true',
    html: 'true',
    placement: 'top',
    title: 'от 1 до 1000',
    trigger: 'focus',
    delay: {hide: 100}
});

var megaval = 0;
function indecrement(change) {
    megaval += change;
    $('#info-priority').val(megaval);
}
$('.stock-action-priority', this.element).on('mousedown', function (e) {
    $(this).css('cursor', 'e-resize');
    e.preventDefault();
    var self = this;
    var val = $(self).attr('data-val');
    var inputval = $('#info-priority').val();
    if (!val) {
        $(self).attr('data-val', Number(inputval));
        $('#info-priority').val(Number(inputval));
    }    
    var clicktrack = e.pageX;
    $('.stock-action-priority').bind('mousemove', function (e) {
        var change = e.pageX - clicktrack;
        indecrement(change);
        $(self).data('val', val + change);        
        clicktrack = e.pageX;
    });
});
$('.stock-action-priority', this.element).on('mouseup mouseout', function (e) {
    $(this).css('cursor', 'text');
    $('.stock-action-priority').unbind('mousemove');
    $('#info-priority').blur();
});