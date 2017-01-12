(function () {
    var commands = elFinder.prototype._options.commands,
        disabled = ['extract', 'archive', 'help', 'select', 'resize'],
        idx;

    $.each(disabled, function (i, cmd) {
        (idx = $.inArray(cmd, commands)) !== -1 && commands.splice(idx, 1);
    });
})();
