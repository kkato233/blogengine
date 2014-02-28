
angular.module('blogAdmin').controller('CustomController', ["$rootScope", "$scope", "$location", "$filter", "dataService", function ($rootScope, $scope, $location, $filter, dataService) {
    $scope.items = [];
    $scope.customFields = [];
    $scope.galleryFeeds = [];
    $scope.editId = "";
    $scope.package = {};
    $scope.fltr = 'extensions';
    $scope.IsPrimary = $rootScope.SiteVars.IsPrimary == "True";
    $scope.canEditExensions = $rootScope.SiteVars.IsAdmin == "True";
    $scope.canInstallPackages = $rootScope.SiteVars.IsAdmin == "True" && $rootScope.SiteVars.IsPrimary == "True";
    $scope.focusInput = false;

    $scope.id = ($location.search()).id;
    $scope.theme = ($location.search()).id;
    $scope.selectedFeed = $rootScope.SiteVars.GalleryFeedUrl;
    
    if ($scope.id) {
        $("#modal-theme-edit").modal();
    }
    if ($location.path().indexOf("/custom/themes") == 0) {
        $scope.fltr = 'themes';
    }
    if ($location.path().indexOf("/custom/widgets") == 0) {
        $scope.fltr = 'widgets';
    }
    if ($location.path().indexOf("/custom/packages") == 0) {
        $scope.fltr = 'packages';
    }

    $scope.load = function () {
        dataService.getItems('/api/galleryfeeds')
        .success(function (data) {
            angular.copy(data, $scope.galleryFeeds);
            $scope.selectedFeedObject = selectedOption($scope.galleryFeeds, $scope.selectedFeed);
            $scope.loadPackages();
        })
        .error(function () {
            toastr.error($rootScope.lbl.errorLoadingPackages);
        });
    }

    $scope.loadPackages = function () {
        spinOn();
        dataService.getItems('/api/packages', { take: 0, skip: 0, filter: $scope.fltr, order: "LastUpdated desc" })
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

        for (var i = 0, len = $scope.items.length; i < len; i++) {
            if ($scope.items[i].Id === id) {
                angular.copy($scope.items[i], $scope.package);
                if ($scope.package) {
                    if($scope.package.SettingsUrl){
                        $scope.extEditSrc = $scope.package.SettingsUrl.replace("~/", SiteVars.RelativeWebRoot);
                    }
                }
            }
        }

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

    $scope.save = function () {
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
        var i = $scope.items.length;
        var checked = [];
        while (i--) {
            var item = $scope.items[i];
            if (item.IsChecked === true) {
                checked.push(item);
            }
        }
        if (checked.length < 1) {
            spinOff();
            return false;
        }
        dataService.processChecked("/api/packages/processchecked/" + action, checked)
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

    $scope.hasChecked = function () {
        var i = $scope.items.length;
        var checked = [];
        while (i--) {
            var item = $scope.items[i];
            if (item.IsChecked === true) {
                return true;
            }
        }
        return false;
    }

    $scope.addFeed = function () {
        if (!$('#form').valid()) {
            return false;
        }
        spinOn();
        var p = { "OptionName": $("#txtFeedName").val(), "OptionValue": $("#txtFeedUrl").val() };

        dataService.addItem("/api/galleryfeeds", p)
        .success(function (data) {
            $scope.load();
            spinOff();
        })
        .error(function () {
            toastr.error($rootScope.lbl.updateFailed);
            spinOff();
        });
    }

    $scope.removeFeed = function (feed) {
        spinOn();
        dataService.deleteItem("/api/galleryfeeds", { "Id": feed })
        .success(function (data) {
            $scope.load();
            spinOff();
        })
        .error(function () {
            toastr.error($rootScope.lbl.updateFailed);
            spinOff();
        });
    }

    $scope.changeFeed = function () {
        spinOn();
        dataService.updateItem("/api/galleryfeeds", $scope.selectedFeedObject)
        .success(function (data) {
            $scope.selectedFeed = $scope.selectedFeedObject.OptionValue;
            $scope.load();
            toastr.success($rootScope.lbl.completed);
            spinOff();
        })
        .error(function () {
            toastr.error($rootScope.lbl.updateFailed);
            spinOff();
        });
    }

    $scope.load();

    $(document).ready(function () {
        $('#form').validate({
            rules: {
                txtFeedName: { required: true },
                txtFeedUrl: { required: true }
            }
        });
    });

    $scope.loadEditForm = function (id) {
        $("#txtFeedName").val("");
        $("#txtFeedUrl").val("");
        $("#modal-feeds-edit").modal();
        $scope.focusInput = true;
    }

}]);