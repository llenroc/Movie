!(function(window,$,undefined){
    'use strict';

    $.fn.treenode=function(options){
        var default_settings={

        };

        var settings= $.extend({},default_settings,options);

        this.each(function(index,element){
            var $treenode = $(element);

            $treenode.find(".title").click(function () {
                $(this).closest("li").toggleClass("actived").children("ul").slideToggle();
            });

        })
    }
})(window,jQuery);