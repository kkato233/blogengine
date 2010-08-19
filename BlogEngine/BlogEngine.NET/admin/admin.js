
$.ajaxSetup({
    type: "post",
    contentType: "application/json; charset=utf-8",
    dataType: "json"
});

if (location.href.indexOf("Pages/Roles.aspx") != -1) {
    LoadRoles();
}

$(document).ready(function () {
    $('.editButton').live("click", function () { return EditRow(this); });
    $('.deleteButton').live("click", function () { return DeleteRow(this); });


});

//-------------		EDITING

function AddRole() {
    var txtUser = $('#txtUserName').val();
    var rowCnt = $('#Roles tr').length;
    var bg = (rowCnt % 2 == 0) ? 'bgcolor="#F8F8F8"' : 'bgcolor="#F0F0F0"';
    var row = '<tr id="' + txtUser + '" ' + bg + '><td><input type="checkbox" name="chk"' + txtUser + ' class="chk"/></td>';
    row += '<td class="editable">' + txtUser + '</td><td align="center"><a href="#" class="editButton">edit</a></td>';
    row += '<td align="center"><a href="#" class="deleteButton">delete</a></td></tr>';

    if (txtUser.length == 0) {
        $('#txtUserNameReq').removeClass('hidden');
        $('#txtUserName').focus().select();
        return false;
    }
    else {
        $('#txtUserNameReq').addClass('hidden');
        var dto = { "roleName": txtUser };

        $.ajax({
            url: "../../api/RoleService.asmx/Add",
            data: JSON.stringify(dto),
            success: function (result) {
                var rt = result.d;
                if (rt.Success) {
                    $('#Roles').append(row);
                    ShowStatus("success", rt.Message);
                }
                else {
                    ShowStatus("warning", rt.Message);
                }
            }
        });
    }
    return false;
}

function EditRow(obj) {
    var id = $(obj).closest("tr").attr("id");
    var row = $(obj).closest("tr");
    var dto = { roleName: id };

    $('.editable', row).each(function () {
        var txt = '<td><input type=\"text\" class=\"txt200\" value=\"' + $(this).html() + '"/>';
        var button = '<div><input type="button" value="Save" class="saveButton btn rounded" /> or <a href="#" class="cancelButton">Cancel</a></div></td>';
        var revert = $(this).html();
        $(this).after(txt + button).remove();

        $('.saveButton').click(function () { SaveChanges(this, revert); });
        $('.cancelButton').click(function () { CancelChanges(this, revert); });
    });
    return false;
}

function SaveChanges(obj, str) {
    var row = $(obj).closest("tr");
    var oldRole = $(obj).closest("tr").attr("id");
    var newRole = $(row).find(".txt200").val();
    var dto = { "oldRole": oldRole, "newRole": newRole };

    $.ajax({
        url: "../../api/RoleService.asmx/Edit",
        data: JSON.stringify(dto),
        success: function (result) {
            var rt = result.d;
            if (rt.Success) {
                $(obj).parent().parent().after('<td class="editable">' + newRole + '</td>').remove();
                var newRow = $("#" + oldRole);
                newRow.attr("id", newRole);
                ShowStatus("success", rt.Message);
            }
            else {
                ShowStatus("warning", rt.Message);
                $(obj).parent().parent().after('<td class="editable">' + str + '</td>').remove();
            }
        }
    });
}

function CancelChanges(obj, str) {
    $(obj).parent().parent().after('<td class="editable">' + str + '</td>').remove();
}

function DeleteRow(obj) {
    var id = $(obj).closest("tr").attr("id");
    var that = $("[id$='" + id + "']");
    var dto = { "roleName": id };

    $.ajax({
        url: "../../api/RoleService.asmx/Delete",
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
