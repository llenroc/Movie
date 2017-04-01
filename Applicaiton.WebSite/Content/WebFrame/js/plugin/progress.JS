!(function(window,$,undefined){
    'use strict';
    function Progress(options, $element) {
        this.$element = $element;
        this.settings = $.extend({}, Progress.defaultSettings, options);
        this.initialize();
    };
    Progress.defaultSettings = {
        showValue: true,
        autoHide:false,
        progress: function () { },
    };
    Progress.prototype.initialize = function () {
        this.$element.addClass("progress");
        this.$indicator = this.$element.find(".indicator");

        if (!this.$indicator.length) {
            this.$indicator = $("<div>").addClass("indicator").appendTo(this.$element);
        }

        if (this.settings.autoHide) {
            this.$element.addClass("progress-hide");
        }
    };
    Progress.prototype.show = function () {
        this.$element.addClass("show");
    };
    Progress.prototype.hide = function () {
        this.$element.removeClass("show");
    };
    Progress.prototype.setValue = function (value) {
        var percententValue = value + "%";
        this.$indicator.width(percententValue);

        if (this.settings.showValue) {
            this.$indicator.text(percententValue);
        }
    };

    $.fn.progress = function (options) {
        this.eq(0).data("progress") || this.eq(0).data("progress", new Progress(options, this.eq(0)));
        return this.eq(0).data("progress");
    };
})(window,jQuery);