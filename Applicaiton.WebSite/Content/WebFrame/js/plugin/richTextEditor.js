(function ($) {
    "use strict";
    // Õ³ÌùÊÂ¼þ¼à¿Ø
    $.fn.pasteEvents = function (delay) {

        if (delay == undefined)
            delay = 10;
        return $(this).each(function () {
            var $el = $(this);
            $el.on("paste", function () {
                $el.trigger("prepaste");
                setTimeout(function () { $el.trigger("postpaste"); }, delay);
            });
        });
    };
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
    function RichTextEditor(options, $element) {
        var default_settings = {
            focus: false,                 // set focus to editable area after initializing summernote
            lang: 'en-US',                // language 'en-US', 'ko-KR', ...
            editViewStatus: true,
            content: null,
            fontNames: [
                'Arial', 'Arial Black', 'Comic Sans MS', 'Courier New',
                'Helvetica Neue', 'Impact', 'Lucida Grande',
                'Tahoma', 'Times New Roman', 'Verdana'
            ],
            fontSizes: ['8', '9', '10', '11', '12', '14', '18', '24', '36'],
            lineHeights: ['1.0', '1.2', '1.4', '1.5', '1.6', '1.8', '2.0', '3.0'],
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
           { name: 'undo', type: 'icon' },
           { name: 'redo', type: 'icon' },
           {
               name: 'head', type: 'text',
               values: ["1", "2", "3", "4", "5", "6"],
               parseButton: function ($button, value) {
                   $button.text("head" + value);
               },
               parseItem: function ($item, value) {
                   $item.text("head" + value)
               },
               coreCommand: 'head'
           },
           {
               name: 'fontname', type: 'text', values: this.settings.fontNames,
               parseButton: function ($button, value) {
                   $button.text(value);
               },
               parseItem: function ($item, value) {
                   $item.css("font-family", value)
               },
           },
           {
               name: 'fontsize', type: 'text', values: this.settings.fontSizes,
               parseButton: function ($button, value) {
                   $button.text(value);
               },
               parseItem: function ($item, value) {
                   $item.css("font-size", value + "px")
               }
           },
           {
               name: 'forecolor', type: 'color', text: 'F', parseButton: function ($button, value) {
                   $button.css("color", value)
               },
           },
           {
               name: 'backcolor', type: 'color', text: 'B', parseButton: function ($button, value) {
                   $button.css("background-color", value)
               },
           },
           { name: 'removeformat', type: 'icon' },
           { name: 'bold', type: 'icon', 'hotKey': 'ctrl + b'},
           { name: 'italic', type: 'icon' },
           { name: 'underline', type: 'icon' },
           { name: 'strikethrough', type: 'icon' },
           { name: 'justifyleft', type: 'icon' },
           { name: 'justifycenter', type: 'icon' },
           { name: 'justifyright', type: 'icon' },
           { name: 'justifyfull', type: 'icon' },
           { name: 'indent', type: 'icon' },
           { name: 'outdent', type: 'icon' },
           { name: 'code', type: 'icon', coreCommand: 'code' },
           { name: 'annotation', type: 'icon', coreCommand: 'annotation' },
           { name: 'blockquote', type: 'icon', coreCommand: 'createBlockquote' },
           { name: 'insertimage', type: 'icon', coreCommand: 'insertPicture' },
           { name: 'link', type: 'icon', coreCommand: 'insertLink' },
           { name: 'insertorderedlist', type: 'icon' },
           { name: 'insertunorderedlist', type: 'icon' }
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
    RichTextEditor.prototype = new Object();
    RichTextEditor.prototype.initialize = function () {
        var _this = this;
        var html = this.settings.content;
        this.createToolbar();

        if (this.$element[0].nodeName == 'TEXTAREA') {
            this.$textarea = this.$element;
            this.$textarea.wrap("<div>");
            this.$element = this.$textarea.parent;
            html = this.settings.content ? this.settings.content : this.$textarea.val();
            this.$toolbar.insertBefore(this.$textarea);
        }
        else {
            html = this.settings.content ? this.settings.content : this.$element.html();
            this.$element.empty();
            this.$element.append(this.$toolbar);
        }
        this.$element.addClass("rich-text-editor");
        this.$content = $("<div class='content'>").appendTo(this.$element).on("DOMNodeInserted", function (e) {
            _this.domNodeInsertedHandle.call(_this, e);
        }).html(html);
        this.$textarea = $("<textarea class='code'>").appendTo(this.$element).hide();
        this.$previewer = $("<div class='previewer'>").appendTo(this.$element);

        this.$content.on('focus', function (e) {

            if (_this.settings.onFocus) {
                _this.settings.onFocus();
            }
        });
        // Ê¹ÓÃ
        this.$content.on("paste", function (event) {
            //console.log(event.originalEvent.clipboardData.getData("text"))
            //return false;
        });
       
        if (this.settings.focus) {
            this.setEdit().focus();
        }
        else {
            this.setViewEditStatus(this.settings.editViewStatus);
        }
    };

    RichTextEditor.prototype.createToolbar = function () {
        var _this = this;
        this.$toolbar = $("<div class='toolbar'>");
        this.buttons.forEach(function (buttonData) {
            var $button = _this.createToolbarItem(buttonData);
        });
        return this.$toolbar;
    };
    RichTextEditor.prototype.createToolbarItem = function (buttonData) {
        var _this = this;
        var $toolbarItem = $("<div class='toolbar-item'>").appendTo(this.$toolbar);
        var $button = $("<button>").attr("title",buttonData.name).appendTo($toolbarItem);

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
            $toolbarItem.addClass("dropdown-wripper dropdown-menu");

            var $dropdownMenu = $("<ul class='dropdown dropdown-menu-list hide'>").appendTo($toolbarItem);
            $dropdownMenu.dropdown({
                handler: $button
            });
            buttonData.values.forEach(function (value) {
                var $item = $("<li>").appendTo($dropdownMenu);
                var $itemA=$("<a>").appendTo($item).html(value);
                $item.get(0).dataset["command"] = buttonData.name;
                $item.get(0).dataset["value"] = value;

                if (buttonData.parseItem) {
                    buttonData.parseItem($itemA, value);
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
                    $dropdownMenu.hide();

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

    RichTextEditor.prototype.execCommand = function (name, value) {
        document.execCommand(name, false, value);
        return this;
    };

    RichTextEditor.prototype.initializeImage = function (img) {
        var _this = this;

        if (!$(img).closest(".image-container").length) {
            $(img).wrap("<div class='image-container'/>");
        }
        
        $(img).click(function (e) {
            _this.setImageSettingMode.call(_this, this);
        });
    };
    RichTextEditor.prototype.domNodeInsertedHandle = function (e) {
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
    RichTextEditor.prototype.setImageSettingMode = function (img) {
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
    RichTextEditor.prototype.removeImageSettingMode = function (img) {
        var $imgContainer = $(img).closest(".image-container");
        $imgContainer.removeClass("setting");
        $imgContainer.find(".image-setting-bar").remove();
    };

    RichTextEditor.prototype.save = function () {

        if (this.settings.onSave) {
            this.settings.onSave.call(this, this.getContent());
        }
    };
    RichTextEditor.prototype.colorPicker = function () {
        var _this = this;
        var range = _this.getRange();
    };
    RichTextEditor.prototype.insertPicture = function () {
        var _this = this;

        if (!this.$pictureFileInput) {
            this.$pictureFileInput = $("<input type=file>").hide().appendTo(this.$element).change(function () {
                _this.settings.onImageUpload(this.files, _this);
            });
        }
        this.$pictureFileInput.click();
    };
    
    RichTextEditor.prototype.code = function () {
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
    RichTextEditor.prototype.annotation = function () {
        var _this = this;
        var selection = document.getSelection();
        var range;
        var content;

        if (selection.rangeCount) {
            range = selection.getRangeAt(0);

            if (selection.type == "Range") {
                content = range.cloneContents();

                if (!this.$annotationDialog) {
                    var $annotationInput = $("<textarea class='block' rows=10>").wrap("<div>");
                    this.$annotationDialog = $.dialog({
                        responsive: true,
                        title: "insert annotation", onOk: function () {

                            content = range.cloneContents();

                            if (content) {
                                range.deleteContents();
                                var $annotation = $("<annotation content='" + $annotationInput.val() + "'>").append(content);
                                range.insertNode($annotation.get(0));
                            }
                        }
                    });
                    this.$annotationDialog.setContent($annotationInput.parent());
                }
                this.$annotationDialog.show();
            }
        }
    }
    RichTextEditor.prototype.setContent = function (content) {
        this.$content.html(content);
        this.$textarea.val(content);
        return this;
    };
    RichTextEditor.prototype.getContent = function () {
        var content = this.$content.html();
        return content;
    };
    RichTextEditor.prototype.focus = function () {
        this.$content.focus();
    };
    RichTextEditor.prototype.setViewEditStatus = function (status) {

        if (status) {
            return this.setEdit();
        }
        else {
            return this.setView();
        }
    };
    RichTextEditor.prototype.setView = function () {
        this.$previewer.html(this.$content.html());
        this.$previewer.find("pre").codeHighlight();
        this.$toolbar.hide();
        this.$content.attr("contenteditable", false).hide();
        this.$previewer.show();
        this.status = 2;
        return this;
    };
    RichTextEditor.prototype.setEdit = function () {
        this.$previewer.hide();
        this.$toolbar.show();
        this.$content.attr("contenteditable", true).show().focus();
        this.status = 1;
        return this;
    };
    RichTextEditor.prototype.getSelection = function () {
        return window.getSelection();
    };
    RichTextEditor.prototype.getRange = function () {
        var selection = window.getSelection();
        var range;

        if (selection.rangeCount) {
            range = selection.getRangeAt(0);
        }
        else {
            range = document.createRange();
        }
        return range;
    };
    RichTextEditor.prototype.insertImage = function (imagePath) {
        var selection = this.getSelection();
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
    RichTextEditor.prototype.head = function (level) {
        var _this = this;
        var selection = document.getSelection();
        var range;
        var content;

        if (selection.rangeCount) {
            range = selection.getRangeAt(0);

            if (selection.type == "Range") {
                content = range.cloneContents();

                if (content) {
                    range.deleteContents();
                    var $h = $("<h" + level + " class='head " + level + "'>").append(content);
                    range.insertNode($h.get(0));
                }
            }
        }
    };
    RichTextEditor.prototype.colorPicker = function (command, $button) {
        var _this = this;
        var $popup = $.popup({
            $target: $button
        });
        var $colorPicker = $("<div class='color-picker'>");
        var $colorPickerMartix = $("<div class='color-picker-martix'>").appendTo($colorPicker);
        $popup.setConent($colorPicker);

        for (var i = 0; i < baseColors.length; i++) {
            var colorGroup = baseColors[i];

            for (var j = 0; j < colorGroup.length; j++) {
                var color = colorGroup[j];
                var $colorCellButton = $("<button class='color-cell'>").css("background-color", color).data('color', color).appendTo($colorPickerMartix).click(function () {
                    _this.execCommand(command, $(this).data('color'));
                    $popup.remove();
                });
            }
        }
    };
    RichTextEditor.prototype.insertLink = function (link) {
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
            title: "insert link",
            onOk: function () {
                var linkValue = $linkInput.val();

                if (content) {
                    range.deleteContents();
                    var $a = $("<a href='" + linkValue + "'>").append(content);
                }
                else {
                    var $a = $("<a href='" + linkValue + "'>").html($linkTextInput.val());
                }
                range.insertNode($a.get(0));
                return true;
            }
        });
        this.$linkDialog.setContent($linkInput.parent());

        if (!content) {
            this.$linkDialog.setContent($linkTextInput.parent());
        }
        this.$linkDialog.show();
    };
    RichTextEditor.prototype.createBlockquote = function () {
        var _this = this;
        var selection = document.getSelection();
        var range;
        var content;

        if (selection.rangeCount) {
            range = selection.getRangeAt(0);

            if (selection.type == "Range") {
                content = range.cloneContents();
            }

            if (content) {
                range.deleteContents();
                var $blockquote = $("<blockquote>").append(content);
                range.insertNode($blockquote.get(0));
            }
        }
    };
    RichTextEditor.prototype.insertHTML = function (html) {
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
    
    $.fn.richTextEditor = function (options) {
        this.eq(0).data("RichTextEditor") || this.eq(0).data("RichTextEditor", new RichTextEditor(options, this.eq(0)));
        return this.eq(0).data("RichTextEditor");
    };
})(jQuery);