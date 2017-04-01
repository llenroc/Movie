!(function(window,$,undefined){
    'use strict';
    function CodeHighlight(options,$element) {
        var code="";
        var $pre;
        var $codeBody;

        if (options.code) {
            code = options.code;
        }
        else {
            code = $element.html();
        }
        var result = hljs.highlightAuto(code);
        var lines = result.value.split('\n');
        var language = result.language;

        if ($element.get(0).tagName=="PRE") {
            $element.empty();
            $pre = $element;
            $pre.wrap("<div class='code-body'>");
            $pre.parent().wrap("<div class='code-view'>");
            $codeBody = $pre.parent();
            this.$element = $pre.parent().parent();
        }
        else {
            this.$element = $element;
            this.$element.addClass("code-view");
            $codeBody=$("<div class='code-body'>");
            this.$element.append($codeBody);
            $pre = $("<pre>").appendTo($codeBody);
        }
        var $codeHeader = $("<div class='code-header'><label class='language'>" + language + "</label></div>").insertBefore($codeBody);
        var $preNumbering = $("<ul class='pre-numbering'>").insertBefore($pre);
        var $codeList = $("<ul class='code-list'>").appendTo($pre); 
        var $copyButton=$("<img>")
        lines.forEach(function (value, i) {
            var $countLi = $("<li>").text(i).appendTo($preNumbering);
            var $codeLi = $("<li>").html(value).appendTo($codeList);
        });
    }
    $.fn.codeHighlight=function(options){
        var default_settings={
            code:null
        };
        var settings = $.extend({}, default_settings, options);
        this.each(function (index, element) {
            var $this = $(this);
            $this.data("codeHighlight") || $this.data("codeHighlight", new CodeHighlight(settings, $this));
        });
    }
})(window,jQuery);