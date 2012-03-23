$(document).ready(function () {
    $("body").prepend('<div id="q-notes"></div>');
    QuickNotes();
    $("#open").live("click", function () {
        $("div#q-panel").slideDown("slow");
        if ($('.q-area') && $('.q-area').val()) {
            var len = $('.q-area').val().length;
            $('.q-area').selectRange(len, len);
        }
        else {
            if ($('.q-area')) {
                $('.q-area').focus();
            }
        }
    });
    $("#close").live("click", function () {
        $("div#q-panel").slideUp("slow");
    });
    $(".closeup").live("click", function () {
        $("div#q-panel").slideUp("slow");
        $("#q-toggle a").toggle();
    });
    $("#q-toggle a").live("click", function () {
        $("#q-toggle a").toggle();
    });
});

function QuickNotes() {
    $.ajax({
        url: BlogEngineRes.webRoot + "Modules/QuickNotes/Qnotes.asmx/GetQuickNotes",
        data: "{ }",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            ClearCookies();
            $('#q-notes').setTemplateURL(BlogEngineRes.webRoot + 'Modules/QuickNotes/Templates/Panel.htm', null, { filter_data: false });
            $('#q-notes').processTemplate(msg);
        }
    });  
}

// NOTE
function GetNote(id) {
    $.ajax({
        url: BlogEngineRes.webRoot + "Modules/QuickNotes/Qnotes.asmx/GetNote",
        data: "{'id':'" + id + "'}",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (id.length > 0) {
                var rt = result.d;
                SetCookies(rt.Id, rt.Note);
                $('#q-panel').setTemplateURL(BlogEngineRes.webRoot + 'Modules/QuickNotes/Templates/Note.htm', null, { filter_data: false });
                $('#q-panel').processTemplate(result);
                $('.q-area').val(rt.Note);
            }
            else {
                $('#q-panel').setTemplateURL(BlogEngineRes.webRoot + 'Modules/QuickNotes/Templates/Note.htm', null, { filter_data: false });
                $('#q-panel').processTemplate(result);
                $('.q-area').val(JSON.parse($.cookie('quck-note-current')).Note);
            }
            var len = $('.q-area').val().length;
            $('.q-area').selectRange(len, len);
            $('#q-loading').hide();
            return false;
        }
    });
}

// LIST
function GetNotes() {
    if ($('.q-area').val()) {
        $.cookie('quck-note-current', JSON.stringify({ "Id": JSON.parse($.cookie('quck-note-current')).Id, "Note": $('.q-area').val() }), { expires: 2 });
    }
    $.ajax({
        url: BlogEngineRes.webRoot + "Modules/QuickNotes/Qnotes.asmx/GetNotes",
        data: "{ }",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $('#q-panel').setTemplateURL(BlogEngineRes.webRoot + 'Modules/QuickNotes/Templates/List.htm', null, { filter_data: false });
            $('#q-panel').processTemplate(msg);
        }
    });
}

function SaveNote() {
    if ($.trim($('.q-area').val()) == '') {
        ShowWarning("Note can not be empty");
        return false;
    }
    ShowLoader();
    var id = '';
    if ($.cookie('quck-note-current')) {
        id = JSON.parse($.cookie('quck-note-current')).Id;
    }
    $.ajax({
        url: BlogEngineRes.webRoot + "Modules/QuickNotes/Qnotes.asmx/SaveNote",
        data: "{'id':'" + id + "', 'note':'" + $('.q-area').val() + "'}",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var rt = result.d;
            SetCookies(rt.Id, rt.Note);
            GetNote(rt.Id);
        }
    });
}

function DeleteNote() {
    id = JSON.parse($.cookie('quck-note-current')).Id;
    $.ajax({
        url: BlogEngineRes.webRoot + "Modules/QuickNotes/Qnotes.asmx/DeleteNote",
        data: "{'id':'" + id + "'}",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            NewNote();
        }
    });
}

function NewNote() {
    ClearCookies();
    GetNote('');
}

function IsNewNote() {
    if (JSON.parse($.cookie('quck-note-current')).Id.length > 0) {
        return false;
    }
    else {
        return true;
    }
}

function SaveQuickPost() {
    if ($.trim($('.q-area').val()) == '') {
        ShowWarning("Note can not be empty");
        return false;
    }

    ShowLoader();

    var dto = {
        "content": $('.q-area').val()
    };

    $.ajax({
        url: BlogEngineRes.webRoot + "Modules/QuickNotes/Qnotes.asmx/SaveQuickPost",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dto),
        success: function (result) {
            var rt = result.d;
            if (rt.Success) {
                ShowSuccess("Published!");
                DeleteNote();
            }
            else {
                ShowWarning(rt.Message);
            }
        }
    });
    return false;
}

// SETTINGS
function GetSettings() {
    if ($('.q-area').val()) {
        $.cookie('quck-note-current', JSON.stringify({ "Id": JSON.parse($.cookie('quck-note-current')).Id, "Note": $('.q-area').val() }), { expires: 2 });
    }
    $.ajax({
        url: BlogEngineRes.webRoot + "Modules/QuickNotes/Qnotes.asmx/GetSettings",
        data: "{ }",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $('#q-panel').setTemplateURL(BlogEngineRes.webRoot + 'Modules/QuickNotes/Templates/Settings.htm', null, { filter_data: false });
            $('#q-panel').processTemplate(msg);
        }
    });
}

function SaveSettings() {
    ShowLoader();
    $.ajax({
        url: BlogEngineRes.webRoot + "Modules/QuickNotes/Qnotes.asmx/SaveSettings",
        data: "{'category':'" + $('#selCategory').val() + "', 'tags':'" + $('#txtTags').val() + "'}",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            ShowSuccess("Saved!");
        }
    });
}

function ClearCookies() {
    SetCookies('', '');
}

function SetCookies(id, note) {
    $.cookie('quck-note-current', JSON.stringify({ "Id": id, "Note": note }), { expires: 7 });
}

function ShowLoader() {
    $("#q-loader").removeClass("hidden");
    $("#q-loader").addClass("loader");
}

function HideLoader() {
    var q = $("#q-loader");
    q.removeClass("loader");
    q.removeClass("warning");
    q.addClass("hidden");
}

function ShowSuccess(msg) {
    var q = $("#q-loader");
    q.removeClass("loader");
    q.removeClass("warning");
    q.html(msg);
    q.addClass("success");
    q.fadeIn(1000);
    q.fadeOut(1000);
}

function ShowWarning(msg) {
    var q = $("#q-loader");
    q.removeClass("loader");
    q.removeClass("hidden");
    q.html(msg);
    q.addClass("warning");
}

$.fn.selectRange = function (start, end) {
    return this.each(function () {
        if (this.setSelectionRange) {
            this.focus();
            this.setSelectionRange(start, end);
        } else if (this.createTextRange) {
            var range = this.createTextRange();
            range.collapse(true);
            range.moveEnd('character', end);
            range.moveStart('character', start);
            range.select();
        }
    });
};