var editorGetHtml = function () {
    return $('.summernote').code();
}

var editorSetHtml = function (html) {
    $('.summernote').code(html);
}

$(function () {
    $('.summernote').summernote({
        height: 240
    });
});
