angular.module('blogAdmin').controller('CommentsController', function ($scope, $location, $filter, $element, $log, dataService) {
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
                gridInit($scope, $filter, $element);
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
        while (i--) {
            var item = $scope.items[i];
            if (item.IsChecked === true) {
                var url = '/api/comments';
                if (action == "delete") {
                    $scope.items.splice(i, 1);
                    dataService.deleteItem(url, item)
                        .success(function (data) {
                            toastr.success("Comment deleted");
                            $scope.load();
                            gridInit($scope, $filter);
                        })
                        .error(function () {
                            toastr.error("Error deleting comment");
                        });
                }
                else {
                    if (action == "approve") {
                        item.IsPending = false;
                        item.IsApproved = true;
                        item.IsSpam = false;
                    }
                    if (action == "unapprove") {
                        item.IsPending = false;
                        item.IsApproved = false;
                        item.IsSpam = true;
                    }
                    dataService.updateItem(url, item)
                        .success(function (data) {
                            toastr.success("Comment Updated");
                            $scope.load();
                            gridInit($scope, $filter);
                        })
                        .error(function () {
                            toastr.error("Error deleting comment");
                        });
                }

            }
        }
        $scope.search();
        spinOff();
    };

    $scope.save = function () {
        if ($scope.tag) {
            dataService.updateItem("/api/comments", { item: $scope.item })
           .success(function (data) {
               toastr.success("Comment updated");
               $scope.load();
               gridInit($scope, $filter, $element);
           })
           .error(function () { toastr.error("Update failed"); });
        }
        $("#modal-add-item").modal('hide');
    }
});