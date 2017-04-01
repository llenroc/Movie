(function () {
    function AudioNotify(options) {
        this.config = $.extend({}, AudioNotify.defaultConfig, options);
        this.$audio = null;
        this.initialize();
    }
    AudioNotify.defaultConfig = {

    };
    AudioNotify.prototype.initialize = function () {
        this.$audio = $("<audio>").hide().appendTo(document.body);
        this.$audio[0].src = "http://www.helloweba.com/demo/notifysound/notify.mp3";
    };
    AudioNotify.prototype.notify = function () {
        this.$audio[0].play();
    };
    var audioNotify = null;

    $.audioNotify = function (options) {
       
        if (!audioNotify) {
            audioNotify = new AudioNotify(options);
        }
        return audioNotify;
    };
})();