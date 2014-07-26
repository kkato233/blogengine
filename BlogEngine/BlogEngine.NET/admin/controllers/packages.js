
angular.module('blogAdmin').controller('CustomController', ["$rootScope", "$scope", "$location", "$filter", "dataService", function ($rootScope, $scope, $location, $filter, dataService) {
    $scope.items = [];
    $scope.customFields = [];
    $scope.galleryFeeds = [];
    $scope.editId = "";
    $scope.package = {};
    $scope.fltr = 'extensions';
    $scope.IsPrimary = $rootScope.SiteVars.IsPrimary == "True";
    $scope.security = $rootScope.security;
    $scope.focusInput = false;

    $scope.id = ($location.search()).id;
    $scope.theme = ($location.search()).id;
    $scope.selectedFeed = $rootScope.SiteVars.GalleryFeedUrl;
    $scope.activeTheme = ActiveTheme;
    $scope.themesPage = false;
    $scope.showRating = false;
    $scope.extras = [];
    $scope.packageExtra = {};
    $scope.selectedRating = 0;
    
    if ($scope.id) {
        $("#modal-theme-edit").modal();
    }
    if ($location.path().indexOf("/custom/themes") == 0) {
        $scope.fltr = 'themes';
        $scope.themesPage = true;
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
        dataService.getItems('/api/packages', { take: 0, skip: 0, filter: $scope.fltr, order: "LastUpdated desc" })
        .success(function (data) {
            angular.copy(data, $scope.items);

            if ($scope.fltr != "packages") {
                $scope.loadExtras();
            }

            gridInit($scope, $filter);
            rowSpinOff($scope.items);

            var pkgId = getFromQueryString('pkgId');
            if (pkgId != null) {
                $scope.query = pkgId;
                $scope.search();
            }
        })
        .error(function () {
            toastr.error($rootScope.lbl.errorLoadingPackages);
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
            $scope.getPackageExtra(id + "." + $scope.package.OnlineVersion);
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
        processChecked("/api/packages/processchecked/", action, $scope, dataService);
    }

    $scope.pkgLinkType = function (locVersion, galVersion) {
        if (locVersion === '') {
            return "download";
        }
        if (locVersion === galVersion) {
            return "installed";
        }
        if (locVersion < galVersion) {
            return "refresh";
        }
    }

    $scope.installPackage = function (pkgId) {
        spinOn();
        dataService.updateItem("/api/packages/install/" + pkgId, pkgId)
        .success(function (data) {
            toastr.success($rootScope.lbl.completed);
            $scope.load();
            spinOff();
        })
        .error(function () {
            toastr.error($rootScope.lbl.failed);
            spinOff();
        });
    }

    $scope.uninstallPackage = function (pkgId) {
        spinOn();
        dataService.updateItem("/api/packages/uninstall/" + pkgId, pkgId)
        .success(function (data) {
            toastr.success($rootScope.lbl.completed);
            $scope.load();
            spinOff();
        })
        .error(function () {
            toastr.error($rootScope.lbl.failed);
            spinOff();
        });
    }

    $scope.refreshGalleryList = function () {
        spinOn();
        dataService.updateItem("/api/packages/refresh/list", { })
        .success(function (data) {
            toastr.success($rootScope.lbl.completed);
            spinOff();
            $scope.load();
        })
        .error(function () {
            toastr.error($rootScope.lbl.failed);
            spinOff();
        });
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

    $scope.checkStar = function (item, rating) {
        if (item === rating) {
            return true;
        }
        return false;
    }

    $scope.setRating = function (rating) {
        $scope.selectedRating = rating;
    }

    $scope.loadExtras = function () {
        $scope.extras = [];
        for (var i = 0; i < $scope.items.length; i++) {
            var item = $scope.items[i];
            if (item.OnlineVersion != null && item.OnlineVersion.length > 0) {
                dataService.getItems('/api/packageextra/' + item.Id + "." + item.OnlineVersion)
                .success(function (data) {
                    if (data) {
                        $scope.extras.push(data);
                        $scope.updatePackagesFromExtra(data);
                    }
                });
            }
        }
    }

    $scope.updatePackagesFromExtra = function (extra) {
        for (var i = 0; i < $scope.items.length; i++) {
            var item = $scope.items[i];
            if (item != null && item.Id + "." + item.OnlineVersion === extra.Id) {
                $scope.items[i].DownloadCount = extra.DownloadCount;
                $scope.items[i].Rating = extra.Rating;
            }
        }
    }

    $scope.showRatingForm = function (item, rating) {
        $scope.selectedRating = rating;
        $scope.getPackageExtra(item.Id + "." + item.OnlineVersion);
        $("#modal-rating").modal();
    }

    $scope.getPackageExtra = function (id) {
        dataService.getItems('/api/packageextra/' + id)
        .success(function (data) {
            if (data) {
                angular.copy(data, $scope.packageExtra);
            }
        });
    }

    $scope.submitRating = function () {
        var review = { "Name": UserVars.Name, "Rating": $scope.selectedRating, "Body": $("#txtReview").val() };
        dataService.updateItem("/api/packageextra/rate/" + $scope.packageExtra.Id, review)
        .success(function (data) {
            if (data.length === "") {
                toastr.success($rootScope.lbl.completed);
            }
            else {
                toastr.error(data.replace('"', '').replace('"', ''));
            }
            $("#modal-rating").modal('hide');
            $scope.load();
        })
        .error(function () {
            toastr.error($rootScope.lbl.failed);
        });
    }

}]);