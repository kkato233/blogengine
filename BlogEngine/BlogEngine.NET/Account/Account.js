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

function Hide(element) {
    $("[id$='" + element + "']").fadeOut('slow', function () { });
    return false;
}

function ValidatePasswordRetrieval() {
    //alert($("[id$='txtEmail']").val().length);

    if ($("[id$='txtEmail']").val().length == 0) {
        ShowStatus('warning', 'Email is required');
        return false;
    }
    return true;
}