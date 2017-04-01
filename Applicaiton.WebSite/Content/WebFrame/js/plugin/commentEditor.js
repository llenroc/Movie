(function ($) {
    "use strict";
    var baseColors = [
        ['#000000', '#424242', '#636363', '#9C9C94', '#CEC6CE', '#EFEFEF', '#F7F7F7', '#FFFFFF'],
        ['#FF0000', '#FF9C00', '#FFFF00', '#00FF00', '#00FFFF', '#0000FF', '#9C00FF', '#FF00FF'],
        ['#F7C6CE', '#FFE7CE', '#FFEFC6', '#D6EFD6', '#CEDEE7', '#CEE7F7', '#D6D6E7', '#E7D6DE'],
        ['#E79C9C', '#FFC69C', '#FFE79C', '#B5D6A5', '#A5C6CE', '#9CC6EF', '#B5A5D6', '#D6A5BD'],
        ['#E76363', '#F7AD6B', '#FFD663', '#94BD7B', '#73A5AD', '#6BADDE', '#8C7BC6', '#C67BA5'],
        ['#CE0000', '#E79439', '#EFC631', '#6BA54A', '#4A7B8C', '#3984C6', '#634AA5', '#A54A7B'],
        ['#9C0000', '#B56308', '#BD9400', '#397B21', '#104A5A', '#085294', '#311873', '#731842'],
        ['#630000', '#7B3900', '#846300', '#295218', '#083139', '#003163', '#21104A', '#4A1031']
    ];
    function CommentEditor(options, $element) {
        var default_settings = {
            focus: false,                 // set focus to editable area after initializing summernote
            lang: 'en-US',                // language 'en-US', 'ko-KR', ...
            editViewStatus: true,
            content: null,
            // callbacks
            oninit: null,             // initialize
            onfocus: null,            // editable has focus
            onblur: null,             // editable out of focus
            onenter: null,            // enter key pressed
            onkeyup: null,            // keyup
            onkeydown: null,          // keydown
            onImageUploadError: null, // imageUploadError
            onToolbarClick: null,
            onSave: null,
            onFocus: null,
            onImageUpload: null,
            buttons:[]
        };
        this.settings = $.extend(default_settings, options);
        this.buttons = [
           { name: 'bold', type: 'icon', 'hotKey': 'ctrl + b'},
           { name: 'italic', type: 'icon' },
           { name: 'underline', type: 'icon' },
           { name: 'strikethrough', type: 'icon' },
           { name: 'code', type: 'icon', coreCommand: 'code' },
           { name: 'insertimage', type: 'icon', coreCommand: 'insertPicture' },
           { name: 'link', type: 'icon', coreCommand: 'insertLink' },
        ];
        this.buttons = $.extend(this.buttons, this.settings.buttons);
        this.$element = $element;
        this.$toolbar = null;
        this.$content = null;
        this.$textarea = null;
        this.range = null;
        this.status = 1;//±à¼­Ô¤ÀÀ×´Ì¬¡£1Îª±à¼­¡¢2ÎªÔ¤ÀÀ
        this.bookmark = null;
        this.initialize();
    };
    CommentEditor.prototype = new Object();
    CommentEditor.prototype.initialize = function () {
        var _this = this;
        this.createToolbar();

        if (this.$element[0].nodeName == 'TEXTAREA') {
            this.$textarea = this.$element;
            this.$textarea.wrap("<div>");
            this.$element = this.$textarea.parent;
            this.$toolbar.insertBefore(this.$textarea);
        }
        else {
            this.$element.empty();
            this.$element.append(this.$toolbar);
        }
        this.$element.addClass("comment-editor");
        this.$content = $("<div class='content'>").appendTo(this.$element).on("DOMNodeInserted", function (e) {
            _this.domNodeInsertedHandle.call(_this, e);
        });
        this.$textarea = $("<textarea class='code'>").appendTo(this.$element).hide();

        this.$content.on('focus', function (e) {

            if (_this.settings.onFocus) {
                _this.settings.onFocus();
            }
        });
        this.setEdit();
    };
    CommentEditor.prototype.createToolbar = function () {
        var _this = this;
        this.$toolbar = $("<div class='toolbar'>");
        this.buttons.forEach(function (buttonData) {
            var $button = _this.createToolbarItem(buttonData);
        });
        return this.$toolbar;
    };
    CommentEditor.prototype.createToolbarItem = function (buttonData) {
        var _this = this;
        var $toolbarItem = $("<div class='toolbar-item'>").appendTo(this.$toolbar);
        var $button = $("<button>").appendTo($toolbarItem);

        switch (buttonData.type) {
            case 'icon':
                $button.addClass("button-icon").addClass("edit-icon-" + buttonData.name);
                break;
            case 'text':
                $button.addClass("button-text");

                if (buttonData.text) {
                    $button.text(buttonData.text);
                }
                break;
            case 'color':
                $button.addClass("button-text");

                if (buttonData.text) {
                    $button.text(buttonData.text);
                }
                break;
        }

        if (buttonData.values) {

            if (buttonData.parseButton) {
                buttonData.parseButton($button, buttonData.values[0]);
            }
            $toolbarItem.addClass("dropdown-wripper");
            var $valuesBox = $("<div class='dropdown width-em10 hide'>").appendTo($toolbarItem);
            $valuesBox.dropdown({
                $toggle: $button
            });
            var $dropdownMenu = $("<div class='dropdown-menu'>").appendTo($valuesBox);
            var $dropdownMenuList = $("<ul class='dropdown-menu-list'>").appendTo($dropdownMenu);
            buttonData.values.forEach(function (value) {
                var $item = $("<li>").html(value).appendTo($dropdownMenuList);
                $item.get(0).dataset["command"] = buttonData.name;
                $item.get(0).dataset["value"] = value;

                if (buttonData.parseItem) {
                    buttonData.parseItem($item, value);
                }

                $item.click(function () {

                    if (buttonData.coreCommand) {
                        _this[buttonData.coreCommand].call(_this, this.dataset['value']);
                    }
                    else if (buttonData.handler) {
                        buttonData.handler.call(_this, this.dataset['value']);
                    }
                    else {
                        _this.execCommand(this.dataset['command'], this.dataset['value']);
                    }

                    $valuesBox.hide();

                    if (buttonData.parseButton) {
                        buttonData.parseButton($button, value);
                    }
                });
            });
        }
        else if (buttonData.coreCommand) {
            $button.data("coreCommand", buttonData.coreCommand).click(function (event) {
                _this[buttonData.coreCommand].call(_this, this.dataset['value']);
                event.stopPropagation();
            });
        }
        else if (buttonData.handler) {
            $button.click(function (event) {
                buttonData.handler.call(_this, this.dataset['value']);
                event.stopPropagation();
            });
        }
        else if (buttonData.type == 'color') {
            $button.click(function (event) {
                _this.colorPicker(buttonData.name, $button);
                event.stopPropagation();
            });
        }
        else {
            $button.get(0).dataset['command'] = buttonData.name;
            $button.attr("unselectable", "on").click(function (event) {
                _this.execCommand(this.dataset['command']);
                event.stopPropagation();
            });
        }
        return $button;
    };
    CommentEditor.prototype.setEdit = function () {
        this.$toolbar.show();
        this.$content.attr("contenteditable", true).show().focus();
        return this;
    }
    CommentEditor.prototype.execCommand = function (name, value) {
        document.execCommand(name, false, value);
        return this;
    };

    CommentEditor.prototype.initializeImage = function (img) {
        var _this = this;

        if (!$(img).closest(".image-container").length) {
            $(img).wrap("<div class='image-container'/>");
        }
        
        $(img).click(function (e) {
            _this.setImageSettingMode.call(_this, this);
        });
    };
    CommentEditor.prototype.domNodeInsertedHandle = function (e) {
        var _this = this;
        
        if (e.target.nodeName == "IMG") {
            _this.initializeImage(e.target);
        }
        else {
            var imgs = e.target.querySelectorAll("img");
            imgs.forEach(function (img) {
                _this.initializeImage(img);
            })
        }
    };
    CommentEditor.prototype.setImageSettingMode = function (img) {
        var _this = this;
        var $img = $(img);
        var $imgContainer = $img.closest(".image-container");

        if (!$imgContainer.hasClass("setting")){
            $imgContainer.addClass("setting");
            var $imageSettingBar = $("<div class='image-setting-bar'>").appendTo($imgContainer);
            var $fullWidthButton = $("<button class='button-text'>").text("100%").appendTo($imageSettingBar).click(function () {
                $imgContainer.css({ width: "100%" });
                _this.removeImageSettingMode(img);
            });
            var $harfWidthButton = $("<button class='button-text'>").text("50%").appendTo($imageSettingBar).click(function () {
                $imgContainer.css({ width: "50%" });
                _this.removeImageSettingMode(img);
            });
            var $pullLeftButton = $("<button class='button-text'>").text("pull left").appendTo($imageSettingBar).click(function () {
                $imgContainer.css({ "float": "left" });
                _this.removeImageSettingMode(img);
            });
            var $pullRightButton = $("<button class='button-text'>").text("pull right").appendTo($imageSettingBar).click(function () {
                $imgContainer.css({ "float": "right" });
                _this.removeImageSettingMode(img);
            });
            var $clearLeftButton = $("<button class='button-text'>").text("clear pull").appendTo($imageSettingBar).click(function () {
                $imgContainer.css({ "float": "none" });
                _this.removeImageSettingMode(img);
            });
            var $clearZoomButton = $("<button class='button-text'>").text("clear zoom").appendTo($imageSettingBar).click(function () {
                $img.css({ "width": "100%" });
                $imgContainer.css({ "width": "auto" });
                _this.removeImageSettingMode(img);
            });

            $(document).click(function () {
                _this.removeImageSettingMode(img);
            });
            $imgContainer.click(function (event) {
                event.stopPropagation();
            });
        }
    };
    CommentEditor.prototype.removeImageSettingMode = function (img) {
        var $imgContainer = $(img).closest(".image-container");
        $imgContainer.removeClass("setting");
        $imgContainer.find(".image-setting-bar").remove();
    };

    CommentEditor.prototype.save = function () {

        if (this.settings.onSave) {
            this.settings.onSave.call(this, this.getContent());
        }
    };
    CommentEditor.prototype.colorPicker = function () {
        var _this = this;
        var range = _this.getRange();
    };
    CommentEditor.prototype.insertPicture = function () {
        var _this = this;

        if (!this.$pictureFileInput) {
            this.$pictureFileInput = $("<input type=file>").hide().appendTo(this.$element).change(function () {
                _this.settings.onImageUpload(this.files, _this);
            });
        }
        this.$pictureFileInput.click();
    };
    
    CommentEditor.prototype.code = function () {
        var _this = this;
        var range = _this.getRange();

        if (!this.$codeDialog) {
            var $codeInput = $("<textarea class='block' rows=10>").wrap("<div>");
            this.$codeDialog = $.dialog({
                responsive: true,
                title: "insert code", onOk: function () {
                    var $pre = $("<pre>").html($codeInput.val());
                    range.insertNode($pre.get(0));
                }
            });
            this.$codeDialog.setContent($codeInput.parent());
        }
        this.$codeDialog.show();
    };
    CommentEditor.prototype.setContent = function (content) {
        this.$content.html(content);
        this.$textarea.val(content);
    };
    CommentEditor.prototype.getContent = function () {
        var content = this.$content.html();
        return content;
    };
    CommentEditor.prototype.focus = function () {
        this.$content.focus();
    }
    CommentEditor.prototype.getSelection = function () {
        return window.getSelection();
    }
    CommentEditor.prototype.getRange = function () {
        var selection = window.getSelection();
        var range;

        if (selection.rangeCount) {
            range = selection.getRangeAt(0);
        }
        else {
            range = document.createRange();
        }
        return range;
    }
    CommentEditor.prototype.insertImage = function (imagePath) {
        var selection = document.getSelection();
        var content;
        var range;

        if (selection.rangeCount) {
            range = selection.getRangeAt(0);
        }
        else {
            range = document.createRange();
        }

        if (selection.type == "Range") {
            content = range.cloneContents();
        }

        if (content) {
            range.deleteContents();
        }
        var $img = $("<img>").attr("src", imagePath);
        range.insertNode($img.get(0));
    };
    CommentEditor.prototype.insertLink = function (link) {
        var _this = this;
        var selection = document.getSelection();
        var range;
        var content;
        var $linkInput = $("<input type='text' placeholder='link'>").wrap("<div class='input-wrapper large'>");

        if (selection.rangeCount) {
            range = selection.getRangeAt(0);
        }
        else {
            range = document.createRange();
        }

        if (selection.type == "Range") {
            content = range.cloneContents();
        }
        else {
            var $linkTextInput = $("<input type='text' placeholder='linkText'>").wrap("<div class='input-wrapper large'>");
        }

        this.$linkDialog = $.dialog({
            responsive: true,
            title: "insert link", onOk: function () {
                var linkValue = $linkInput.val();

                if (content) {
                    range.deleteContents();
                    var $a = $("<a href='" + linkValue + "'>").append(content);
                }
                else {
                    var $a = $("<a href='" + linkValue + "'>").html($linkTextInput.val());
                }
                range.insertNode($a.get(0));
            }
        });
        this.$linkDialog.setContent($linkInput.parent());

        if (!content) {
            this.$linkDialog.setContent($linkTextInput.parent());
        }
        this.$linkDialog.show();
    };
    CommentEditor.prototype.insertHTML = function (html) {
        var selection = document.getSelection();
        var range;

        if (selection.rangeCount) {
            range = selection.getRangeAt(0);
        }
        else {
            range = document.createRange();
        }
        var range = this.getRange();
        var node = $(html).get(0);
        range.surroundContents(node);
        selection.removeAllRanges();//Çå³ýÑ¡Ôñ
        selection.addRange(range);
        return this;
    };
    
    $.fn.commentEditor = function (options) {
        this.eq(0).data("CommentEditor") || this.eq(0).data("CommentEditor", new CommentEditor(options, this.eq(0)));
        return this.eq(0).data("CommentEditor");
    };
})(jQuery);