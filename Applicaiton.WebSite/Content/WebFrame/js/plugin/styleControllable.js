!(function (window, $, undefined) {
    'use strict';
    var DATA_KEY = 'styleControllable';
    var EVENT_KEY = '.' + DATA_KEY;
    var DATA_API_KEY = '.data-api';

    var Event = {
        HIDE: 'hide' + EVENT_KEY,
        SHOW: 'show' + EVENT_KEY,
        CLICK: 'click' + EVENT_KEY,
        CLICK_DATA_API: 'click' + EVENT_KEY + DATA_API_KEY,
    };

    function StyleControllable($element, options) {
        var _self = this;
        this.$element = $element;
        this.element = $element[0];
        this.settings = $.extend({}, this.defaultSettings, options);
        this.$handlers = $('[data-target="#' + this.element.id + '"]');

        if (this.element.dataset.style) {
            this.settings.style = this.element.dataset.style;
        }

        if (this.settings.handler) {

            if (!this.$handlers.length) {
                this.$handlers = this.settings.handler;
            }
            else {
                this.$handlers.add(this.settings.handler);
            }
        }
        this.bindEvent();
    }
    StyleControllable.prototype.defaultSettings = {
        handler: null,
        style:"show"
    };
    // private
    StyleControllable.prototype.bindEvent = function () {
        var _self = this;
        this.$handlers.on(Event.CLICK_DATA_API, function (event) {
            _self.toggle.call(_self, event);
        });
    };
    StyleControllable.prototype.toggle = function (event) {
        this.$element.toggleClass(this.settings.style);
        return false;
    };
    StyleControllable.prototype.dispose = function dispose() {
        this.$element.off(EVENT_KEY);
        this.$handlers.off(CLICK_DATA_API);
        this.element = null;
    };

    $.fn.styleControllable = function (options) {
        this.eq(0).data("styleControllable") || this.eq(0).data("styleControllable", new StyleControllable(this.eq(0), options));
        return this.eq(0).data("styleControllable");
    };
})(window, jQuery);