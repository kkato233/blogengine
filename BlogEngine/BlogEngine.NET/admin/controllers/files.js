angular.module('blogAdmin').controller('FilesController', ["$rootScope", "$scope", "$location", "$filter", "$log", "dataService", function ($rootScope, $scope, $location, $filter, $log, dataService) {
    $scope.data = dataService;
    $scope.items = [];
    $scope.id = {};
    $scope.file = {};
    $scope.dirName = '';
    $scope.currentPath = '/';
    $scope.focusInput = false;

    $scope.load = function (path) {
        spinOn();
        var p = path ? { take: 0, skip: 0, path: path } : { take: 0, skip: 0 };
        dataService.getItems('/api/filemanager', p)
            .success(function (data) {
                angular.copy(data, $scope.items);
                gridInit($scope, $filter);
                $scope.currentPath = path;
                spinOff();
            })
            .error(function (data) {
                toastr.error($rootScope.lbl.Error);
                spinOff();
            });
    }

    $scope.load('');

    $scope.processChecked = function (action) {
        var i = $scope.items.length;
        var checked = [];
        while (i--) {
            var item = $scope.items[i];
            if (item.IsChecked === true) {
                checked.push(item);
            }
        }
        if (checked.length < 1) {
            return false;
        }

        if (action === "append") {
            var j = checked.length;
            while (j--) {
                var item = checked[j];
                var editorHtml = $("#editor").html();
                if (item.FileType === 1) {
                    var fileTag = "<p><a href='" + SiteVars.ApplicationRelativeWebRoot + "file.axd?file=" + item.FullPath + "' target='_blank'>" + item.Name + " (" + item.FileSize + ")</a></p>";
                    $("#editor").html(editorHtml + fileTag);
                }
                if (item.FileType === 2) {
                    $("#editor").html(editorHtml + "<img src='" + SiteVars.ApplicationRelativeWebRoot + "image.axd?picture=" + item.FullPath + "' />");
                }
            }
            toastr.success($rootScope.lbl.completed);
            $("#modal-file-manager").modal('hide');
        }
        if (action === "delete") {
            spinOn();
            dataService.processChecked("/api/filemanager/processchecked/delete", checked)
            .success(function (data) {
                $scope.load($scope.currentPath);
                gridInit($scope, $filter);
                toastr.success($rootScope.lbl.completed);
                spinOff();
            })
            .error(function () {
                toastr.error($rootScope.lbl.failed);
                spinOff();
            });
        }
    }

    $scope.uploadFile = function (files) {
        var fd = new FormData();
        fd.append("file", files[0]);

        dataService.uploadFile("/api/upload?action=filemgr&dirPath=" + $scope.currentPath, fd)
        .success(function (data) {
            $scope.load($scope.currentPath);
            gridInit($scope, $filter);
            toastr.success($rootScope.lbl.completed);
        })
        .error(function () { toastr.error($rootScope.lbl.failed); });
    }

    $scope.addFolder = function () {
        $("#modal-form").modal();
        $scope.focusInput = true;
    }

    $scope.createFolder = function () {
        if (!$('#form').valid()) {
            return false;
        }
        spinOn();
        dataService.updateItem("/api/filemanager/addfolder/add", { Name: $scope.dirName, FullPath: $scope.currentPath })
        .success(function (data) {
            $scope.load($scope.currentPath);
            gridInit($scope, $filter);
            toastr.success($rootScope.lbl.completed);
            spinOff();
            $("#modal-form").modal('hide');
        })
        .error(function () {
            toastr.error($rootScope.lbl.failed);
            spinOff();
        });
    }

    $(document).ready(function () {
        $('#form').validate({
            rules: {
                txtFolder: { required: true }
            }
        });
    });

    $scope.hasChecked = function () {
        var i = $scope.items.length;
        var checked = [];
        while (i--) {
            var item = $scope.items[i];
            if (item.IsChecked === true) {
                return true;
            }
        }
        return false;
    }

}]);