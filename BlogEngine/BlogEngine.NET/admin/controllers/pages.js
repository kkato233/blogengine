angular.module('blogAdmin').controller('PagesController', function ($scope, $location, $http, $filter, dataService) {
    $scope.data = dataService;
    $scope.items = [];

    $scope.load = function () {
        spinOn();
        var url = '/api/pages';
        var p = { take: 0, skip: 0 }
        dataService.getItems('/api/pages', p)
        .success(function (data) {
            angular.copy(data, $scope.items);
            gridInit($scope, $filter);
            spinOff();
        })
        .error(function () {
            toastr.error("Error loading posts");
            spinOff();
        });
    }

    $scope.load();

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
        dataService.processChecked("/api/pages/processchecked/" + action, $scope.items)
        .success(function (data) {
            $scope.load();
            gridInit($scope, $filter);
            toastr.success("Completed");
            spinOff();
        })
        .error(function () {
            toastr.error("Failed");
            spinOff();
        });
    }

    $scope.pageFilter = function (fld, prm) {
        var url = '/api/posts';
        var p = { page: 1, size: 0, filter: fld + ' == "' + prm + '"' }
        dataService.getItems(url, p)
        .success(function (data) {
            angular.copy(data, $scope.items);
            gridInit($scope, $filter);
        })
        .error(function () {
            toastr.error("Error filtering posts");
        });
    }
});