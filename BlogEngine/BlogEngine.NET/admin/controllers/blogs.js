angular.module('blogAdmin').controller('BlogsController', function ($rootScope, $scope, $filter, $element, dataService) {
    $scope.items = [];
    $scope.editItem = {};
    $scope.newItem = {};
    $scope.modalTitle = $rootScope.lbl.addNewBlog;
    $scope.blogPath = $rootScope.SiteVars.AbsoluteWebRoot;

    $scope.modalNew = function () {
        $scope.modalTitle = $rootScope.lbl.addNewBlog;
        $scope.editItem = {};
        $("#modal-add").modal();
    }

    $scope.modalEdit = function (id) {
        $scope.modalTitle = "Edit blog";
        spinOn();
        dataService.getItems('/api/blogs/' + id)
        .success(function (data) {
            angular.copy(data, $scope.editItem);
            $("#modal-edit").modal();
            spinOff();
        })
        .error(function () {
            toastr.error("Error loading blog");
            spinOff();
        });
    }

    $scope.load = function () {
        spinOn();
        dataService.getItems('/api/blogs', { take: 0, skip: 0, filter: "1 == 1", order: "Name" })
            .success(function (data) {
                angular.copy(data, $scope.items);
                gridInit($scope, $filter, $element);
                spinOff();
            })
            .error(function () {
                toastr.error("Error loading blogs");
                spinOff();
            });
    }

    $scope.save = function () {
        spinOn();
        dataService.updateItem("/api/blogs", $scope.editItem)
        .success(function (data) {
            toastr.success("Blog saved");
            $scope.load();
            spinOff();
            $("#modal-edit").modal('hide');
        })
        .error(function () {
            toastr.error("Failed adding new role");
            spinOff();
            $("#modal-edit").modal('hide');
        });
    }

    $scope.saveNew = function () {
        if (!$('#form').valid()) {
            return false;
        }
        spinOn();
        dataService.addItem("/api/blogs", $scope.newItem)
        .success(function (data) {
            toastr.success("Blog added");
            $scope.newItem = {};
            $scope.load();
            spinOff();
            $("#modal-add").modal('hide');
        })
        .error(function () {
            toastr.error("Failed adding new role");
            spinOff();
            $("#modal-add").modal('hide');
        });
    }

    $scope.deleteChecked = function () {
        var i = $scope.items.length;
        while (i--) {
            if ($scope.items[i].IsChecked === true) {
                $scope.items.splice(i, 1);
                $scope.items[i].IsChecked = false;
            }
        }
        $scope.search();
    };

    $scope.load();

    $(document).ready(function () {
        $('#form').validate({
            rules: {
                txtBlogName: { required: true },
                txtUserName: { required: true },
                txtEmail: { required: true, email: true },
                txtPassword: { required: true },
                txtConfirmPassword: { required: true, equalTo: '#txtPassword' }
            }
        });
    });
});