!(function (window, $, undefined) {
    'use strict';
    var WebkitNotify = {};
    WebkitNotify.initialize = function () {
        if (Notification && Notification.permission !== "granted") {
            Notification.requestPermission(function (status) {

                if (Notification.permission !== status) {
                    Notification.permission = status;
                }
            });
        }
    }
    WebkitNotify.notification = function (settings) {
        var config = $.extend({}, WebkitNotify.defaultSettings, settings);
        var options = {
            dir: "ltr",
            lang: "utf-8",
            icon: config.icon,
            body: config.body
        };

        if (Notification && Notification.permission === "granted") {
            var notification = new Notification(settings.title, options);
            notification.onclick = config.onClick;
        }
        else if (Notification && Notification.permission !== "denied") {
            Notification.requestPermission(function (status) {

                if (Notification.permission !== status) {
                    Notification.permission = status;
                }

                if (status === "granted") {
                    var notification = new Notification(settings.title, options);
                    notification.onclick = config.onClick;
                }
            });
        }
    };
    WebkitNotify.defaultSettings = {
        icon: "/Content/WebFrame/image/notificationIcon.png",
        title: "Notification Title",
        body: "Notification Body",
        onClick:function(){}
    };
    window.WebkitNotify = WebkitNotify;
})(window, jQuery);