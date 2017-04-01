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
                var scrollY = $(document).scrollTop();// ��ȡ��ֱ�����ľ��룬�������˶���

                if (scrollY > height) { //��������������550px�����أ�����ɾ��������
                    $nav.addClass('top');
                }
                else {
                    $nav.removeClass('top');
                }
            });
        })
    }
})(window,jQuery);