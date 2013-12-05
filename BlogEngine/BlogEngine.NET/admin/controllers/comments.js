﻿angular.module('blogAdmin').controller('CommentsController', function ($scope, $location, $filter, $log, dataService) {
    $scope.items = [];
    $scope.item = {};
    $scope.id = ($location.search()).id;

    if ($scope.id) {
        $("#modal-add-item").modal();
    }

    $scope.load = function () {
        spinOn();
        var p = { type: 5, take: 0, skip: 0, filter: "", order: "" };
        dataService.getItems('/api/comments', p)
            .success(function (data) {
                angular.copy(data, $scope.items);
                gridInit($scope, $filter);
                spinOff();
            })
            .error(function (data) {
                toastr.success("Error getting tags");
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
        dataService.processChecked("/api/comments/processchecked/" + action, checked)
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

    $scope.save = function () {
        if ($scope.tag) {
            dataService.updateItem("/api/comments", { item: $scope.item })
           .success(function (data) {
               toastr.success("Comment updated");
               $scope.load();
               gridInit($scope, $filter);
           })
           .error(function () { toastr.error("Update failed"); });
        }
        $("#modal-add-item").modal('hide');
    }
});