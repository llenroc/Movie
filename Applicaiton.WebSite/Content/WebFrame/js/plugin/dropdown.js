!(function(window,$,undefined){
    'use strict';
    var DATA_KEY = 'dropdown';
    var EVENT_KEY = '.' + DATA_KEY;
    var DATA_API_KEY = '.data-api';

    var Event = {
        HIDE: 'hide' + EVENT_KEY,
        SHOW: 'show' + EVENT_KEY,
        CLICK: 'click' + EVENT_KEY,
        CLICK_DATA_API: 'click' + EVENT_KEY + DATA_API_KEY,
    };

    function Dropdown($element,options){
        var _self = this;
        this.$element = $element;
        this.element = $element[0];
        this.settings = $.extend({}, this.defaultSettings, options);

        this.$handlers = $('[data-target="#' + this.element.id + '"]');
        var $dropdownWripperToggle = this.$element.closest(".dropdown-wripper").find('[data-toggle="dropdown"]');

        if ($dropdownWripperToggle.length) {
            this.$handlers.push($dropdownWripperToggle.get(0));
        }
        
        if (this.settings.handler) {

            if (!this.$handlers.length) {
                this.$handlers = this.settings.handler;
            }
            else {
                this.$handlers.add(this.settings.handler);
            }  
        }
        this.initialize();
    }
    Dropdown.prototype.defaultSettings = {
        $positionTarget:null,
        handler: null,
        onShow: function () {

        },
        onHide: function () {

        }
    };
    Dropdown.prototype.initialize = function () {
        var _this = this;
        this.$element.addClass("dropdown");

        if (this.settings.$positionTarget) {
            this.setPosition();

            $(window).on("resize", function () {
                _this.setPosition();
            });
        }
        this.bindEvent();
    };
    Dropdown.prototype.setPosition = function () {
        this.$element.css("left", this.settings.$positionTarget.offset().left);
        this.$element.css("top", this.settings.$positionTarget.offset().top + this.settings.$positionTarget.height());
    };
    Dropdown.prototype.bindEvent = function() {
        var _self = this;
        this.$handlers.on(Event.CLICK_DATA_API, function (event) {
            _self.toggle.call(_self, event);
        });

        $(document).on(Event.CLICK, function (event) {
        
            if ($(event.target).isChildAndSelfOf(_self.$handlers)) {
                return;
            }

            if (!_self.$element.is(event.target)) {
                _self.hide();
            }
        });

        this.$element.click(function (event) {
            event.stopPropagation();
        });
    };
    Dropdown.prototype.show = function (event) {
        this.$element.show().closest(".dropdown-wripper").addClass("open");
        this.settings.onShow.call(this);

        var relatedTarget = {
            relatedTarget: this
        };
        var showEvent = $.Event(Event.SHOW, relatedTarget);
        this.$element.trigger(showEvent);
    };
    Dropdown.prototype.hide = function (event) {
        this.$element.hide().closest(".dropdown-wripper").removeClass("open");
        this.settings.onHide.call(this);

        var relatedTarget = {
            relatedTarget: this
        };
        var hideEvent = $.Event(Event.HIDE, relatedTarget);
        this.$element.trigger(hideEvent);
    };
    Dropdown.prototype.toggle = function (event) {

        if (this.$element.is(":hidden")) {
            this.show(event);
        }
        else {
            this.hide(event);
        }
        return false;
    };
    Dropdown.prototype.dispose = function dispose() {
        $.removeData(this.element, DATA_KEY);
        this.$element.off(EVENT_KEY);
        this.$handlers.off(CLICK_DATA_API);
        this.element = null;
    };

    $.fn.dropdown = function (options) {
        this.eq(0).data("dropdown") || this.eq(0).data("dropdown", new Dropdown(this.eq(0), options));
        return this.eq(0).data("dropdown");
    };
})(window,jQuery);