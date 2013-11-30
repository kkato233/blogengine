angular.module('blogAdmin').controller('ProfileController', function ($scope, $rootScope, $filter, $element, dataService) {
    $scope.user = {};

    $scope.load = function () {
        spinOn();
        dataService.getItems('/api/users/' + SiteVars.UserName)
        .success(function (data) {
            angular.copy(data, $scope.user);
            spinOff();
        })
        .error(function () {
            toastr.error("Error loading user");
            spinOff();
        });
    }

    $scope.save = function () {
        spinOn();
        dataService.updateItem("/api/users/update/item", $scope.user)
        .success(function (data) {
            toastr.success("User updated");
            $scope.load();
            spinOff();
        })
        .error(function () {
            toastr.error("Update failed");
            spinOff();
        });
    }

    $scope.load();
});