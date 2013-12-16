
angular.module('blogAdmin').controller('CustomController', function ($scope, $location, $filter, dataService) {
    $scope.items = [];
    $scope.customFields = [];
    $scope.editId = "";
    $scope.package = {};
    $scope.pkgType = "Extension";
    $scope.pkgLocation = "L";
    $scope.fltr = 'PackageType == "Extension" and Location != "G"';
    $scope.canEditExensions = SiteVars.IsAdmin;
    $scope.canInstallPackages = SiteVars.IsAdmin;

    $scope.id = ($location.search()).id;
    $scope.theme = ($location.search()).id;

    if ($scope.id) {
        $("#modal-theme-edit").modal();
    }

    if ($location.path().indexOf("/custom/themes") == 0) {
        $scope.pkgType = "Theme";
        $scope.fltr = 'PackageType == "Theme"  and Location != "G"';
    }
    if ($location.path().indexOf("/custom/widgets") == 0) {
        $scope.pkgType = "Widget";
        $scope.fltr = 'PackageType == "Widget"  and Location != "G"';
    }

    $scope.load = function () {
        spinOn();
        var loc = $scope.pkgLocation == 'G' ? ' == "G"' : ' != "G"';
        $scope.fltr = 'PackageType == "' + $scope.pkgType + '" and Location ' + loc;

        dataService.getItems('/api/packages', { take: 0, skip: 0, filter: $scope.fltr, order: "Title" })
            .success(function (data) {
                angular.copy(data, $scope.items);
                gridInit($scope, $filter);
                spinOff();
            })
            .error(function () {
                toastr.error($rootScope.lbl.errorLoadingPackages);
                spinOff();
            });
    }

    $scope.loadCustomFields = function (id) {
        $scope.editId = id;
        $scope.extEditSrc = SiteVars.RelativeWebRoot + "admin/Extensions/Settings.aspx?ext=" + id + "&enb=False";
        $scope.customFields = [];

        dataService.getItems('/api/packages', { Id: id })
        .success(function (data) { angular.copy(data, $scope.package); })
        .error(function () { toastr.error($rootScope.lbl.errorLoadingPackage); });

        dataService.getItems('/api/customfields', { filter: 'CustomType == "THEME" && ObjectId == "' + id + '"' })
        .success(function (data) {
            angular.copy(data, $scope.customFields);
            $("#modal-theme-edit").modal();
        })
        .error(function () {
            toastr.error($rootScope.lbl.errorLoadingCustomFields);
        });
    }

    $scope.relocate = function (loc) {
        $scope.pkgLocation = loc;
        $("#fltr-loc").removeClass("active");
        $("#fltr-gal").removeClass("active");
        $scope.load();
    }

    $scope.saveCustomFields = function () {
        spinOn();

        dataService.updateItem("/api/packages/update/foo", $scope.package)
        .success(function (data) {
            toastr.success($rootScope.lbl.completed);
        })
        .error(function () {
            toastr.error($rootScope.lbl.failed);
        });

        dataService.updateItem("/api/customfields", $scope.customFields)
        .success(function (data) {
            toastr.success($rootScope.lbl.customFieldsUpdated);
            $scope.load();
            spinOff();
            $("#modal-theme-edit").modal('hide');
        })
        .error(function () {
            toastr.error($rootScope.lbl.updateFailed);
            spinOff();
            $("#modal-theme-edit").modal('hide');
        });
    }

    $scope.processChecked = function (action) {
        spinOn();
        dataService.processChecked("/api/packages?action=" + action, $scope.items)
        .success(function (data) {
            $scope.load();
            gridInit($scope, $filter);
            toastr.success($rootScope.lbl.completed);
            spinOff();
        })
        .error(function () {
            toastr.error($rootScope.lbl.failed);
            spinOff();
        });
    }

    $scope.load();
});