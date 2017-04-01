/*
 * nofity 0.1
 * Copyright (c) 2015 Canknow http://www.canknow.com/
 * Date: 2015-07-30
 * nofity
 */

(function($){
    function Countdown(element, options) {
        var _this = this;
        this.duringtime = 0;
        this.$element = $(element);
        this.settings = $.extend({}, this.defaultSettings, options);
        this.settings.endtime = this.settings.endtime || element.dataset.optionEndtime;
        this.elements = {
            hm: this.$element.find(".hm"),
            second: this.$element.find(".second"),
            minite: this.$element.find(".minite"),
            hour: this.$element.find(".hour"),
            day: this.$element.find(".day"),
            month: this.$element.find(".month"),
            year: this.$element.find(".year")
        };
        this.tip();
    };
    Countdown.prototype.constructor = Countdown;
    Countdown.prototype.defaultSettings = {
        end: function () { }
    };
    Countdown.prototype.microsecond=function(n){
        if(n < 10)
            return "00" + n.toString();
        if(n < 100)
            return "0" + n.toString();
        return n.toString();
    };
    Countdown.prototype.zero=function(n){
        var _n = parseInt(n, 10);//解析字符串,返回整数

        if (_n > 0) {

            if(_n <= 9){
                _n = "0" + _n
            }
            return String(_n);
        }
        else{
            return "00";
        }
    };
    Countdown.prototype.getDuring=function(){
        var now = new Date(),
            endDate = new Date(this.settings.endtime);
        //现在将来秒差值
        //alert(future.getTimezoneOffset());
        var during = (endDate - now.getTime()) / 1000 ;
        this.duringtime = endDate - now.getTime() ;
        var during_formate = {
                hm:"000",
                second: "00",
                minite: "00",
                hour: "00",
                day: "00",
                month: "00",
                year: "0"
            };
        if(this.duringtime > 0){
            during_formate.hm = this.microsecond(this.duringtime % 1000);
            during_formate.second = this.zero(during % 60);
            during_formate.minite = Math.floor((during / 60)) > 0? this.zero(Math.floor((during / 60)) % 60) : "00";
            during_formate.hour = Math.floor((during / 3600)) > 0? this.zero(Math.floor((during / 3600)) % 24) : "00";
            during_formate.day = Math.floor((during / 86400)) > 0? this.zero(Math.floor((during / 86400)) % 30) : "00";
            //月份，以实际平均每月秒数计算
            during_formate.month = Math.floor((during / 2629744)) > 0? this.zero(Math.floor((during / 2629744)) % 12) : "00";
            //年份，按按回归年365天5时48分46秒算
            during_formate.year = Math.floor((during / 31556926)) > 0? Math.floor((during / 31556926)) : "0";
        }
        else{
            during_formate.year=during_formate.month=during_formate.day=during_formate.hour=during_formate.minite=during_formate.second="00";
            during_formate.hm = "000";
        }

        if(this.duringtime==0){
            this.settings.end();
        }
        return during_formate;
    };
    Countdown.prototype.tip=function(){
        var _this=this;
        var during=this.getDuring();
        var draw=this.settings.draw||this.draw;
        draw.call(this,during);
        setTimeout(function(){
            _this.tip();
        }, 1);
    };
    Countdown.prototype.draw = function (during) {

        if (this.elements.hm) {
            this.elements.hm.text(during.hm);
        }
        if (this.elements.second) {
            this.elements.second.text(during.second);
        }
        if (this.elements.minite) {
            this.elements.minite.text(during.minite);
        }
        if (this.elements.hour) {
            this.elements.hour.text(during.hour);
        }
        if (this.elements.day) {
            this.elements.day.text(during.day);
        }
        if (this.elements.month) {
            this.elements.month.text(during.month);
        }
        if (this.elements.year) {
            this.elements.year.text(during.year);
        }
    };

    $.fn.countdown=function(options){
        return new Countdown(this.get(0),options);
    };
})(jQuery);