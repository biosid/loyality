function popover(options) {
    $(options.selector).popover({
        placement: options.placement,
        title: titleOpen + options.title + titleClose,
        html: 'true',
        content: btnYesNo
    });
}