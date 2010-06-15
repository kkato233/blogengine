
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
    $("[id$='_AdminStatus']").fadeIn(1000, function () { });
}

function HideStatus() {
    $("[id$='_AdminStatus']").fadeOut('slow', function () { });
}