function gridInit(scope, filter, element) {
    var sortingOrder = ''; // 'Title';
    scope.sortingOrder = sortingOrder;
    scope.reverse = false;
    scope.filteredItems = [];
    scope.groupedItems = [];
    scope.itemsPerPage = 25;
    scope.pagedItems = [];
    scope.currentPage = 0;

    scope.range = function (start, end) {
        var ret = [];
        if (!end) {
            end = start;
            start = 0;
        }
        for (var i = start; i < end; i++) {
            ret.push(i);
        }
        return ret;
    };

    scope.prevPage = function () {
        if (scope.currentPage > 0) {
            scope.currentPage--;
        }
        clearChecks();
    };

    scope.nextPage = function () {
        if (scope.currentPage < scope.pagedItems.length - 1) {
            scope.currentPage++;
        }
        clearChecks();
    };

    var searchMatch = function (haystack, needle) {
        if (!needle) {
            return true;
        }
        if (!haystack) {
            return false;
        }
        return haystack.toString().toLowerCase().indexOf(needle.toString().toLowerCase()) !== -1;
    };

    scope.search = function () {
        scope.filteredItems = filter('filter')(scope.items, function (item) {
            for (var attr in item) {
                if (searchMatch(item[attr], scope.query))
                    return true;
            }
            return false;
        });
        if (scope.sortingOrder !== '') {
            scope.filteredItems = filter('orderBy')(scope.filteredItems, scope.sortingOrder, scope.reverse);
        }
        scope.currentPage = 0;
        scope.groupToPages();
    };

    scope.groupToPages = function () {
        scope.pagedItems = [];

        for (var i = 0; i < scope.filteredItems.length; i++) {
            if (i % scope.itemsPerPage === 0) {
                scope.pagedItems[Math.floor(i / scope.itemsPerPage)] = [scope.filteredItems[i]];
            } else {
                scope.pagedItems[Math.floor(i / scope.itemsPerPage)].push(scope.filteredItems[i]);
            }
        }
    };

    scope.setPage = function () {
        scope.currentPage = this.n;
        clearChecks();
    };

    scope.sort_by = function (newSortingOrder) {
        if (scope.sortingOrder == newSortingOrder)
            scope.reverse = !scope.reverse;

        scope.sortingOrder = newSortingOrder;

        $('th i').each(function () {
            $(this).removeClass().addClass('icon-sort');
        });
        if (scope.reverse)
            $('th.' + new_sorting_order + ' i').removeClass().addClass('icon-chevron-up');
        else
            $('th.' + new_sorting_order + ' i').removeClass().addClass('icon-chevron-down');
    };

    scope.gridFilter = function (field, value) {
        scope.filteredItems = filter('filter')(scope.items, function (item) {
            if (value == 'all') {
                return true;
            }
            else {
                $("#fltr-all").removeClass("active");
            }
                
            return item[field] === value;
        });
        if (scope.sortingOrder !== '') {
            scope.filteredItems = filter('orderBy')(scope.filteredItems, scope.sortingOrder, scope.reverse);
        }
        scope.currentPage = 0;
        scope.groupToPages();
    };

    scope.checkAll = function (e) {
        for (var i = 0; i < scope.pagedItems[scope.currentPage].length; i++)
            scope.pagedItems[scope.currentPage][i].IsChecked = e.target.checked;
    };

    function clearChecks() {
        for (var i = 0; i < scope.items.length; i++)
            scope.items[i].IsChecked = false;

        $('#chkAll').prop('checked', false);
    };

    scope.search();
}