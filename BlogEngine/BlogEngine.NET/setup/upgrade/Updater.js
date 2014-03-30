$(document).ready(function () {
    Check();
});

var newVersion = "";

function Check() {
    // CurrentVersion
    CheckVersion();

    if (!newVersion) { newVersion = ""; }

    if (newVersion.length > 0) {
        $("#spin1").hide();
        $("#spin2").hide();
        $("#spin3").hide();
        $("#spin4").hide();
        $("#spin5").hide();
        $("#spin9").hide();
        $("#step9").hide();
        $('#msg-success').hide();
        $('#spnNewVersion').html(newVersion);
    }
    else {
        $("#frm").hide();
        $("#btnRun").hide();
        $("h2").html("There is nothing to upgrade");
    }
}

function CheckVersion() {
    $("#spin1").show();
    $.ajax({
        url: AppRoot + "setup/upgrade/Updater.asmx/Check",
        data: "{ version: '" + CurrentVersion + "' }",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (result) {
            newVersion = result.d;
        }
    });
}

var upgrade = function () {
    $("#btnRun").prop("disabled", true);
    $("#btnHome").prop("disabled", true);
    $("#btnBack").prop("disabled", true);
    Download();
}

// returns version to which blog can be upgrated
function Download() {
    $("#spin1").show();
    $.ajax({
        url: AppRoot + "setup/upgrade/Updater.asmx/Download",
        data: "{ version: '" + newVersion + "' }",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var rt = result.d;
            if (rt.length > 0) {
                $("#spin1").hide();
                ShowError("1", rt);
            }
            else {
                ShowSuccess("1");
                Extract();
            }        
        }
    });
}

function Extract() {
    $("#spin2").show();
    $.ajax({
        url: AppRoot + "setup/upgrade/Updater.asmx/Extract",
        data: "{ }",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var rt = result.d;
            if (rt.length > 0) {
                $("#spin2").hide();
                ShowError("2", rt);
            }
            else {
                ShowSuccess("2");
                Backup();
            }
        }
    });
}

function Backup() {
    $("#spin3").show();
    $.ajax({
        url: AppRoot + "setup/upgrade/Updater.asmx/Backup",
        data: "{ }",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var rt = result.d;
            if (rt.length > 0) {
                $("#spin3").hide();
                ShowError("3", rt);
            }
            else {
                ShowSuccess("3");
                Delete();
            }
        }
    });
}

function Delete() {
    $("#spin4").show();
    $.ajax({
        url: AppRoot + "setup/upgrade/Updater.asmx/Delete",
        data: "{ }",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var rt = result.d;
            if (rt.length > 0) {
                $("#spin4").hide();
                ShowError("4", rt);
            }
            else {
                ShowSuccess("4");
                Install();
            }
        }
    });
}

function Install() {
    $("#spin5").show();
    $.ajax({
        url: AppRoot + "setup/upgrade/Updater.asmx/Install",
        data: "{ }",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var rt = result.d;
            if (rt.length > 0) {
                $("#spin5").hide();
                ShowError("5", rt);
            }
            else {
                ShowSuccess("5");
                $('#msg-success').show();
                $("#btnHome").prop("disabled", false);
                $("#btnBack").prop("disabled", false);
            }          
        }
    });
}

function Rollback() {
    $("#spin9").show();
    $.ajax({
        url: AppRoot + "setup/upgrade/Updater.asmx/Rollback",
        data: "{ }",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var rt = result.d;
            ShowSuccess("9");
            $('#msg-success').show();
            $("#btnHome").prop("disabled", false);
            $("#btnBack").prop("disabled", false);
        }
    });
}

function ShowSuccess(item) {
    $("#spin" + item).hide();
    $("#step" + item).removeClass("alert-info");
    $("#step" + item).addClass("alert-success");
}

function ShowError(item, msg) {
    $("#spin" + item).hide();
    $("#step" + item).removeClass("alert-info");
    $("#step" + item).addClass("alert-danger");
    $("#step" + item).find("strong").html(msg);
    $("#btnHome").prop("disabled", false);
    $("#btnBack").prop("disabled", false);
}

function goHome() {
    window.location.href = AppRoot;
}