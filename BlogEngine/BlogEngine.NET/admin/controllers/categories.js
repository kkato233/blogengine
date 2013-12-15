angular.module('blogAdmin').controller('CategoriesController', function ($scope, $location, $http, $filter, dataService) {
    $scope.items = [];
    $scope.lookups = [];
    $scope.category = {};
    $scope.id = ($location.search()).id;

    if ($scope.id) {
        dataService.getItems('/api/categories', { Id: $scope.id })
            .success(function (data) { angular.copy(data, $scope.category); })
            .error(function () { toastr.error("Error loading category"); });
        $("#modal-add-cat").modal();
    }

    $scope.addNew = function () {
        $scope.clear();
        $("#modal-add-cat").modal();
    }

    $scope.load = function () {
        spinOn();
        dataService.getItems('/api/lookups')
            .success(function (data) { angular.copy(data, $scope.lookups); })
            .error(function () { toastr.error("Error loading lookups"); });

        dataService.getItems('/api/categories')
            .success(function (data) {
                angular.copy(data, $scope.items);
                gridInit($scope, $filter);
                spinOff();
            })
            .error(function () {
                toastr.error("Error loading categories");
                spinOff();
            });
    }

    $scope.load();

    $scope.save = function () {
        if ($scope.category.Id) {
            dataService.updateItem("/api/categories/update/" + $scope.category.Id, $scope.category)
           .success(function (data) {
               toastr.success("Category updated");
               $scope.load();
           })
           .error(function () { toastr.error("Update failed"); });
        }
        else {
            dataService.addItem("/api/categories", $scope.category)
           .success(function (data) {
               toastr.success("Category added");
               if (data.Id) {
                   angular.copy(data, $scope.category);
                   $scope.load();
               }
           })
           .error(function () { toastr.error("Failed adding new category"); });
        }
        $("#modal-add-cat").modal('hide');
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
        dataService.processChecked("/api/categories/processchecked/" + action, $scope.items)
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

    $scope.clear = function () {
        $scope.category = { "IsChecked": false, "Id": null, "Parent": null, "Title": "New category", "Description": "Description", "Count": 0 };
        $scope.id = null;
    }
});