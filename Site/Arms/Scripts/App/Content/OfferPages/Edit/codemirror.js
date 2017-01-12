define('Content/OfferPages/Edit/codemirror', [], function () {

    var o = {
        setupTextArea: setupTextArea
    };

    var mixedMode = {
        name: "htmlmixed",
        scriptTypes: [
            {
                matches: /\/x-handlebars-template|\/x-mustache/i,
                mode: null
            },
            {
                matches: /(text|application)\/(x-)?vb(a|script)/i,
                mode: "vbscript"
            }]
    };

    function setupTextArea(textArea) {
        var myCodeMirror = CodeMirror.fromTextArea(textArea, {
            mode: mixedMode,
            tabMode: "indent",
            value: textArea.value,
            theme: 'xq-light',
            tabSize: 2,
            smartIndent: true,
            lineNumbers: true,
            autoCloseBrackets: true,
            autoCloseTags: true
        });
    }

    return o;
});
