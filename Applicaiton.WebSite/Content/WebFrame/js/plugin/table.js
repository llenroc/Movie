(function ($) {
    var defaultSettings = {
        title: null,
        paging: false,
        pageSize: 10,
        pageSizes: [10, 25, 50, 100, 250, 500],
        pageLength:3,
        sorting: true,
        multiSorting: true,
        defaultSorting: '',
        initializeLoad:true,
        saveUserPreferences:false,
        actions: {
            get: function (sortParam,currentPageIndex,pageSize) { },
            delete: function () { },
            add:function(){},
        },
        fields: {}
    };

    function Table(element, options) {
        var _self = this;
        this.element = element;
        this.$element = $(element);
        this.settings = $.extend({}, defaultSettings, options);
        this.lastSorting = [];
        this.currentPageIndex = 1;
        this.pageSize = this.settings.pageSize;
        this.totalCount = 0;
        this.parameters = {};
        this.$paginationList = null;
        this.initialize();
    }
    Table.prototype.initialize = function () {
        this.buildDefaultSortingArray();
        this.createHeader();
        this.createPaginationList();
        this.$element.addClass("table-sorting");
        this.cookieKeyPrefix = this.generateCookieKeyPrefix();

        if (this.settings.initializeLoad) {
            this.reloadTable();
        }
    };
    Table.prototype.load = function (postData, completeCallback) {
        this.parameters = postData;
        this.reloadTable(completeCallback);
    }
    Table.prototype.reloadTable = function (completeCallback) {
        var _this = this;
        var parameters = {
            sorting: this.createSortParamForLoading(),
            pageIndex:this.currentPageIndex, 
            pageSize:this.pageSize
        };

        if (this.parameters) {
            parameters = $.extend(parameters, this.parameters);
        }
        this.settings.actions.listAction.beforeCallback && this.settings.actions.listAction.beforeCallback.call(this);
        this.settings.actions.listAction.method(parameters).success(function (result) {
            _this.settings.actions.listAction.callback(result) && _this.settings.actions.listAction.callback.call(_this);

            if (_this.settings.actions.listAction.getTotalCountFromResult) {
                _this.totalCount = _this.settings.actions.listAction.getTotalCountFromResult(result);
            }
            else {
                _this.totalCount = result.totalCount
            }
            _this.createPagination.call(_this);

            //Call complete callback
            if (completeCallback) {
                completeCallback.call(this,result);
            }
        });
    };
    Table.prototype.buildDefaultSortingArray = function () {
        var self = this;
        $.each(self.settings.defaultSorting.split(","), function (orderIndex, orderValue) {

            $.each(self.settings.fields, function (fieldName, fieldProps) {

                if (fieldProps.sorting) {
                    var colOffset = orderValue.indexOf(fieldName);

                    if (colOffset > -1) {

                        if (orderValue.toUpperCase().indexOf(' DESC', colOffset) > -1) {
                            self.lastSorting.push({
                                fieldName: fieldName,
                                sortOrder: 'DESC'
                            });
                        }
                        else {
                            self.lastSorting.push({
                                fieldName: fieldName,
                                sortOrder: 'ASC'
                            });
                        }
                    }
                }
            });
        });
    };
    Table.prototype.createHeader = function () {
        var $ths = this.$element.find("thead th");
        var index = 0;

        for (var i in this.settings.fields) {

            if (!this.settings.fields[i].disabled) {
                this.makeColumnSortable($ths.eq(index), i);
            }
            index++;
        }
    };
    Table.prototype.makeColumnSortable = function ($columnHeader, fieldName) {
        var self = this;

        $columnHeader.data("fieldName", fieldName).addClass('table-column-header-sortable').click(function (e) {
            e.preventDefault();

            if (!self.settings.multiSorting || !e.ctrlKey) {
                self.lastSorting = []; //clear previous sorting
            }
            self.sortTableByColumn($columnHeader);
        });

        //Set default sorting
        $.each(this.lastSorting, function (sortIndex, sortField) {

            if (sortField.fieldName == fieldName) {

                if (sortField.sortOrder == 'DESC') {
                    $columnHeader.addClass('table-column-header-sorted-desc');
                }
                else {
                    $columnHeader.addClass('table-column-header-sorted-asc');
                }
            }
        });
    };
    Table.prototype.sortTableByColumn = function ($columnHeader) {

        //Remove sorting styles from all columns except this one
        if (this.lastSorting.length == 0) {
            $columnHeader.siblings().removeClass('table-column-header-sorted-asc table-column-header-sorted-desc');
        }

        //If current sorting list includes this column, remove it from the list
        for (var i = 0; i < this.lastSorting.length; i++) {

            if (this.lastSorting[i].fieldName == $columnHeader.data('fieldName')) {
                this.lastSorting.splice(i--, 1);
            }
        }

        //Sort ASC or DESC according to current sorting state
        if ($columnHeader.hasClass('table-column-header-sorted-asc')) {
            $columnHeader.removeClass('table-column-header-sorted-asc').addClass('table-column-header-sorted-desc');
            this.lastSorting.push({
                'fieldName': $columnHeader.data('fieldName'),
                sortOrder: 'DESC'
            });
        }
        else {
            $columnHeader.removeClass('table-column-header-sorted-desc').addClass('table-column-header-sorted-asc');
            this.lastSorting.push({
                'fieldName': $columnHeader.data('fieldName'),
                sortOrder: 'ASC'
            });
        }

        //Load current page again
        this.reloadTable();
    };
    Table.prototype.addSortParamToUrl = function (url) {

        if (!this.settings.sorting || this.lastSorting.length == 0) {
            return url;
        }
        var sorting = [];

        $.each(this.lastSorting, function (idx, value) {
            sorting.push(value.fieldName + ' ' + value.sortOrder);
        });
        return (url + (url.indexOf('?') < 0 ? '?' : '&') + 'sorting=' + sorting.join(","));
    };
    Table.prototype.createSortParamForLoading = function () {
        var sortParam='';

        if (this.settings.sorting && this.lastSorting.length) {
            var sorting = [];

            $.each(this.lastSorting, function (idx, value) {
                sorting.push(value.fieldName + ' ' + value.sortOrder);
            });
            sortParam = sorting.join(",");
        }
        return sortParam;
    };

    Table.prototype.simpleHash = function (value) {
        var hash = 0;

        if (value.length == 0) {
            return hash;
        }
        for (var i = 0; i < value.length; i++) {
            var ch = value.charCodeAt(i);
            hash = ((hash << 5) - hash) + ch;
            hash = hash & hash;
        }
        return hash;
    };
    /* Generates a hash key to be prefix for all cookies for this jtable instance.
    *************************************************************************/
    Table.prototype.generateCookieKeyPrefix=function () {
        var strToHash = '';

        if (this.settings.tableId) {
            strToHash = strToHash + this.options.tableId + '#';
        }
        strToHash = strToHash + this.$element.find('thead th').length;
        return 'table#' + this.simpleHash(strToHash);
    };
    Table.prototype.setCookie = function (key, value) {
        key = this.cookieKeyPrefix + key;
        var expireDate = new Date();
        expireDate.setDate(expireDate.getDate() + 30);
        document.cookie = encodeURIComponent(key) + '=' + encodeURIComponent(value) + "; expires=" + expireDate.toUTCString();
    };
    Table.prototype.getCookie= function (key) {
        key = this.cookieKeyPrefix + key;
        var equalities = document.cookie.split('; ');

        for (var i = 0; i < equalities.length; i++) {

            if (!equalities[i]) {
                continue;
            }
            var splitted = equalities[i].split('=');

            if (splitted.length != 2) {
                continue;
            }

            if (decodeURIComponent(splitted[0]) === key) {
                return decodeURIComponent(splitted[1] || '');
            }
        }
        return null;
    }

    Table.prototype.createPaginationList = function () {
        var $paginationContainer = $("<div>").addClass("pagination-container");
        $tableResponsive = this.$element.closest(".responsive-table-container");

        if ($tableResponsive.length) {
            $paginationContainer.insertAfter($tableResponsive);
        }
        else {
            $paginationContainer.insertAfter(this.$element);
        }
        this.$paginationList = $("<ul>").addClass("pagination").appendTo($paginationContainer);
    };
    Table.prototype.savePagingSettings = function () {

        if (!this.settings.saveUserPreferences) {
            return;
        }
        this.setCookie('pageSize', this.settings.pageSize);
    };
    Table.prototype.calculatePageCount = function () {
        return Math.ceil(this.totalCount / this.pageSize);
    };
    Table.prototype.changePageSize = function (pageSize) {

        if (pageSize == this.settings.pageSize) {
            return;
        }
        this.settings.pageSize = pageSize;
        var pageCount = this.calculatePageCount();

        if (this.currentPageIndex > pageCount) {
            this.currentPageIndex = pageCount;
        }

        if (this.currentPageIndex <= 0) {
            this.currentPageIndex = 1;
        }
        this.savePagingSettings();
        this.reloadTable();
    };
    Table.prototype.createPagination = function () {
        this.$paginationList.empty();

        var pageCount = this.calculatePageCount();

        if (pageCount <= 1) {
            return;
        }
        var previousPageNo = normalizeNumber(this.currentPageIndex - 1, 1, pageCount, 1);
        var nextPageNo = normalizeNumber(this.currentPageIndex + 1, 1, pageCount, 1);

        var startIndex=normalizeNumber(this.currentPageIndex-this.settings.pageLength, 1, pageCount, 1);
        var endIndex=normalizeNumber(this.currentPageIndex+this.settings.pageLength, 1, pageCount, 1);

        this.createFirstPaginationItem();
        this.createPrePaginationItem();

        for (var i = startIndex; i <=endIndex; i++) {
            this.createPaginationItem(i);
        }
        this.createNextPaginationItem(pageCount);
        this.createLastPaginationItem(pageCount);
    };
    Table.prototype.createFirstPaginationItem = function () {
        var self = this;
        var $paginationItem = $('<li></li>').append($("<a>").text("<<")).appendTo(this.$paginationList);

        if (this.currentPageIndex== 1) {
            $paginationItem.addClass("disabled");
        }
        else {
            $paginationItem.click(function (e) {
                e.preventDefault();
                self.changePage(1);
            });
        }
    };
    Table.prototype.createPrePaginationItem = function () {
        var self = this;
        var $paginationItem = $('<li></li>').append($("<a>").text("<")).appendTo(this.$paginationList);

        if (self.currentPageIndex == 1) {
            $paginationItem.addClass("disabled");
        }
        else {
            $paginationItem.click(function (e) {
                e.preventDefault();
                self.changePage(self.currentPageIndex - 1);
            });
        }
    };
    Table.prototype.createPaginationItem = function (pageIndex) {
        var self = this;
        var $paginationItem = $('<li></li>').append($("<a>").text(pageIndex))
            .data('pageIndex', pageIndex)
            .appendTo(this.$paginationList)
            .click(function (e) {
                e.preventDefault();
                self.changePage($(this).data('pageIndex'));
            });

        if (this.currentPageIndex == pageIndex) {
            $paginationItem.addClass('current');
        }
    };
    Table.prototype.createNextPaginationItem = function (pageCount) {
        var self = this;
        var $paginationItem = $('<li></li>').append($("<a>").text(">")).appendTo(this.$paginationList);

        if (self.currentPageIndex >= pageCount) {
            $paginationItem.addClass("disabled");
        }
        else {
            $paginationItem.click(function (e) {
                e.preventDefault();
                self.changePage(self.currentPageIndex + 1);
            });
        }
    };
    Table.prototype.createLastPaginationItem = function (pageCount) {
        var self = this;
        var $paginationItem = $('<li></li>').append($("<a>").text(">>")).appendTo(this.$paginationList);

        if (self.currentPageIndex >= pageCount) {
            $paginationItem.addClass("disabled");
        }
        else {
            $paginationItem.click(function (e) {
                e.preventDefault();
                self.changePage(pageCount);
            });
        }
    };
   
    Table.prototype.calculatePageNumbers = function (pageCount) {

        if (pageCount <= 4) {
            var pageNumbers = [];

            for (var i = 1; i <= pageCount; ++i) {
                pageNumbers.push(i);
            }
            return pageNumbers;
        }
        else {
            var shownPageNumbers = [1, 2, pageCount - 1, pageCount];
            var previousPageNo = normalizeNumber(this.currentPageIndex - 1, 1, pageCount, 1);
            var nextPageNo = normalizeNumber(this.currentPageIndex + 1, 1, pageCount, 1);

            this.insertToArrayIfDoesNotExists(shownPageNumbers, previousPageNo);
            this.insertToArrayIfDoesNotExists(shownPageNumbers, this.currentPageIndex);
            this.insertToArrayIfDoesNotExists(shownPageNumbers, nextPageNo);

            shownPageNumbers.sort(function (a, b) { return a - b; });
            return shownPageNumbers;
        }
    };
    Table.prototype.changePage=function (pageIndex) {
        pageIndex = normalizeNumber(pageIndex, 1, this.calculatePageCount(), 1);

        if (pageIndex == this.currentPageIndex) {
            this.refreshGotoPageInput && this.refreshGotoPageInput();
            return;
        }
        this.currentPageIndex = pageIndex;
        this.reloadTable();
    };

    $.fn.table = function (options) {
        this.eq(0).data("table") || this.eq(0).data("table", new Table(this.eq(0),options));
        return this.eq(0).data("table");
    };
})(jQuery);