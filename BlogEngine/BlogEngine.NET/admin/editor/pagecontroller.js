angular.module('blogEditor').controller('PageEditorController', function ($scope, $location, $filter, $element, $log, dataService) {
    $scope.id = editVars.id;
    $scope.page = newPage;
    $scope.lookups = [];
    $scope.selectedParent = {};
    $scope.htmlMode = true;

    $scope.load = function () {
        var lookupsUrl = '/api/lookups';
        dataService.getItems(lookupsUrl)
        .success(function (data) {
            angular.copy(data, $scope.lookups);
            if ($scope.id) {
                $scope.loadPage();
            }
        })
        .error(function () {
            toastr.error("Error loading lookups");
        });
    }

    $scope.loadPage = function () {
        spinOn();
        var url = '/api/pages/' + $scope.id;
        dataService.getItems(url)
        .success(function (data) {
            angular.copy(data, $scope.page);
            if ($scope.page.Parent != null) {
                $scope.selectedParent = selectedOption($scope.lookups.PageList, $scope.page.Parent.OptionValue);
                var x = $scope.selectedParent;
            }
            $("#editor").html($scope.page.Content);
            spinOff();
        })
        .error(function () {
            toastr.error("Error loading page");
            spinOff();
        });
    }

    $scope.save = function () {
        spinOn();
        $scope.page.Content = $("#editor").html();
        $scope.page.Parent = $scope.selectedParent;

        if ($scope.page.Slug.toLowerCase() === "unpublished" || $scope.page.Slug.length == 0) {
            $scope.page.Slug = toSlug($scope.page.Title);
        }

        if ($scope.page.Id) {
            dataService.updateItem('/api/pages/update/foo', $scope.page)
           .success(function (data) {
               toastr.success("Page updated");
               $("#modal-form").modal('hide');
               spinOff();
           })
           .error(function () {
               toastr.error("Update failed");
               spinOff();
           });
        }
        else {
            dataService.addItem('/api/pages', $scope.page)
           .success(function (data) {
               toastr.success("Page added");
               $log.log(data);
               if (data.Id) {
                   angular.copy(data, $scope.page);
                   var x = $scope.page.Id;
               }
               $("#modal-form").modal('hide');
               spinOff();
           })
           .error(function () {
               toastr.error("Failed adding new page");
               spinOff();
           });
        }
    }

    $scope.toggleEditor = function () {
        if ($scope.htmlMode) {
            $("#editor").fadeOut();
            $("#editorSource").fadeIn();
            $scope.page.Content = $("#editor").html();
            $scope.htmlMode = false;
        }
        else {
            $("#editor").fadeIn();
            $("#editorSource").fadeOut();
            $scope.htmlMode = true;
        }
    }

    $scope.uploadFile = function (action, files) {
        var fd = new FormData();
        fd.append("file", files[0]);

        dataService.uploadFile("/api/upload?action=" + action, fd)
        .success(function (data) {
            toastr.success("Uploaded");
            var editorHtml = $("#editor").html();
            if (action === "image") {
                $("#editor").html(editorHtml + '<img src=' + data + ' />');
            }
            if (action === "video") {
                $("#editor").html(editorHtml + '<p>[video src=' + data + ']</p>');
            }
            if (action === "file") {
                var res = data.split("|");
                if (res.length === 2) {
                    $("#editor").html(editorHtml + '<a href="' + res[0].replace('"', '') + '">' + res[1].replace('"', '') + '</a>');
                }
            }
        })
        .error(function () { toastr.error("Import failed"); });
    }

    $scope.load();
});

var newPage = {
    "Id": "",
    "Title": "Unpublished page",
    "Content": "<p>Type here...</p>",
    "DateCreated": moment().format("MM/DD/YYYY HH:MM"),
    "Slug": "unpublished",
    "IsPublished": true
}