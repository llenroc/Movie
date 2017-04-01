(function () {
    $.fn.turnOffLight = function () {
        var lightOverlay = $("<div>").addClass("light-overlay").appendTo(document.body);
        this.addClass("light-target");
    };
    $.fn.turnOnLight = function () {
        $(".light-overlay").remove();
        this.removeClass("light-target");
    };
})();