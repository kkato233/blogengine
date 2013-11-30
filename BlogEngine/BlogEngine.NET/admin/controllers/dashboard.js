angular.module('blogAdmin').controller('DashboardController', function ($scope, $location, $log, dataService) {
    $scope.stats = {};
    $scope.draftposts = [];
    $scope.draftpages = [];
    $scope.recentcomments = [];
    $scope.trash = [];
    $scope.packages = [];
    $scope.logItems = [];
    $scope.itemToPurge = {};

    $scope.openLogFile = function () {
        dataService.getItems('/api/logs/getlog/file')
        .success(function (data) {
            angular.copy(data, $scope.logItems);
            $("#modal-log-file").modal();
            return false;
        })
        .error(function (data) {
            toastr.error("Error getting log file");
        });
    }

    $scope.purgeLog = function () {
        dataService.updateItem('/api/logs/purgelog/file', $scope.itemToPurge)
        .success(function (data) {
            $scope.logItems = [];
            $("#modal-log-file").modal('hide');
            toastr.success("Purged");
            return false;
        })
        .error(function (data) {
            toastr.error("Error purging");
        });
    }

    $scope.purge = function (id) {
        if (id) {
            $scope.itemToPurge = findInArray($scope.trash, "Id", id);
        }
        dataService.updateItem('/api/trash/purge/' + id, $scope.itemToPurge)
        .success(function (data) {
            $scope.loadTrash();
            toastr.success("Purged");
            return false;
        })
        .error(function (data) {
            toastr.error("Error purging");
        });
    }

    $scope.purgeAll = function () {
        dataService.updateItem('/api/trash/purgeall/all')
        .success(function (data) {
            $scope.loadTrash();
            toastr.success("Purged");
            return false;
        })
        .error(function (data) {
            toastr.error("Error purging");
        });
    }

    $scope.restore = function (id) {
        if (id) {
            $scope.itemToPurge = findInArray($scope.trash, "Id", id);
        }
        dataService.updateItem('/api/trash/restore/' + id, $scope.itemToPurge)
        .success(function (data) {
            $scope.loadTrash();
            toastr.success("Restored");
            return false;
        })
        .error(function (data) {
            toastr.error("Error restoring");
        });
    }

    $scope.load = function () {
        spinOn();

        dataService.getItems('/api/stats')
            .success(function (data) { angular.copy(data, $scope.stats); })
            .error(function (data) { toastr.success("Error getting stats"); });

        dataService.getItems('/api/posts', { take: 3, skip: 0, filter: "IsPublished == false" })
            .success(function (data) { angular.copy(data, $scope.draftposts); })
            .error(function () { toastr.error("Error loading draft posts"); });

        dataService.getItems('/api/pages', { take: 3, skip: 0, filter: "IsPublished == false" })
            .success(function (data) { angular.copy(data, $scope.draftpages); })
            .error(function () { toastr.error("Error loading draft pages"); });

        dataService.getItems('/api/comments', { type: 5, take: 5, skip: 0, filter: "IsDeleted == false", order: "DateCreated descending" })
            .success(function (data) { angular.copy(data, $scope.recentcomments); })
            .error(function () { toastr.error("Error loading recent comments"); });

        $scope.loadTrash();

        $scope.loadPackages();

        //spinOff();
    }

    $scope.loadPackages = function () {
        spinOn();
        dataService.getItems('/api/packages', { take: 5, skip: 0 })
        .success(function (data) {
            angular.copy(data, $scope.packages);
            spinOff();
        })
        .error(function () {
            toastr.error("Error loading packages");
            spinOff();
        });
    }

    $scope.loadTrash = function () {
        dataService.getItems('/api/trash', { type: 0, take: 5, skip: 0 })
            .success(function (data) { angular.copy(data, $scope.trash); })
            .error(function () { toastr.error("Error loading trash"); });
    }

    $scope.load();
});