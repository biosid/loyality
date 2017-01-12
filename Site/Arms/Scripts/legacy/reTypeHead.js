function reTypeHead(){
    var subjects = ['PHP', 'MySQL', 'SQL', 'PostgreSQL', 'HTML', 'CSS', 'HTML5', 'CSS3', 'JSON'];
    $('.typeahead').typeahead({source: subjects});
}
reTypeHead();