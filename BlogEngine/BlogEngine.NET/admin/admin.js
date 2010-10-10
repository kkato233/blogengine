
var pageSize = 10;

LoadView();

$(document).ready(function () {
    $('.editButton').live("click", function () { return EditRow(this); });
    $('.deleteButton').live("click", function () { return DeleteRow(this); });
    $('.loader').hide();
});

//-------------		EDITING

function EditRow(obj) {
    var row = $(obj).closest("tr");
    var revert = $(row).html();
    var button = '<div><input type="button" value="Save" class="saveButton btn rounded" /> <br/> <a href="#" class="cancelButton">Cancel</a></div>';

    $('.editable', row).each(function () {
        var txt = '<td><input id="' + $(this).html() + '" type=\"text\" class=\"txt200\" value=\"' + $(this).html() + '"/></td>';
        $(this).after(txt).remove();
    });

    $(obj).replaceWith(button);

    $('.cancelButton').unbind('click');
    $('.saveButton').unbind('click');

    $('.cancelButton').bind("click", function () { CancelChanges(this, revert); });
    $('.saveButton').bind("click", function () { SaveChanges(this, revert); });

    return false;
}

function SaveChanges(obj, str) {
    var row = $(obj).closest("tr");
    var id = $(obj).closest("tr").attr("id");
    var srv = $(obj).closest("table").attr("id");
    var editVals = new Array();
    var bg = (($(obj).closest('tr').prevAll().length + 1) % 2 == 0) ? 'F8F8F8' : 'F0F0F0';
    var cnt = 0;

    $(':text', row).each(function () {
        editVals[cnt] = $(this).val();
        cnt = cnt + 1;
    });

    var dto = { "id": id, "bg": bg, "vals": editVals };
    $.ajax({
        url: "../../api/" + srv + ".asmx/Edit",
        data: JSON.stringify(dto),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var rt = result.d;
            if (rt.Success) {
                $(obj).parent().parent().parent().after(rt.Data).remove();
                ShowStatus("success", rt.Message);
            }
            else {
                ShowStatus("warning", rt.Message);
            }
        }
    });
}

function CancelChanges(obj, str) {
    var row = $(obj).closest("tr");
    var id = $(obj).closest("tr").attr("id");
    var bg = (($(obj).closest('tr').prevAll().length + 1) % 2 == 0) ? 'F8F8F8' : 'F0F0F0';

    $(obj).parent().parent().parent().after('<tr id="' + id + '" bgcolor="#' + bg + '">' + str + '</tr>').remove();
    return false;
}

function DeleteRow(obj) {
    var id = $(obj).closest("tr").attr("id");
    var srv = $(obj).closest("table").attr("id");
    var that = $("[id$='" + id + "']");
    var dto = { "id": id };

    $.ajax({
        url: "../../api/" + srv + ".asmx/Delete",
        data: JSON.stringify(dto),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var rt = result.d;
            if (rt.Success) {
                $(that).fadeOut(500, function () {
                    $(that).remove();
                });
                ShowStatus("success", rt.Message);
            }
            else {
                ShowStatus("warning", rt.Message);
            }
        }
    });
    return false;
}

//--------------	LOAD DATA VIEWS

function LoadView() {
    if (location.href.indexOf("Users/Roles.aspx") != -1) {
        LoadRoles();
    }

    if (location.href.indexOf("Users/Users.aspx") != -1) {
        LoadUsers();
    }

    if (location.href.indexOf("Users/Profile.aspx") != -1) {
        LoadProfile();
    }
}

function LoadComments(pg, srvs) {
    $.ajax({
        url: srvs + "/LoadComments",
        data: "{'PageSize':'" + pageSize + "', 'Page':'" + pg + "'}",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $('#Container').setTemplateURL('../../Templates/comments.htm', null, { filter_data: false });
            $('#Container').processTemplate(msg);
            LoadPager(pg, srvs);
        }
    });
    return false;
}

function LoadPager(pg, srvs) {
    $.ajax({
        url: srvs + "/LoadPager",
        data: "{'PageSize':'" + pageSize + "', 'Page':'" + pg + "'}",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $('#Pager').html(msg.d);
        }
    });
    return false;
}

function LoadRoles() {
    $.ajax({
        url: "Roles.aspx/GetRoles",
        data: "{ }",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $('#Container').setTemplateURL('../../Templates/roles.htm', null, { filter_data: false });
            $('#Container').processTemplate(msg);
        }
    });
}

function LoadUsers() {
    $.ajax({
        url: "Users.aspx/GetUsers",
        data: "{ }",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $('#Container').setTemplateURL('../../Templates/users.htm', null, { filter_data: false });
            $('#Container').processTemplate(msg);
        }
    });
}

function LoadProfile() {
    var dto = { "id": Querystring('id') };
    $.ajax({
        url: "Profile.aspx/GetProfile",
        data: JSON.stringify(dto),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $('#Container').setTemplateURL('../../Templates/profile.htm', null, { filter_data: false });
            $('#Container').processTemplate(msg);

            $('#Container2').setTemplateURL('../../Templates/profile2.htm', null, { filter_data: false });
            $('#Container2').processTemplate(msg);
        }
    });
}

//--------------    COMMENTS

var rowLoading = '<td colspan="8" style="text-align:center"><img src="../../pics/ajax-loader.gif" alt="Loading" /></td>';

function ProcessSelected(action, page) {
    var vals = new Array();
    var cnt = 0;
    // action: approve, reject, restore or delete
    var srv = "../../api/Comments.asmx/" + action;

    $('input[@type=checkbox]:checked').each(function () {
        vals[cnt] = $(this).closest("tr").attr("id");
        cnt = cnt + 1;
    });

    if (cnt == 0) return false;
    $('.loader').show();

    var dto = { "vals": vals };
    $.ajax({
        url: srv,
        data: JSON.stringify(dto),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var rt = result.d;
            if (rt.Success) {
                $('input[@type=checkbox]:checked').each(function () { 
                    var that = $("[id$='" + $(this).closest("tr").attr("id") + "']");
                    
                    $(that).fadeOut(500, function () {
                        $(that).remove();
                    });

                    // update menu counters
                    var com_cnt = $('#comment_counter').text();
                    var spm_cnt = $('#spam_counter').text();
                    var pbk_cnt = $('#pingback_counter').text();
                    var pnd_cnt = $('#pending_counter').text();

                    if (action == 'Reject') {
                        $('#spam_counter').text(parseInt(spm_cnt) + 1);

                        if (page == 'Approved') $('#comment_counter').text(parseInt(com_cnt) - 1);
                        if (page == 'Pending') $('#pending_counter').text(parseInt(pnd_cnt) - 1);
                    }
                    if (action == 'Approve') {
                        $('#comment_counter').text(parseInt(com_cnt) + 1);

                        if (page == 'Pending') $('#pending_counter').text(parseInt(pnd_cnt) - 1);
                        if (page == 'Spam') $('#spam_counter').text(parseInt(spm_cnt) - 1);
                    }
                    if (action == 'Delete') {
                        if (page == 'Approved') $('#comment_counter').text(parseInt(com_cnt) - 1);
                        if (page == 'Spam') $('#spam_counter').text(parseInt(spm_cnt) - 1);
                        if (page == 'Pingback') $('#pingback_counter').text(parseInt(pbk_cnt) - 1);
                        if (page == 'Pending') $('#pending_counter').text(parseInt(pnd_cnt) - 1);
                    }
                });
            }
            else {
                ShowStatus("warning", rt.Message);
            }
        }
    });

    $('.loader').hide();
    
    ShowStatus("success", "Updated");
    return false;
}

var editingComment = '';

function EditComment(id) {
    var oRow = $("[id$='" + id + "']");
    var hRow = oRow.html();
    editingComment = hRow;

    $(oRow).html(rowLoading);

    var dto = { "id": id };
    $.ajax({
        url: "Approved.aspx/GetComment",
        data: JSON.stringify(dto),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            oRow.setTemplateURL('../../Templates/editcomment.htm', null, { filter_data: false });
            oRow.processTemplate(result);
        }
    });

    return false;
}

function SaveComment(obj) {
    var frm = document.forms['aspnetForm'];
    $(frm).validate({
        rules: {
            txtAuthor: {
                required: true,
                maxlength: 30
            },
            txtComment: {
                required: true,
                maxlength: 2000
            },
            txtEmail: {
                required: true,
                email: true
            }
        }
    });

    var isValid = $(frm).valid();
    if (!isValid) return false;

    var oRow = $(obj).closest("tr");
    var vals = new Array();

    vals[0] = $(obj).closest("tr").attr("id");
    vals[1] = $("#txtAuthor").val();
    vals[2] = $("#txtEmail").val();
    vals[3] = $("#txtWebsite").val();
    vals[4] = $("#txtComment").val();

    var dto = { "vals": vals };
    $.ajax({
        url: "Approved.aspx/SaveComment",
        data: JSON.stringify(dto),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            oRow.setTemplateURL('../../Templates/commentrow.htm', null, { filter_data: false });
            oRow.processTemplate(result);
        }
    });
    
    return false;
}

function CancelEditComment(obj) {
    var oRow = $(obj).closest("tr");
    $(oRow).html(editingComment);
    //$(oRow).fadeOut(1).html(editingComment).fadeIn(1);
}

function DeleteComment(id) {
    var oRow = $("[id$='" + id + "']");
    var hRow = oRow.html();

    oRow.html(rowLoading);

    var vals = new Array();
    vals[0] = id;
    var dto = { "vals": vals };
    $.ajax({
        url: "../../api/Comments.asmx/Delete",
        data: JSON.stringify(dto),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var rt = result.d;
            if (rt.Success) {
                var com_cnt = $('#comment_counter').text();
                var spm_cnt = $('#spam_counter').text();
                var pbk_cnt = $('#pingback_counter').text();
                var pnd_cnt = $('#pending_counter').text();

                if (location.href.indexOf('Comments\/Approved.aspx') > 0) $('#comment_counter').text(parseInt(com_cnt) - 1);
                if (location.href.indexOf('Comments\/Spam.aspx') > 0) $('#spam_counter').text(parseInt(spm_cnt) - 1);
                if (location.href.indexOf('Comments\/Pingback.aspx') > 0) $('#pingback_counter').text(parseInt(pbk_cnt) - 1);
                if (location.href.indexOf('Comments\/Pending.aspx') > 0) $('#pending_counter').text(parseInt(pnd_cnt) - 1);

                $(oRow).fadeOut(500, function () {
                    $(oRow).remove();
                });
                ShowStatus("success", rt.Message);
            }
            else {
                oRow.html(hRow);
                ShowStatus("warning", rt.Message);
            }
        }
    });   
    return false;
}

function DeleteAllSpam() {
    $('.loader').show();
    $.ajax({
        url: "../../api/Comments.asmx/DeleteAll",
        data: "{ }",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var rt = result.d;
            if (rt.Success) {
                $('.chk').each(function () {
                    var that = $("[id$='" + $(this).closest("tr").attr("id") + "']");
                    $(that).fadeOut(500, function () {
                        $(that).remove();
                    });
                    ShowStatus("success", rt.Message);
                    $('#spam_counter').text('0');
                });
            }
            else {
                ShowStatus("warning", rt.Message);
            }
        }
    });
    $('.loader').hide();
    return false;
}

//-------------- 	HELPERS AND MISC

function toggleAllChecks(o) {
    if ($(o).attr('checked')) {
        $('.chk').not(':disabled').attr('checked', 'checked');
    }
    else {
        $('.chk').attr('checked', '');
    }
    return false;
}

function formatJSONDate(jsonDate) {
    var date = new Date(parseInt(jsonDate.substr(6)));
    var parsedDate = Date.parse(date);
    var d = new Date(parsedDate);
    var m = d.getMonth() + 1;
    var s = m + "/" + d.getDate() + "/" + d.getFullYear();

    if (s == "1/1/1001") {
        return "";
    } else {
        return s;
    }
}

function Querystring(key) {
    key = key.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regex = new RegExp("[\\?&]" + key + "=([^&#]*)");
    var qs = regex.exec(window.location.href);
    if (qs == null)
        return "";
    else
        return qs[1];
}

//--------------	STATUS AND MESSAGES

function ShowStatus(status, msg) {
    $("[id$='AdminStatus']").removeClass("warning");
    $("[id$='AdminStatus']").removeClass("success");
    $("[id$='AdminStatus']").addClass(status);

    if (status == "success") {
        $("[id$='AdminStatus']").html(msg);
        $("[id$='AdminStatus']").fadeIn(1000, function () { }).delay(5000).fadeOut(1000, function () { });
    }
    else {
        $("[id$='AdminStatus']").html(msg + '<a href="javascript:HideStatus()" style="padding-left:20px; color:#444">close</a>');
        $("[id$='AdminStatus']").fadeIn(1000, function () { });
    }
}

function HideStatus() {
    $("[id$='AdminStatus']").slideUp('slow', function () { });
}

function Show(element) {
    $("[id$='" + element + "']").slideDown('slow', function () { });
    return false;
}

function Hide(element) {
    $("[id$='" + element + "']").slideUp('slow', function () { });
    return false;
}
