(function($) {
    "use strict";
    function Switcher($element, options) {
        var self = this;
        this.$element = $element;
    }
    Switcher.prototype.initialize = function () {

    };
    $.fn.switcher = function(options){
        var default_settings = {

        };
        var settings = $.extend({}, default_settings, options);
        this.eq(0).data("switcher") || this.eq(0).data("switcher", new Switcher(this.eq(0),options));
        return this.eq(0).data("switcher");
    };
})(jQuery);