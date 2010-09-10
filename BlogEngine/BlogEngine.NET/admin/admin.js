
$.ajaxSetup({
    type: "post",
    contentType: "application/json; charset=utf-8",
    dataType: "json"
});

LoadView();

$(document).ready(function () {
    $('.editButton').live("click", function () { return EditRow(this); });
    $('.deleteButton').live("click", function () { return DeleteRow(this); });
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

    var dto = { "id": id, "bg" : bg, "vals": editVals };
    $.ajax({
        url: "../../api/" + srv + ".asmx/Edit",
        data: JSON.stringify(dto),
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

function LoadRoles() {
    $.ajax({
        url: "Roles.aspx/GetRoles",
        data: "{ }",
        success: function (msg) {
            $('#Container').setTemplateURL('../../Templates/roles.htm', null, { filter_data: false });
            $('#Container').processTemplate(msg);

            //$('#RSSTable').tablesorter();
        }
    });
}

function LoadUsers() {
    $.ajax({
        url: "Users.aspx/GetUsers",
        data: "{ }",
        success: function (msg) {
            $('#Container').setTemplateURL('../../Templates/users.htm', null, { filter_data: false });
            $('#Container').processTemplate(msg);

            //$('#RSSTable').tablesorter();
        }
    });
}

function LoadProfile() {
    var dto = { "id": Querystring('id') };
    $.ajax({
        url: "Profile.aspx/GetProfile",
        data: JSON.stringify(dto),
        success: function (msg) {
            $('#Container').setTemplateURL('../../Templates/profile.htm', null, { filter_data: false });
            $('#Container').processTemplate(msg);

            $('#Container2').setTemplateURL('../../Templates/profile2.htm', null, { filter_data: false });
            $('#Container2').processTemplate(msg);
        }
    });
}

//-------------- 	HELPERS AND MISC

function toggleAllChecks(o) {
    if ($(o).attr('checked')) {
        $('.chk').attr('checked', 'checked');
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
