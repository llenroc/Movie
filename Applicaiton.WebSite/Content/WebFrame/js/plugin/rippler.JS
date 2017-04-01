(function($) {
    "use strict";
    function diagonal(x, y) {

        if (x > 0 && y > 0)
            return Math.sqrt(Math.pow(x, 2) + Math.pow(y, 2));
        else
            return false;
    }

    $.fn.rippler = function(options){
        var default_settings={
            duration: 600
        };

        var settings= $.extend(default_settings,options);

        this.each(function(index,element){
            var $this = $(this);
            $this.on("mousedown", function(event) {
                var offset = $this.offset();
                var left=event.pageX-offset.left;
                var top=event.pageY-offset.top;
                var $ripplerPointer=$("<div></div>").appendTo($this).addClass("rippler-pointer").css({
                    left:left,
                    top:top
                });
            });
            $this.on("mouseup", function(event) {
                var offset = $this.offset();
                var left=event.pageX-offset.left;
                var top=event.pageY-offset.top;
                var thisW = $this.outerWidth();
                var thisH = $this.outerHeight();
                var effectMaxWidth = diagonal(thisW, thisH) * 2;

                var $ripplerPointer= $this.find(".rippler-pointer").css({
                    'width':effectMaxWidth,
                    'height':effectMaxWidth,
                    'left': left - ( effectMaxWidth / 2 ),
                    'top': top - ( effectMaxWidth / 2 ),
                    'opacity': 0,
                    'transition':'all '+settings.duration/1000+'s ease-out'
                });
                setTimeout(function(){
                    $ripplerPointer.remove();
                }, settings.duration);
            });
        })
    };

})(jQuery);