function load_tags(postTags, allTags) {
    $('#postTags')
    .textext({
        plugins: 'tags autocomplete',
        tagsItems: postTags
    })
    .bind('getSuggestions', function (e, data) {
        var list = allTags,
            textext = $(e.target).textext()[0],
            query = (data ? data.query : '') || '';

        $(this).trigger(
            'setSuggestions',
            { result: textext.itemManager().filter(list, query) }
        );
    });
}

function get_tags() {
    var tags = [];
    var tagList = [];
    $('.post-tags-selector .text-tags .text-label').each(function () { tags.push($(this).text()) });
    for (var i = 0; i < tags.length; i++) {
        tagList[i] = { TagCount: 0, TagName: tags[i] };
    }
    return tagList;
}

$(function () {
    function initToolbarBootstrapBindings() {
        var fonts = ['Serif', 'Sans', 'Arial', 'Arial Black', 'Courier',
              'Courier New', 'Comic Sans MS', 'Helvetica', 'Impact', 'Lucida Grande', 'Lucida Sans', 'Tahoma', 'Times',
              'Times New Roman', 'Verdana'],
              fontTarget = $('[title=' + BlogAdmin.i18n.font + ']').siblings('.dropdown-menu');
        $.each(fonts, function (idx, fontName) {
            fontTarget.append($('<li><a data-edit="fontName ' + fontName + '" style="font-family:\'' + fontName + '\'">' + fontName + '</a></li>'));
        });
        $('a[title]').tooltip({ container: 'body' });
        $('.dropdown-menu input').click(function () { return false; })
            .change(function () { $(this).parent('.dropdown-menu').siblings('.dropdown-toggle').dropdown('toggle'); })
            .keydown('esc', function () {
            this.value = ''; $(this).change();
        });

        $('[data-role=magic-overlay]').each(function () {
            var overlay = $(this), target = $(overlay.data('target'));
            overlay.css('opacity', 0).css('position', 'absolute').offset(target.offset()).width(target.outerWidth()).height(target.outerHeight());
        });
        if ("onwebkitspeechchange" in document.createElement("input")) {
            var editorOffset = $('#editor').offset();
            $('#voiceBtn').css('position', 'absolute').offset({ top: editorOffset.top, left: editorOffset.left + $('#editor').innerWidth() - 35 });
        } else {
            $('#voiceBtn').hide();
        }
    };
    function showErrorAlert(reason, detail) {
        var msg = '';
        if (reason === 'unsupported-file-type') { msg = "Unsupported format " + detail; }
        else {
            console.log("error uploading file", reason, detail);
        }
        $('<div class="alert"> <button type="button" class="close" data-dismiss="alert">&times;</button>' +
         '<strong>File upload error</strong> ' + msg + ' </div>').prependTo('#alerts');
    };
    $("#beVersion").html(SiteVars.Version);
    initToolbarBootstrapBindings();
    $('#editor').wysiwyg({ fileUploadError: showErrorAlert });
});

function spinOn() {
    $("#spinner").removeClass("loaded");
    $("#spinner").addClass("loading");
}

function spinOff() {
    $("#spinner").removeClass("loading");
    $("#spinner").addClass("loaded");
}

function selectedOption(arr, val) {
    for (var i = 0; i < arr.length; i++) {
        if (arr[i].OptionValue.toLowerCase() === val.toLowerCase()) return arr[i];
    }
}

var keys = keys || function (o) { var a = []; for (var k in o) a.push(k); return a; };

var toSlug = function (string) {
    var accents = "\u00e0\u00e1\u00e4\u00e2\u00e8"
      + "\u00e9\u00eb\u00ea\u00ec\u00ed\u00ef"
      + "\u00ee\u00f2\u00f3\u00f6\u00f4\u00f9"
      + "\u00fa\u00fc\u00fb\u00f1\u00e7";

    var without = "aaaaeeeeiiiioooouuuunc";

    var map = {
        '@': ' at ', '\u20ac': ' euro ',
        '$': ' dollar ', '\u00a5': ' yen ',
        '\u0026': ' and ', '\u00e6': 'ae', '\u0153': 'oe'
    };

    return string
      .toLowerCase()
      .replace(
        new RegExp('[' + accents + ']', 'g'),
        function (c) { return without.charAt(accents.indexOf(c)); })
      .replace(
        new RegExp('[' + keys(map).join('') + ']', 'g'),
        function (c) { return map[c]; })
      .replace(/[^a-z0-9]/g, '-')
      .replace(/-+/g, '-')
      .replace(/^-|-$/g, '');
};

function webRoot(url) {
    var result = SiteVars.ApplicationRelativeWebRoot;
    if (url.substring(0, 1) === "/") {
        return result + url.substring(1);
    }
    else {
        return result + url;
    }
}

$(document).ready(function () {
    $("#txtTitle").focus();
});
