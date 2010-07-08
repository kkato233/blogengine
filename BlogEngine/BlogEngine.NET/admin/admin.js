$.ajaxSetup({
    type: "post",
    contentType: "application/json; charset=utf-8",
    dataType: "json"
});

$(document).ready(function () {
    setEditable();
});

// ================= Ajax CRUD =========================
function setEditable() {
    $('.deleteButton').live("click", function () { return DeleteVal(this); });
    $('.btnAddNew').live("click", function () { return AddVal(this); });

    $('.editable').live("click", function () {
        var txt = '<td><input type=\"text\" class=\"editBox\" value=\"' + $(this).html() + '"/>';
        var button = '<div><input type="button" value="Save" class="saveButton" /> or <a href="#" class="cancelButton">Cancel</a></div></td>';
        var revert = $(this).html();
        $(this).after(txt + button).remove();

        $('.saveButton').click(function () { SaveChanges(this, revert); });
        $('.cancelButton').click(function () { CancelChanges(this, revert); });
    })
};

function CancelChanges(obj, str) {
    $(obj).parent().parent().after('<td class="editable">' + str + '</td>').remove();
}

function AddVal(obj) {
    var dto = { roleName: $('.txtAddNew').val() };
    $.ajax({
        url: "../../api/RoleService.asmx/AddRole",
        data: JSON.stringify(dto),
        success: function (result) {
            var rt = result.d;
            if (rt.Success) {
                $('#RolesList').append('<tr id="' + dto.roleName + '"><td class="editable">' + dto.roleName + '</td><td><a href="#" class="deleteButton">delete</a></td></tr>');
                ShowStatus("success", rt.Message);
            }
            else {
                ShowStatus("warning", rt.Message);
            }
        }
    });
    return false;
}

function DeleteVal(obj) {
    var id = $(obj).closest("tr").attr("id");
    var that = $(obj).closest("tr");
    var dto = { roleName: id };
    $.ajax({
        url: "../../api/RoleService.asmx/DeleteRole",
        data: JSON.stringify(dto),
        success: function (result) {
            var rt = result.d;
            if (rt.Success) {
                ShowStatus("success", rt.Message);
                $(that).fadeOut('slow', function () {
                    $(that).remove();
                });
            }
            else {
                ShowStatus("warning", rt.Message);
            }
        }
    });
    return false;
}

function SaveChanges(obj, oldVal) {
    var t = $(obj).parent().siblings(0).val();
    var dto = { oldRole: oldVal, newRole: t };
    $.ajax({
        url: "../../api/RoleService.asmx/UpdateRole",
        data: JSON.stringify(dto),
        success: function (result) {
            var rt = result.d;
            if (rt.Success) {
                ShowStatus("success", rt.Message);
                $(obj).parent().parent().after('<td class="editable">' + t + '</td>').remove();
            }
            else {
                ShowStatus("warning", rt.Message);
                $(obj).parent().parent().after('<td class="editable">' + oldVal + '</td>').remove();
            }
        }
    });
    setEditable();
}

// ================= Validation ==================================
function ValidateEmptyField(fld, msg) {
    if ($("[id$='" + fld + "']").val().length == 0) {
        ShowStatus('warning', msg);
        return false;
    }
    return true;
}

function ValidateNotSelectedField(fld, msg) {
    if ($("[id$='" + fld + "']").attr("selectedIndex") == -1) {
        ShowStatus('warning', msg);
        return false;
    }
    return true;
}

// ================= Message and status ============================
function ShowStatus(status, msg) {
    $("[id$='_AdminStatus']").removeClass("warning");
    $("[id$='_AdminStatus']").removeClass("success");

    $("[id$='_AdminStatus']").addClass(status);
    $("[id$='_AdminStatus']").html(msg + '<a href="javascript:HideStatus()" style="width:20px;float:right">X</a>');

    if (status == "success") {
        $("[id$='_AdminStatus']").slideDown(1000, function () { }).delay(5000).slideUp(1000, function () { });
    }
    else {
        $("[id$='_AdminStatus']").slideDown(1000, function () { });
    }
}

function HideStatus() {
    $("[id$='_AdminStatus']").slideUp('slow', function () { });
}

function Show(element) {
    $("[id$='" + element + "']").slideDown('slow', function () { });
    return false;
}

function Hide(element) {
    $("[id$='" + element + "']").slideUp('slow', function () { });
    return false;
}