// кнопка - загрузки, при нажатии на кнопку с атрибутами data-loading-text
// она изменяется в цвете и изеняет текст внутри на текст: loading, все это длится в течении 3 сек
// затем кнопка возвращается в исходное состояние
$('[data-loading-text]').on('click',function () {
    var btn = $(this);
    btn.button('loading');
    setTimeout(function () {
        btn.button('reset')
    }, 3000)
});