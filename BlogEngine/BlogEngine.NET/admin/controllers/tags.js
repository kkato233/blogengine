angular.module('blogAdmin').controller('TagsController', function ($scope, $location, $filter, $element, $log, dataService) {
    $scope.data = dataService;
    $scope.items = [];

    $scope.id = {};
    $scope.tag = {};

    $scope.loadEditForm = function (id) {
        $scope.id = id;
        $scope.tag = id;
        $("#modal-add-tag").modal();
    }

    $scope.load = function () {
        spinOn();
        var p = { take: 0, skip: 0, postId: "" };
        dataService.getItems('/api/tags', p)
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

    $scope.deleteChecked = function () {
        spinOn();
        var i = $scope.items.length;
        while (i--) {
            var item = $scope.items[i];
            if (item.IsChecked === true) {
                $scope.items.splice(i, 1);
                var url = '/api/tags';
                dataService.deleteItem(url, { Id: item.TagName })
                    .success(function (data) {
                        toastr.success("Tag deleted");
                        $scope.load();
                        gridInit($scope, $filter);
                    })
                    .error(function () {
                        toastr.error("Error deleting tag");
                    });
            }
        }
        $scope.search();
        spinOff();
    };

    $scope.save = function () {
        if ($scope.tag) {
            dataService.updateItem("/api/tags", { OldTag: $scope.id, NewTag: $scope.tag })
           .success(function (data) {
               toastr.success("Tag updated");
               $scope.load();
               gridInit($scope, $filter, $element);
           })
           .error(function () { toastr.error("Update failed"); });
        }
        $("#modal-add-tag").modal('hide');
    }
});