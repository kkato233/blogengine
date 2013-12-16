angular.module('blogEditor').controller('PostEditorController', function ($rootScope, $scope, $location, $filter, $log, dataService) {
    $scope.id = editVars.id;
    $scope.post = newPost;
    $scope.lookups = [];
    $scope.allTags = [];
    $scope.selectedAuthor = {};

    $scope.load = function () {
        dataService.getItems('/api/lookups')
        .success(function (data) {
            angular.copy(data, $scope.lookups);
            $scope.loadTags();
        })
        .error(function () {
            toastr.error("Error loading lookups");
        });
    }

    $scope.loadTags = function () {
        var tagsUrl = '/api/tags';
        var p = { take: 0, skip: 0 };
        dataService.getItems(tagsUrl, p)
        .success(function (data) {
            $scope.allTags = [];
            for (var i = 0; i < data.length; i++) {
                $scope.allTags[i] = (data[i].TagName);
            }
            if ($scope.id) {
                $scope.loadPost();
            }
            else {
                load_tags([], $scope.allTags);
                $scope.selectedAuthor = selectedOption($scope.lookups.AuthorList, SiteVars.UserName);
            }
        })
        .error(function () {
            toastr.error($rootScope.lbl.errorLoadingTags);
        });
    }

    $scope.loadPost = function () {
        spinOn();
        var url = '/api/posts/' + $scope.id;
        dataService.getItems(url)
        .success(function (data) {
            angular.copy(data, $scope.post);
            // check post categories in the list
            if ($scope.post.Categories != null) {
                for (var i = 0; i < $scope.post.Categories.length; i++) {
                    for (var j = 0; j < $scope.lookups.CategoryList.length; j++) {
                        if ($scope.post.Categories[i].Id === $scope.lookups.CategoryList[j].OptionValue) {
                            $scope.lookups.CategoryList[j].IsSelected = true;
                        }
                    }
                }
            }
            var existingTags = [];
            if ($scope.post.Tags != null) {
                for (var i = 0; i < $scope.post.Tags.length; i++) {
                    existingTags[i] = ($scope.post.Tags[i].TagName);
                }
            }
            $scope.selectedAuthor = selectedOption($scope.lookups.AuthorList, $scope.post.Author);
            load_tags(existingTags, $scope.allTags);
            $("#editor").html($scope.post.Content);
            spinOff();
        })
        .error(function () {
            toastr.error($rootScope.lbl.errorLoadingPosts);
            spinOff();
        });
    }

    $scope.save = function () {
        spinOn();
        $scope.post.Content = $("#editor").html();
        $scope.post.Author = $scope.selectedAuthor.OptionValue;
        if ($scope.post.Slug.toLowerCase() === "unpublished" || $scope.post.Slug.length == 0) {
            $scope.post.Slug = toSlug($scope.post.Title);
        }
        // get selected categories
        $scope.post.Categories = [];
        if ($scope.lookups.CategoryList != null) {
            for (var i = 0; i < $scope.lookups.CategoryList.length; i++) {
                var cat = $scope.lookups.CategoryList[i];
                if (cat.IsSelected) {
                    var catAdd = { "IsChecked": false, "Id": cat.OptionValue, "Title": cat.OptionName };
                    $scope.post.Categories.push(catAdd);
                }
            }
        }
        $scope.post.Tags = get_tags();

        if ($scope.post.Id) {
            dataService.updateItem('api/posts/update/foo', $scope.post)
           .success(function (data) {
               toastr.success($rootScope.lbl.postUpdated);
               $("#modal-form").modal('hide');
               spinOff();
           })
           .error(function () { toastr.error($rootScope.lbl.updateFailed); spinOff(); });
        }
        else {
            dataService.addItem('api/posts', $scope.post)
           .success(function (data) {
               toastr.success($rootScope.lbl.postAdded);
               if (data.Id) {
                   angular.copy(data, $scope.post);
                   var x = $scope.post.Id;
               }
               $("#modal-form").modal('hide');
               spinOff();
           })
           .error(function () { toastr.error($rootScope.lbl.failedAddingNewPost); spinOff(); });
        }
    }

    $scope.saveSource = function () {
        $scope.post.Content = $("#editor-source").val();
        $("#editor").html($("#editor-source").val());
        $("#modal-source").modal('hide');
    }

    $scope.publish = function (doPublish){
        $scope.post.IsPublished = doPublish;
        $scope.save();
    }

    $scope.uploadFile = function (action, files) {
        var fd = new FormData();
        fd.append("file", files[0]);

        dataService.uploadFile("/api/upload?action=" + action, fd)
        .success(function (data) {
            toastr.success($rootScope.lbl.uploaded);
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
        .error(function () { toastr.error($rootScope.lbl.importFailed); });
    }

    $scope.toggleEditor = function (e) {
        if ($scope.fullScreen) {
            $scope.compress();
            $scope.fullScreen = false;
        }
        else {
            $scope.expand();
            $scope.fullScreen = true;
        }
    }

    $scope.expand = function () {
        $('#overlay-editor').addClass('overlay-editor');
        $('#editor').addClass('full-editor');
    }

    $scope.compress = function () {
        $('#overlay-editor').removeClass('overlay-editor');
        $('#editor').removeClass('full-editor');
    }

    $scope.source = function () {
        $("#modal-source").modal();
        $("#editor-source").val($("#editor").html());
    }

    $scope.status = function () {
        // 0 - unpublished; 1 - saved; 2 - published;
        if ($scope.post && $scope.post.Id && $scope.post.IsPublished) {
            return 2;
        }
        if ($scope.post && $scope.post.Id && !$scope.post.IsPublished) {
            return 1;
        }
        return 0;
    }; 

    $scope.load();
});

var newPost = {
    "Id": "",
    "Title": BlogAdmin.i18n.unpublishedPost,
    "Author": "Admin",
    "Content": "<p>" + BlogAdmin.i18n.typeHere + "...</p>",
    "DateCreated": moment().format("YYYY-MM-DD HH:MM"),
    "Slug": "unpublished",
    "Categories": "",
    "Tags": "",
    "Comments": "",
    "HasCommentsEnabled": true,
    "IsPublished": false
}