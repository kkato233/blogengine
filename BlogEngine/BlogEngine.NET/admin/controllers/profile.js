angular.module('blogAdmin').controller('ProfileController', function ($rootScope, $scope, $rootScope, $filter, dataService) {
    $scope.user = {};
    $scope.timeStamp = Math.round(new Date().getTime() / 1000);

    $scope.load = function () {
        spinOn();
        dataService.getItems('/api/users/' + SiteVars.UserName)
        .success(function (data) {
            angular.copy(data, $scope.user);
            $scope.timeStamp = Math.round(new Date().getTime() / 1000);
            spinOff();
        })
        .error(function () {
            toastr.error($rootScope.lbl.errorLoadingUser);
            spinOff();
        });
    }

    $scope.save = function () {
        spinOn();
        dataService.updateItem("/api/users/saveprofile/item", $scope.user)
        .success(function (data) {
            toastr.success($rootScope.lbl.userUpdatedShort);
            $scope.load();
            spinOff();
        })
        .error(function () {
            toastr.error($rootScope.lbl.updateFailed);
            spinOff();
        });
    }

    $scope.removePicture = function () {
        $scope.user.Profile.PhotoUrl = "";
        $scope.save();
    }

    $scope.changePicture = function (files) {
        var fd = new FormData();
        fd.append("file", files[0]);

        dataService.uploadFile("/api/upload?action=profile", fd)
        .success(function (data) {
            $scope.user.Profile.PhotoUrl = data;
            $scope.save();
        })
        .error(function () { toastr.error($rootScope.lbl.failed); });
    }

    $scope.load();
});