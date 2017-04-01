!(function ($) {
    'use strict';

    function Promte(text, defaultText, callback) {
        var self = this;
        this.$dialog = $.dialog({
            title: "Promte",
            responsive: true,
            content: "",
            closeButtonText: "Close",
            okButtonText: "Ok",
            onOk: function () { },
        });
        var $content = $("<div class=''>").append($("<h3>").text(text));
        var $input = $("<input type='text' value='" + defaultText + "'>").appendTo($content).wrap($("<div class='input-wrapper'>"));
        this.$dialog.setContent($content);
        this.$dialog.settings.onOk = function () {

            if ($input.val() && callback) {
                callback.call(self, $input.val());
            }
            return true;
        }
        this.$dialog.show();
    }

    function Confirm(text, callback) {
        var self = this;
        this.$dialog = $.dialog({
            title: "Confirm",
            responsive: true,
            content: "",
            closeButtonText: "Close",
            okButtonText: "Ok",
            onOk: function () { },
        });
        var $content = $("<div class=''>").append($("<h3>").text(text));
        this.$dialog.setContent($content);
        this.$dialog.settings.onOk = function () {

            if (callback) {
                callback.call(self);
            }
            return true;
        }
        this.$dialog.show();
    }

    $.promte = function (text, defaultText, callback) {
        return new Promte(text, defaultText, callback);
    }
    $.confirm = function (text, callback) {
        return new Confirm(text, callback);
    }
})(jQuery);