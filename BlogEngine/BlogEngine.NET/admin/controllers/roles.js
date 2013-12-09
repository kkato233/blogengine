angular.module('blogAdmin').controller('RolesController', function ($scope, $filter, dataService) {
    $scope.items = [];
    $scope.rights = [];
    $scope.editItem = {};
    $scope.newItem = false;

    $scope.load = function () {
        spinOn();
        dataService.getItems('/api/roles', { take: 0, skip: 0 })
            .success(function (data) {
                angular.copy(data, $scope.items);
                gridInit($scope, $filter);
                spinOff();
            })
            .error(function () {
                toastr.error("Error loading roles");
                spinOff();
            });
    }

    $scope.loadRightsForm = function (id) {
        if (!id) {
            id = "Anonymous";
            $scope.editItem = {};
            $scope.newItem = true;
        }
        else {
            $scope.newItem = false;
            $scope.loadCurrentRole(id);
        }
        spinOn();
        dataService.getItems('/api/roles/getrights/' + id)
        .success(function (data) {
            angular.copy(data, $scope.rights);
            $("#modal-edit").modal();
            spinOff();
        })
        .error(function () {
            toastr.error("Error loading rights");
            spinOff();
        });
    }

    $scope.loadCurrentRole = function (id) {
        spinOn();
        dataService.getItems('/api/roles/get/' + id)
        .success(function (data) {
            angular.copy(data, $scope.editItem);
            $("#modal-edit").modal();
            spinOff();
        })
        .error(function () {
            toastr.error("Error loading role");
            spinOff();
        });
    }

    $scope.save = function () {
        if ($scope.newItem) {
            if (!$('#form').valid()) {
                return false;
            }
            $scope.saveRole();
            $scope.saveRights();
        }
        else {
            $scope.saveRights();
        }
    }

    $scope.saveRole = function () {
        spinOn();
        dataService.addItem("/api/roles", $scope.editItem)
        .success(function (data) {
            toastr.success("Role added");
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

    $scope.saveRights = function () {
        spinOn();
        dataService.updateItem("/api/roles/saverights/" + $scope.editItem.RoleName, $scope.rights)
        .success(function (data) {
            toastr.success("Rights saved");
            $scope.load();
            spinOff();
            $("#modal-edit").modal('hide');
        })
        .error(function () {
            toastr.error("Failed to save rights");
            spinOff();
            $("#modal-edit").modal('hide');
        });
    }

    $scope.processChecked = function (action) {
        spinOn();
        dataService.processChecked("/api/roles/processchecked/" + action, $scope.items)
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

    $scope.delete = function (id) {
        spinOn();
        dataService.deleteById("/api/roles", id)
        .success(function (data) {
            toastr.success("Item " + id + " deleted");
            spinOff();
        })
        .error(function () {
            toastr.error("Could not delete item " + id);
            spinOff();
        });
    }

    $scope.load();

    $(document).ready(function () {
        $('#form').validate({
            rules: {
                txtRoleName: { required: true }
            }
        });
    });
});