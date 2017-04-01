/*
 * nofity 0.1
 * Copyright (c) 2015 Canknow http://www.canknow.com/
 * Date: 2015-07-30
 * nofity
 */

(function($){
    function Message(message,options){
        var _this=this;
        this.settings = $.extend({}, this.defaultSettings, options);

        var title_html=this.settings.title?'<h5 class="title">'+this.settings.title+'</h5>':"";
        var message_html=message?'<p class="content">'+message+'</p>':"";

        if(this.settings.showMask){
            $('body').addClass("mask");
        }

        this.$element=$('<div class="message '+this.settings.type+'">\
        <span class="icon '+this.settings.type+'"></span>\
            <div class="body">'+title_html+'\
                <p class="content">'+message+'</p>\
            </div>\
        </div>').appendTo("body").addClass("show");

        if(this.settings.autoHide){
            setTimeout(function(){
                _this.hide();
            },this.settings.timeShow);
        }

    }
    Message.prototype.constructor = Message;
    Message.prototype.defaultSettings = {
        type: "info",
        autoHide: true,
        showMask: false,
        timeShow: 5000
    };
    Message.prototype.hide=function(delayTime){
        var _this = this;

        if (delayTime) {
            setTimeout(function () {
                _this.doHide();
            },delayTime)
        }
        else {
            _this.doHide();
        }
        
    };
    Message.prototype.doHide = function () {
        var _this = this;

        _this.$element.removeClass("show").addClass("hide");
        setTimeout(function () {
            _this.$element.remove();
            if (_this.settings.showMask) {
                $('body').removeClass("mask");
            }
        }, 1000);
    }
    Message.prototype.loadingToSuccess = function (message, callback, delayTime) {
        this.$element.removeClass(this.settings.type).addClass("success").find(".icon").removeClass(this.settings.type).addClass("success");
        this.$element.find(".content").html(message);

        if (delayTime) {
            setTimeout(function () {
                callback();
            }, delayTime);
        }
        
    }
    Message.prototype.loadingToError = function (message, callback, delayTime) {
        this.$element.removeClass(this.settings.type).addClass("error").find(".icon").removeClass(this.settings.type).addClass("error");
        this.$element.find(".content").html(message);

        if (delayTime) {
            setTimeout(function () {
                callback();
            }, delayTime);
        }
    }

    $.message=function(message,options){
        return new Message(message,options);
    };
    $.message.success = function (message, options) {
        var options = $.extend({ type: 'success' }, options);
        return $.message(message, options);
    };
    $.message.info = function (message, options) {
        var options = $.extend({ type: 'success' }, options);
        return $.message(message, options);
    }
    $.message.warn = function (message, options) {
        var options = $.extend({ type: 'success' }, options);
        return $.message(message, options);
    };
    $.message.error = function (message, options) {
        var options = $.extend({ type: 'error' }, options);
        return $.message(message, options);
    };
    $.message.loading = function (message, options) {
        var options = $.extend({ type: 'loading', autoHide: false, showMask: true }, options);
        return $.message(message, options);
    };
})(jQuery)