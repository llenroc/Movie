(function () {
    $.fn.busy = function () {
        return this.each(function () {
            var $this = $(this);
            $this.addClass("busy-target");
            var $busyLayer = $("<div>").addClass("busy-layer show").appendTo($this);
            var $busyLoading = $("<div>").addClass("busy-loading").appendTo($busyLayer);
        });
    };
    $.fn.clearBusy = function () {
        return this.each(function () {
            var $this = $(this);
            $this.find(".busy-loading").remove();
            $this.find(".busy-layer").remove();
        });
    };

    $.busy = function () {
        $(document.body).busy();
    };
    $.clearBusy = function () {
        $(document.body).clearBusy();
    };
})();