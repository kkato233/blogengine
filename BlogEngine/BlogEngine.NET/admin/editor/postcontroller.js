angular.module('blogEditor').controller('PostEditorController', function ($scope, $location, $filter, $log, dataService) {
    $scope.id = editVars.id;
    $scope.post = newPost;
    $scope.lookups = [];
    $scope.allCats = [];
    $scope.allTags = [];
    $scope.selectedAuthor = {};
    $scope.fullScreen = false;

    $scope.load = function () {
        dataService.getItems('/api/lookups')
        .success(function (data) {
            angular.copy(data, $scope.lookups);
            $scope.allCats = [];
            if (data != null && data.CategoryList != null) {
                for (var i = 0; i < data.CategoryList.length; i++) {
                    $scope.allCats[i] = (data.CategoryList[i].OptionName);
                }
            }
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
                load_cats([], $scope.allCats);
                load_tags([], $scope.allTags);
                $scope.selectedAuthor = selectedOption($scope.lookups.AuthorList, SiteVars.UserName);
            }
        })
        .error(function () {
            toastr.error("Error loading tags");
        });
    }

    $scope.loadPost = function () {
        spinOn();
        var url = '/api/posts/' + $scope.id;
        dataService.getItems(url)
        .success(function (data) {
            angular.copy(data, $scope.post);
            existingCategories = [];
            existingTags = [];
            if ($scope.post.Categories != null) {
                for (var i = 0; i < $scope.post.Categories.length; i++) {
                    existingCategories[i] = ($scope.post.Categories[i].Title);
                }
            }
            if ($scope.post.Tags != null) {
                for (var i = 0; i < $scope.post.Tags.length; i++) {
                    existingTags[i] = ($scope.post.Tags[i].TagName);
                }
            }
            $scope.selectedAuthor = selectedOption($scope.lookups.AuthorList, $scope.post.Author);
            load_tags(existingTags, $scope.allTags);
            load_cats(existingCategories, $scope.allCats);
            $("#editor").html($scope.post.Content);
            spinOff();
        })
        .error(function () {
            toastr.error("Error loading posts");
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
        $scope.post.Categories = get_cats();
        $scope.post.Tags = get_tags();

        if ($scope.post.Id) {
            dataService.updateItem('api/posts/update/foo', $scope.post)
           .success(function (data) {
               toastr.success("Post updated");
               $("#modal-form").modal('hide');
               spinOff();
           })
           .error(function () { toastr.error("Update failed"); spinOff(); });
        }
        else {
            dataService.addItem('api/posts', $scope.post)
           .success(function (data) {
               toastr.success("Post added");
               $log.log(data);
               if (data.Id) {
                   angular.copy(data, $scope.post);
                   var x = $scope.post.Id;
               }
               $("#modal-form").modal('hide');
               spinOff();
           })
           .error(function () { toastr.error("Failed adding new post"); spinOff(); });
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

    $scope.load();
});

var newPost = {
    "Id": "",
    "Title": "Unpublished",
    "Author": "Admin",
    "Content": "<p>Type here...</p>",
    "DateCreated": moment().format("MM/DD/YYYY HH:MM"),
    "Slug": "unpublished",
    "Categories": "",
    "Tags": "",
    "Comments": "",
    "HasCommentsEnabled": true,
    "IsPublished": true
}