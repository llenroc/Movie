!(function(window,$,undefined){
    'use strict';
    $.fn.scrollTopNav = function (options) {
        var default_settings={
            height:500
        };
        var settings = $.extend({}, default_settings, options);
        var winHeight = $(document).scrollTop();
  
        this.each(function(index,element){
            var $nav = $(element);
            var height = settings.height;

            if (element.dataset["optionHeight"]) {
                height = element.dataset["optionHeight"];
            }
            $(window).scroll(function () {
                var scrollY = $(document).scrollTop();// 获取垂直滚动的距离，即滚动了多少

                if (scrollY > height) { //如果滚动距离大于550px则隐藏，否则删除隐藏类
                    $nav.addClass('top');
                }
                else {
                    $nav.removeClass('top');
                }
            });
        })
    }
})(window,jQuery);