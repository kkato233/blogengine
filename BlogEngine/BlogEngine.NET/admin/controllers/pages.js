angular.module('blogAdmin').controller('PagesController', function ($scope, $location, $http, $filter, $element, dataService) {
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

    $scope.deleteChecked = function () {
        var i = $scope.items.length;
        while (i--) {
            var item = $scope.items[i];
            if (item.IsChecked === true) {
                $scope.items.splice(i, 1);

                var url = '/api/pages';
                dataService.deleteItem(url, item)
                    .success(function (data) {
                        toastr.success("Page deleted");
                        gridInit($scope, $filter);
                    })
                    .error(function () {
                        toastr.error("Error deleting page");
                    });
            }
        }
        $scope.search();
    };

    $scope.publishChecked = function (p) {
        //spinOn();
        for (var i = 0; i < $scope.pagedItems[$scope.currentPage].length; i++) {
            var item = $scope.pagedItems[$scope.currentPage][i];
            if (item.IsChecked) {
                item.IsPublished = p;
                item.IsChecked = false;

                var url = '/api/pages';
                dataService.updateItem(url, item)
                    .success(function (data) {
                        toastr.success("Page updated");
                        gridInit($scope, $filter);
                    })
                    .error(function () {
                        toastr.error("Error loading pages");
                    });
            }
        }
        $scope.search();
        //spinOff();
    };

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