angular.module('blogAdmin').controller('UsersController', function ($scope, $filter, $element, dataService) {
    $scope.items = [];
    $scope.roles = [];
    $scope.editItem = {};
    $scope.isNewItem = false;

    $scope.load = function () {
        spinOn();
        dataService.getItems('/api/users', { take: 0, skip: 0, filter: "1 == 1", order: "UserName" })
            .success(function (data) {
                angular.copy(data, $scope.items);
                gridInit($scope, $filter, $element);
                spinOff();
            })
            .error(function () {
                toastr.error("Error loading users");
                spinOff();
            });
    }

    $scope.loadEditForm = function (id) {
        $scope.loadRoles(id);
        if (!id) {
            $scope.editItem = {};
            $scope.isNewItem = true;
            $("#modal-user-edit").modal();
            return false;
        }
        else {
            $scope.isNewItem = false;
        }
        spinOn();
        dataService.getItems('/api/users/' + id)
        .success(function (data) {
            angular.copy(data, $scope.editItem);
            $("#modal-user-edit").modal();
            spinOff();
        })
        .error(function () {
            toastr.error("Error loading user");
            spinOff();
        });
    }

    $scope.loadRoles = function (id) {
        spinOn();
        if (!id) {
            id = "fakeusername";
        }
        dataService.getItems('/api/roles/getuserroles/' + id)
            .success(function (data) {
                angular.copy(data, $scope.roles);
                gridInit($scope, $filter, $element);
                spinOff();
            })
            .error(function () {
                toastr.error("Error loading roles");
                spinOff();
            });
    }

    $scope.saveUser = function () {
        if (!$('#form').valid()) {
            return false;
        }
        spinOn();
        $scope.editItem.roles = $scope.roles;
        if ($scope.isNewItem) {
            dataService.addItem("/api/users", $scope.editItem)
            .success(function (data) {
                toastr.success("User added");
                $scope.load();
                spinOff();
                $("#modal-user-edit").modal('hide');
            })
            .error(function () {
                toastr.error("Error adding new user");
                spinOff();
                $("#modal-user-edit").modal('hide');
            });
        }
        else {
            dataService.updateItem("/api/users/update/item", $scope.editItem)
            .success(function (data) {
                toastr.success("User updated");
                $scope.load();
                spinOff();
                $("#modal-user-edit").modal('hide');
            })
            .error(function () {
                toastr.error("Update failed");
                spinOff();
                $("#modal-user-edit").modal('hide');
            });
        }
    }

    $scope.processChecked = function (action) {
        spinOn();
        dataService.processChecked("/api/users/processchecked/" + action, $scope.items)
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

    $scope.load();

    $(document).ready(function () {
        $('#form').validate({
            rules: {
                txtEmail: { required: true, email: true },
                txtUserName: { required: true },
                txtPassword: { required: true },
                txtConfirmPassword: { required: true, equalTo: '#txtPassword' }
            }
        });
    });
});