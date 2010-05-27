
function ToggleAdminStatus(dir) {
    if (dir == 'up') {
        $("[id$='_AdminStatus']").slideUp('slow', function () { });
    } else {
        $("[id$='_AdminStatus']").slideDown('slow', function () { });
    }
}