!(function (window, $, undefined) {
    'use strict';

    function getByteToM(val) {

        if (isNaN(val))
            return val;
        val = val / (1024 * 1024);
        val = Math.round(val * 100) / 100;
        return val;
    };

    function FileUploader(options, $element) {
        var _this = this;
        this.$element = $element;
        this.element = $element.get(0);
        this.element.fileUploader = this;
        this.path = null;

        this.fileInput = this.element.querySelector("input[type=file]");
        this.valueInput = this.element.querySelector("input[type=hidden]");
        this.data = {};
        this.settings = $.extend({}, this.defaultSettings, options);

        if (!this.settings.url) {
            this.settings.url = this.element.dataset.optionUrl;
        }
        this.initializePreviewer();
        this.initialize();
    };
    FileUploader.prototype.addData = function (key, value) {
        this.data[key] = value;
    };
    FileUploader.prototype.initialize = function () {
        this.initializeProgress();
        this.bindEvent();
    };
    FileUploader.prototype.initializePreviewer = function () {
        this.pathViewer = this.element.querySelector("p");
    };
    FileUploader.prototype.initializeProgress = function () {
        var _this = this;

        if (this.settings.showProgress) {

            this.$progress = this.$element.find(".progress");

            if (!this.$progress.length) {
                this.$progress = $("<div>").appendTo(this.$element);
            }
            this.progress = this.$progress.progress({
                autoHide:true
            });
            this.settings.progress = function (value) {
                _this.progress.setValue(value.loaded / value.total * 100);
            }
        }
    };
    FileUploader.prototype.bindEvent = function () {
        var _this = this;
        this.fileInput.addEventListener("change", function (e) {
            _this.progress.show();
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
                        var path;

                        if (result.__infrastructure) {
                            path = result.result.path;
                        }
                        else {
                            path = result.path;
                        }
                        _this.setPath.call(_this, path);
                    }
                }
                setTimeout(function () {
                    _this.progress.hide();
                }, 2000);
            }
            xhr.send(formData);
        });
    };
    FileUploader.prototype.defaultSettings = {
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
    FileUploader.prototype.setPathForPreviewer = function () {

        if (path) {
            this.pathViewer.innerHTML = path;
        }
    };
    FileUploader.prototype.setPath = function (path) {
        this.setPathForPreviewer();
        this.path = path;
        this.valueInput.value = path;
    };
    $.fn.fileUploader = function (options) {
        var default_settings = {
            url: '',
            progress: function () { },
            load: function () { },
            error: function () { },
            abort: function () { }
        };
        this.eq(0).data("fileUploader") || this.eq(0).data("fileUploader", new FileUploader(options, this.eq(0)));
        return this.eq(0).data("fileUploader");
    };
    window.FileUploader = FileUploader;
})(window, jQuery);