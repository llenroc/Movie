!(function(window,$,undefined){
    'use strict';

    $.fn.documentList=function(options){
        var default_settings={

        };
        var settings= $.extend({},default_settings,options);
        this.each(function(index,element){
            var $documentList=$(element);
            $documentList.find(".title").click(function(){
                $(this).filter("a").closest(".document-list").find("a.title").parent("li").removeClass("actived");
                $(this).parent("li").toggleClass("actived").children("ul").slideToggle();
            });

        })
    }
})(window,jQuery);