!(function (window, $, undefined) {
    'use strict';

    function PictureUploader(options, $element) {
        var _this = this;
        FileUploader.call(this, options, $element);
    };
    PictureUploader.prototype = FileUploader.prototype;
    PictureUploader.prototype.initializePreviewer = function () {
        this.image = this.element.querySelector("img");

        if (this.element.dataset["optionDefaultImage"]) {
            this.settings["defaultImage"] = this.element.dataset["optionDefaultImage"];
        }
        if (this.settings["defaultImage"] && !this.image.src) {
            this.image.src = this.settings["defaultImage"];
        }
    };
    PictureUploader.prototype.defaultSettings = {
        url: '',
        showProgress:true,
        defaultImage: null,
        progress: function (value) { },
        load: function () { },
        error: function () { },
        abort: function () { },
        success: function () {

        }
    };
    PictureUploader.prototype.setPathForPreviewer = function () {

        if (path) {
            this.image.src = path;
        }
        else {
            this.image.src = this.settings["defaultImage"];
        }
    };

    $.fn.pictureUploader = function (options) {
        var default_settings = {
            url: '',
            progress: function () { },
            load: function () { },
            error: function () { },
            abort: function () { }
        };
        this.eq(0).data("pictureUploader") || this.eq(0).data("pictureUploader", new PictureUploader(options, this.eq(0)));
        return this.eq(0).data("pictureUploader");
    };
})(window, jQuery);