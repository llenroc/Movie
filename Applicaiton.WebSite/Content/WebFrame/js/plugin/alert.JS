!(function (window, $, undefined) {
    'use strict';
    var DATA_KEY = 'alert';
    var EVENT_KEY = '.' + DATA_KEY;
    var DATA_API_KEY = '.data-api';

    var Event = {
        CLOSE: 'close' + EVENT_KEY,
        CLOSED: 'closed' + EVENT_KEY,
        CLICK_DATA_API: 'click' + EVENT_KEY + DATA_API_KEY
    };

    function Alert(element, options) {
        var _self = this;
        this.element = element;
        this.$element = $(element);
        this.settings = $.extend({}, Alert.defaultSettings, options);
        this.$handlers = $('[data-target="#' + element.id + '"]').add(this.$element.find("[data-dismiss='alert']"));

        if (this.settings.$handler) {
            this.$handlers.add(this.settings.$handler);
        }
        this.addEventListeners();
    }
    Alert.defaultSettings = {

    };
    Alert.prototype.addEventListeners = function () {
        var _self = this;

        this.$handlers.on(Event.CLICK_DATA_API, function (event) {
            _self.close.call(_self, event);
        });
    };
    Alert.prototype.close = function (event) {
        this.destroyElement();
    };
    Alert.prototype.destroyElement = function() {
        this.$element.detach().trigger(Event.CLOSED).remove();
    };
    Alert.prototype.dispose = function dispose() {
        $.removeData(this.element, DATA_KEY);
        this.$element.off(EVENT_KEY);
        this.element = null;
    };

    $.fn.alert = function (options) {
        return new Alert(this.get(0), options);
    };
})(window, jQuery);