(function () {
    $.blockUI = function () {
        document.body.classList.add("mask");
    };
    $.unblockUI = function () {
        document.body.classList.remove("mask");
    };
    $.fn.blockUI = function () {
        this.addClass("mask");
    };
    $.fn.unblock = function () {
        this.removeClass("mask");
    };
})();