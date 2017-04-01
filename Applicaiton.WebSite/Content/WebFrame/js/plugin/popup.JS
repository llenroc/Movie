!(function(window,$,undefined){
    'use strict';
    var globalShowingPopup = null;
    function Popup(options){
        var _self=this;
        this.$element =null;
        this.$content = null;
        this.settings = $.extend({}, this.defaultSettings, options);
        this.initialize();
  
        if (this.settings.$content) {
            this.$content = this.setConent(this.settings.$content);
        }
    }
    Popup.prototype.defaultSettings = {
        $target: null,
        $content: null,
        show:true,
        onShow: function () {

        },
        onHide: function () {

        }
    };
    Popup.prototype.initialize = function () {
        var _this = this;
        this.$element = $("<div class='popup'>").appendTo(document.body);
        
        if (!this.settings.show) {
            this.$element.hide();
        }

        if (this.settings.$target) {
            var left = this.settings.$target.offset().left;
            var top = this.settings.$target.offset().top;
            var divOffset = { left: left, top: top };
            divOffset.top += this.settings.$target.height();
            this.$element.css(divOffset).show();
        }

        $(document).on("click", function (event) {

        });

        this.$element.click(function (event) {
            event.stopPropagation();
        });

        if (globalShowingPopup) {
            globalShowingPopup.remove();
        }
        globalShowingPopup = this;
    }
    Popup.prototype.show = function () {
        this.$element.show();
    }
    Popup.prototype.hide = function () {
        this.$element.hide();
        this.settings.onHide.call(this);
    }
    Popup.prototype.setConent = function ($element) {
        return this.$content = $element.appendTo(this.$element);
    };
    Popup.prototype.remove = function () {
        this.$element.remove();
    };

    $.popup=function(options){
        return new Popup(options);
    };
})(window,jQuery);