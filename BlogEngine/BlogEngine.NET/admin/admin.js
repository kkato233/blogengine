$.ajaxSetup({
    type: "post",
    contentType: "application/json; charset=utf-8",
    dataType: "json"
});

$(document).ready(function () {

    setEditable();

    $("[id$='btnCreateRole']").click(function (evt) {
        evt.preventDefault();
        var dto = { roleName: $("[id$='_txtCreateRole']").val() };
        $.ajax({
            url: "../../api/RoleService.asmx/AddRole",
            data: JSON.stringify(dto),
            success: function (msg) {
                $('#RolesList').append('<tr id="' + dto.roleName + '"><td class="editable">' + dto.roleName + '</td><td><a href="#">delete</a></td></tr>');
                ShowStatus("success", msg.d);
            }
        });
        setEditable();
    });
});

function setEditable() {
    $('.deleteButton').live("click", function () { DeleteVal(this); });
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
    setEditable();
}

function DeleteVal(obj) {
    var id = $(obj).closest("tr").attr("id");
    var that = $(obj).closest("tr");
    var dto = { roleName: id };
    $.ajax({
        url: "../../api/RoleService.asmx/DeleteRole",
        data: JSON.stringify(dto),
        success: function (msg) {
            ShowStatus("success", msg.d);
            $(that).fadeOut('slow', function () {
                $(that).remove();
            });
        }
    });
    setEditable();
}

function SaveChanges(obj, oldVal) {
    var t = $(obj).parent().siblings(0).val();
    var dto = { oldRole: oldVal, newRole: t };
    $.ajax({
        url: "../../api/RoleService.asmx/UpdateRole",
        data: JSON.stringify(dto),
        success: function (msg) {
            ShowStatus("success", msg.d);
        }
    });
    $(obj).parent().parent().after('<td class="editable">' + t + '</td>').remove();
    setEditable();
}

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

function ShowStatus(status, msg) {
    $("[id$='_AdminStatus']").removeClass("warning");
    $("[id$='_AdminStatus']").removeClass("success");

    $("[id$='_AdminStatus']").addClass(status);
    $("[id$='_AdminStatus']").html(msg + '<a href="javascript:HideStatus()" style="width:20px;float:right">X</a>');

    if (status == "success") {
        $("[id$='_AdminStatus']").fadeIn(1000, function () { }).delay(5000).fadeOut(1000, function () { });
    }
    else {
        $("[id$='_AdminStatus']").fadeIn(1000, function () { });
    }
}

function HideStatus() {
    $("[id$='_AdminStatus']").fadeOut('slow', function () { });
}

function Show(element) {
    $("[id$='" + element + "']").slideDown('slow', function () { });
    return false;
}

function Hide(element) {
    $("[id$='" + element + "']").slideUp('slow', function () { });
    return false;
}