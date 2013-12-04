angular.module('blogAdmin').controller('PostsController', function ($scope, $location, $http, $filter, $element, dataService) {
    $scope.data = dataService;
    $scope.isBusy = false;
    $scope.title = "Posts";
    $scope.items = [];

    $scope.load = function () {
        spinOn();
        var url = '/api/posts';
        var p = { take: 0, skip: 0 }
        dataService.getItems(url, p)
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
        dataService.processChecked("/api/posts/processchecked/" + action, $scope.items)
        .success(function (data) {
            $scope.load();
            gridInit($scope, $filter, $element);
            toastr.success("Completed");
            spinOff();
        })
        .error(function () {
            toastr.error("Failed");
            spinOff();
        });
    }

    $scope.postFilter = function (fld, prm) {
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