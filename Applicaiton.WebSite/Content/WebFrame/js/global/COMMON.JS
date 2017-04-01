(function(window, undefined){
    'use strict';

    function startPlugins(domElement) {

        if (domElement.dataset && domElement.dataset["ride"]) {
            processElement(domElement);
        }
        //plugin init
        if(domElement.nodeType==1){
            var pluginElements = domElement.querySelectorAll("[data-ride]");

            for (var i = 0; i < pluginElements.length; i++) {
                var pluginElement = pluginElements[i];
                processElement(pluginElement);
            }
        }
    }

    function processElement(pluginElement) {
        var pluginNames = pluginElement.dataset["ride"].trim().split(" ");

        pluginNames.forEach(function (pluginName) {

            if (!pluginElement[pluginName] && $.fn[pluginName]) {
                $(pluginElement)[pluginName]();
            }
        })
    }

    window.addEventListener("load", function () {

        try{
            //prettyPrint();
        }
        catch(ex){
            console.log(ex.message);
        }
        startPlugins(document.body);

        document.addEventListener("DOMNodeInserted", function (e) {
            startPlugins(e.target);
        });
    });
})(window);