!(function(window,$,undefined){
    'use strict';

    function getByteToM(val) {

        if (isNaN(val))
            return val;
        val = val / (1024 * 1024);
        val = Math.round(val * 100) / 100;
        return val;
    }
    function Uploader(options, $element) {
        var _this=this;
        var default_settings = {
            url: '',
            progress: function () { },
            load: function () { },
            error: function () { },
            abort: function () { },
            success: function () {

            }
        };
        this.$element = $element;
        this.data = {};
        this.settings = $.extend({}, default_settings, options);

        if (!this.settings.url) {
            this.settings.url = $element.get(0).dataset.url;
        }
        this.addData = function (key,value) {
            _this.data[key] = value;
        }
        this.$element.find("input[type=file]").change(function (e) {
            var file = this.files[0];
            var formData = new FormData();
            formData.append("file", file);

            for (var i in _this.data) {
                formData.append(i, _this.data[i]);
            }
            var xhr = new XMLHttpRequest();
            xhr.open('POST', _this.settings.url, true);
            xhr.upload.addEventListener("progress", _this.settings.progress, false);
            xhr.addEventListener("load", _this.settings.load, false);
            xhr.addEventListener("error", _this.settings.error, false);
            xhr.addEventListener("abort", _this.settings.abort, false);
            xhr.onreadystatechange = function () {

                if (xhr.readyState == 4) {

                    if (xhr.status == 200) {
                        var result = JSON.parse(xhr.responseText);
                        _this.settings.success(result);
                    }
                }
            }
            xhr.send(formData);
        });
    }
    $.fn.uploader=function(options){
        var default_settings={
            url: '',
            progress: function () { },
            load: function () { },
            error: function () { },
            abort: function () { }
        };
        this.eq(0).data("uploader") || this.eq(0).data("uploader", new Uploader(options, this.eq(0)));
        return this.eq(0).data("uploader");
    }
})(window,jQuery);