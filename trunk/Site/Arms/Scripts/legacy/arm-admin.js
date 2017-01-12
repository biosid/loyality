function moderTitleDisplay(elem) {
    var flag = false;
    $('.th-filter-list input[type="checkbox"]').each(function () {
        if (this.checked) flag = true;
    });   
    $(elem).closest('.dropdown-menu').prev().find('.th-filter-display').prop('checked', flag);
    
}
$('.th-filter-list input[type="checkbox"]', this.element).change(function () {
    moderTitleDisplay(this);
});

function markParentRow(elem, flag) {
    var tr = $(elem).closest('.rowcheck');
    if (flag) { $(tr).addClass('checkedrow'); }
    else { $(tr).removeClass('checkedrow'); }
}

$('.rowcheck input[type="checkbox"]').click(function (e) {
    e.stopPropagation();
    markParentRow(this, this.checked);
});

$('.sub i', this.element).click(function () {
    $(this).parent().siblings().removeClass('expandedcat');
    $(this).parent().toggleClass('expandedcat');

});
$('.click-check .rowcheck').click(function () {
    var tr = $(this).closest('.rowcheck');
    $(tr).toggleClass('checkedrow');
    var checkbox = $(tr).find('input[type="checkbox"]');
    var checkedAttr = $(checkbox).prop("checked");
    $(checkbox).prop("checked", !checkedAttr);
});


$('.rowcheck').bind('mouseenter', function () {
    var tr = $(this).closest('.rowcheck');
    $(tr).toggleClass('hoveredrow', true);
});
$('.rowcheck').bind('mouseleave', function () {
    var tr = $(this).closest('.rowcheck');
    $(tr).toggleClass('hoveredrow', false);
});

$(document).ready(function () {
    moderTitleDisplay();    
    $('.clickStopPropagation, .rootCat, .notRootCat').click(function (e) { e.stopPropagation(); });
    $('.clickDdClose', this.element).click(function () {
        $(this).closest('.dropdown-menu').parent().removeClass('open');
    });    
});