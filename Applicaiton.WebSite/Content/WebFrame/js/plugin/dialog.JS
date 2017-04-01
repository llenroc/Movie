!(function(window,$){
    'use strict';
    function Dialog(options, $element) {
        var _this = this;
        this.settings = $.extend({}, Dialog.defaultSettings, options);

        if ($element) {
            this.$element = $element;
        }
        this.initialize();
    };
    Dialog.LifeCycle = {
        Transient: 0,
        Singleton: 1
    };
    Dialog.defaultSettings = {
        title: "Dialog title",
        content: "",
        fullPage: false,
        responsive: false,
        closeButtonText: "close",
        okButtonText: "Ok",
        onInitialize:function(){},
        onOk: function () { return true;},
        onHide: function () { },
        lifeCycle: Dialog.LifeCycle.Singleton
    };
    Dialog.prototype.initialize = function () {
        this.settings.onInitialize.call(this);
        this.initializeID();
        var containerString = "container";
        containerString += this.settings.responsive ? "-responsive" : "";

        if (!this.$element) {
            this.$element = $('<div class="dialog">\
            <div class="header"><div class="' + containerString + '">\
            <h5 class="title">'+ this.settings.title + '</h5>\
            <button class="button-icon button-close">×</button>\
            </div></div>\
            <div class="body"><div class="' + containerString + ' content"></div></div>\
            <div class="footer"><div class="' + containerString + '">\
            <button type="button" class="button-text button-close">' + this.settings.closeButtonText + '</button>\
            <button type="button" class="button-text button-ok">' + this.settings.okButtonText + '</button>\
            </div></div>\
            </div>').appendTo("body");
        }
        
        if (this.settings.fullPage) {
            this.$element.addClass("dialog-page");
        }

        if (this.settings.responsive) {
            this.$element.addClass("dialog-responsive").find(".container").addClass("container-responsive");
        }
        this.initializeContent();
        this.bindEvent();
    };
    Dialog.prototype.initializeID = function () {

        if (!this.settings.id) {
            this.id = 'Dialog_' + (Math.floor((Math.random() * 1000000))) + new Date().getTime();
        }
        else {
            this.id = this.settings.id;
        }
        return this.id;
    };
    Dialog.prototype.bindEvent = function () {
        var self = this;

        this.$element.find(".button-close").click(function () {
            self.hide();
        });

        this.$element.find(".button-ok").click(function () {
            self.ok.call(self);
        });

        if (this.$element[0].id) {
            $('[data-target=' + this.$element[0].id + ']').each(function () {

                $(this).on("click.dialog.toggle", function () {
                    $(this.dataset.target).dialog().toggle();
                })
            });
        }
    };
    Dialog.prototype.setOnOk = function (onOk) {
        this.settings.onOk = onOk;
    };
    Dialog.prototype.ok = function () {
        var self = this;

        if (self.settings.onOk.call(this)) {
            self.hide();
        }
    };
    Dialog.prototype.initializeContent = function () {
        this.$content = this.$element.find(".content");

        if (this.settings.content) {
            this.$content.html(this.settings.content);
        }
    };
    Dialog.prototype.setContent = function ($element) {

        if ($element instanceof jQuery) {
            this.$content.append($element);
        }
        else if (isString($element)) {
            this.$content.html($element);
        }
    };
    Dialog.prototype.show = function (callback) {
        $("<div>").appendTo("body").addClass("mask-layer").hide().fadeIn(300);
        this.$element.css({
            top: ($(window).height() - this.$element.outerHeight()) / 2
        }).addClass("show").removeClass("hide");

        if (callback) {
            callback.apply(this);
        }
    };
    Dialog.prototype.hide = function (callback) {
        var _this = this;
        this.$element.addClass("hide").removeClass("show");
        $(".mask-layer").fadeOut(300, function () {

            if (callback) {
                callback.apply(_this);
            }
            _this.settings.onHide.call(_this);

            if (_this.settings.lifeCycle == Dialog.LifeCycle.Transient) {
                _this.remove();
                $(this).remove();
            }
        })
    };
    Dialog.prototype.remove = function () {
        this.$element.remove();
        this.dispose();
    };
    Dialog.prototype.toggle = function () {
        this.$element.hasClass("show") ? this.hide() : this.show()
    };
    Dialog.prototype.dispose = function () {

    };
    window.Dialog = Dialog;
    $.fn.dialog = function (options) {
        var $this = this;
        this.eq(0).data("dialog") || this.eq(0).data("dialog", new Dialog(options, this.eq(0)));
        return this.eq(0).data("dialog");
    };
    $.dialog=function(options,$element){
        return new Dialog(options, $element);
    }
})(window,jQuery);