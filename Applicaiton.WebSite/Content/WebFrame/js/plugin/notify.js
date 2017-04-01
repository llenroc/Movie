/*
 * nofity 0.1
 * Copyright (c) 2015 Canknow http://www.canknow.com/
 * Date: 2015-07-30
 * nofity
 */
(function($){
    function Notify(message, options) {
        var _this = this;
        this.settings = $.extend({}, this.defaultSettings, options);

        $notifyBox = $(".notify-box");

        if (!$notifyBox.length) {
            $notifyBox = $('<div class="notify-box">').appendTo("body");
        }
        var title_html = this.settings.title ? '<h5 class="title">' + this.settings.title + '</h5>' : "";
        var message_html = message ? '<p class="content">' + message + '</p>' : "";

        this.$element = $('<div class="notify ' + this.settings.type + '">\
        <span class="icon '+ this.settings.type + '"></span>\
            <div class="body">'+ title_html + '\
                <p class="content">'+ message + '</p>\
            </div>\
        </div>').appendTo($notifyBox).css({
            left: '100%'
        }).animate({
            left: '0'
        }, 500);

        if (this.settings.autoHide) {
            setTimeout(function () {
                _this.hide();
            }, this.settings.timeShow);
        }
    }
    Notify.prototype.constructor = Notify;
    Notify.prototype.defaultSettings = {
        type: "info",
        autoHide: true,
        timeShow: 5000
    };
    Notify.prototype.hide = function () {
        this.$element.animate({
            left: '-100%',
            opacity: 0,
            height: 0
        }, 500, function () {
            $(this).remove();
        })
    };

    $.notify = function (message, options) {
        return new Notify(message, options);
    };
    $.notify.success = function (message, options) {
        var options = $.extend({ type: 'success' }, options);
        $.notify(message, options);
    };
    $.notify.info = function (message, options) {
        var options = $.extend({ type: 'info' }, options);
        $.notify(message, options);
    };
    $.notify.warn = function (message, options) {
        var options = $.extend({ type: 'warn' }, options);
        $.notify(message, options);
    };
    $.notify.error = function (message, options) {
        var options = $.extend({ type: 'error' }, options);
        $.notify(message, options);
    };
})(jQuery)