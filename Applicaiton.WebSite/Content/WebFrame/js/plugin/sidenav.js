!(function(window,$,undefined){
    'use strict';

    function SideNav($element, options) {
        var _self = this;
        this.$element = $element;
        this.element = $element[0];
        this.settings = $.extend({}, this.defaultSettings, options);
        this.menus = {};
        this.currentMenu = null;

        this.initialize();
    }
    SideNav.prototype.defaultSettings = {
        menu: null,
        enableDefaultHandler: true,
        userUiSref:false
    };
    SideNav.prototype.initialize = function () {

        if (this.settings.data) {
            this.createItems(this.$element, this.settings.data);
        }
        else {
            this.bindEvent();
        }
    };
    SideNav.prototype.createItems = function ($parent, itemsData) {
        var self = this;

        itemsData.forEach(function (value) {
            self.createItem.call(self,$parent, value);
        });
    }
    SideNav.prototype.createItem = function ($parent, itemData) {
        var self = this;
        var $li = $("<li>").appendTo($parent);

        if (itemData.items && itemData.items.length) {
            $li.addClass("has-child");
            var $title = $("<h5>").addClass("title").text(itemData.displayName).appendTo($li).on("click", self.toggleClickHandler);
            var $ul = $("<ul>").appendTo($li);
            self.createItems($ul, itemData.items);
            $title.trigger("click");
        }
        else {
            var $titleBlock = $("<div>").addClass("title-block").appendTo($li);
            var $icon = $("<i>").addClass(itemData.icon).appendTo($titleBlock);
            var hrefAttr = this.settings.userUiSref ? "ui-sref" : "href";
            var $title = $("<a>").addClass("title").attr(hrefAttr, itemData.url).text(itemData.displayName).appendTo($titleBlock);

            if (this.settings.enableDefaultHandler) {
                $title.on("click", self.linkClickHandler);
            }
        }
        this.menus[itemData.name] = {
            data: itemData,
            $li:$li
        };
    };
    SideNav.prototype.setCurrent = function (currentMenuName) {

        if (!currentMenuName || !this.menus[currentMenuName]) {
            return;
        }

        if (this.currentMenu) {
            this.currentMenu.$li.removeClass("actived");
        }
        this.menus[currentMenuName].$li.addClass("actived");
        this.currentMenu = this.menus[currentMenuName];
    };
    SideNav.prototype.bindEvent = function () {
        var self = this;

        if (this.settings.enableDefaultHandler) {
            this.$element.find("a.title").on("click", this.linkClickHandler); 
        }
        this.$element.find("h5.title").on("click", this.toggleClickHandler);
    };
    SideNav.prototype.toggleClickHandler = function (e) {
        $(this).parent("li").toggleClass("opened");
    };
    SideNav.prototype.linkClickHandler = function (e) {
        $(this).closest(".sidenav").find("li").removeClass("actived");
        $(this).closest("li").addClass("actived");
    };

    $.fn.sidenav = function (options) {
        this.eq(0).data("sidenav") || this.eq(0).data("sidenav", new SideNav(this.eq(0), options));
        return this.eq(0).data("sidenav");
    }
})(window,jQuery);