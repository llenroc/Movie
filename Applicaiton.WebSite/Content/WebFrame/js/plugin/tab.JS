!(function(window,$,undefined){
    'use strict';

    var default_settings={

    };

    $.fn.tab=function(options){
        var settings= $.extend({},default_settings,options);

        this.each(function(index,element){
            var $tab=$(element);
            var $tabNavList =$tab.find(".tab-nav-list");
            var $tabBox =$tab.find(".tab-box");
            var $tabPages =$tabBox.find(".tab-page");
            var $tabNavLis = $tab.find("li");

            $tabNavLis.click(function(){
                var $tabNavLi=$(this);
                $tabNavLis.filter(".actived").removeClass("actived");
                $tabNavLi.addClass("actived");

                $tabPages.filter(".show").removeClass("show").addClass("hide");

                var $toTabPage = this.dataset["target"] ? $(document.getElementById(this.dataset["target"])) : $tabPages.eq($tabNavLi.index());
                $toTabPage.removeClass("hide").addClass("show");
            })
        })
    }
})(window,jQuery);