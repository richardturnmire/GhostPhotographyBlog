$(document).ready(function () {
    contentToHTML();
});

function contentToHTML() {
    var html = $('#snip-content').text();
    html = html.replace("<textarea>", "");
    $('#blog-snip').append(html);
}