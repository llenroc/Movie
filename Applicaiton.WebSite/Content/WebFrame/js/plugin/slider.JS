(function ($) {
    var TRANSITION_DURATION = 600;

    var ClassName = {
        Slider: 'slider',
        ACTIVE: 'active',
        SLIDE: 'slide',
        RIGHT: 'right',
        LEFT: 'left',
        NEXT: 'next',
        PREV: 'prev',
        ITEM: 'item'
    };
    var Selector = {
        ACTIVE: '.active',
        ACTIVE_ITEM: '.active.item',
        ITEM: '.item',
        NEXT_PREV: '.next, .prev',
        DATA_SLIDE: '[data-slide], [data-slide-to]',
        DATA_RIDE: '[data-ride="slider"]'
    };
    var Direction = {
        NEXT: 'next',
        PREV: 'prev',
    };

    function Slider($element, options) {
        this.$element = $element;
        this.config = $.extend({}, this.defaultConfig, options);
        this.currentIndex = 1;
        this.length = 0;
        this.timer = null;
        this.isPaused = false;
        this.isSliding = false;
        this.initialize();
    };
    Slider.prototype.defaultConfig = {
        interval: 5000,
        slide: true,
        enableIndicator: true,
        enableControl:true
    };
    Slider.prototype.initialize = function () {
        var $items = this.$element.find(Selector.ITEM);
        this.length = $items.length;

        if (!this.length) {
            return;
        }
        var $activeItem = this.$element.find(Selector.ACTIVE_ITEM);

        if (!$activeItem.length) {
            $items.eq(this.currentIndex-1).addClass("active");
        }
        else {
            this.currentIndex = $items.index($activeItem)+1;
        }

        if (this.config.enableIndicator) {
            this.createIndicator();
        }

        if (this.config.enableControl) {
            this.createControl();
        }
        this.bindEvent();

        if (this.config.slide) {
            this.cycle();
        }
    };
    Slider.prototype.createIndicator = function () {
        var $indicatorList = $("<ol>").addClass("slider-indicators").insertBefore(this.$element.find(".slider-inner"));

        for (var i = 0; i < this.length; i++) {
            var $indicator = $("<li>").appendTo($indicatorList);

            if (i == this.currentIndex - 1) {
                $indicator.addClass("active");
            }
        }
        this.$indicators = $indicatorList.find("li");
    };
    Slider.prototype.createControl = function () {
        var $pre = $("<a>").addClass("slider-control left").attr("data-slide", "pre").html("<i class='icon-long-arrow-left'><i/>").appendTo(this.$element);
        var $next = $("<a>").addClass("slider-control right").attr("data-slide", "next").html("<i class='icon-long-arrow-right'><i/>").appendTo(this.$element);
    };
    Slider.prototype.bindEvent = function () {
        var _this = this;
        this.$element.hover(function () {
            clearInterval(_this.timer);
        }, function () {

            if (_this.config.slide) {
                _this.cycle();
            }
        });
       
        document.addEventListener("DOMNodeInserted", function (e) {

        });

        if(this.config.enableControl){
            this.$element.find(Selector.DATA_SLIDE).on("click", function () {
                _this[this.dataset.slide].call(_this);
            });
        }

        if (this.config.enableIndicator) {
            this.$indicators.on("click", function () {
                _this.to(_this.$indicators.index($(this)) + 1);
            });
        }
    };
    Slider.prototype.cycle = function (e) {
        e || (this.isPaused = false);
        var _this = this;

        if (this.config.interval && !this.isPaused) {
            this.timer = setInterval(function () {
                _this.next();
            }, _this.config.interval);
        }
    };
    Slider.prototype.pre = function () {

        if (this.isSliding)
            return;
        var index = this.currentIndex - 1;

        if (index == 0) {
            index = this.length;
        }
        this.slide(Direction.PREV,index);
    };
    Slider.prototype.next = function () {

        if (this.isSliding)
            return;

        var index = this.currentIndex + 1;

        if (index > this.length) {
            index = 1;
        }
        this.slide(Direction.NEXT, index);
    };
    Slider.prototype.to = function (index) {
        var _this = this;

        if (index > this.length || index < 0) {
            return;
        }

        if (this.isSliding) {
            return this.$element.one('slide', function () {
                _this.to(pos);
            });
        }

        if (this.currentIndex === index) {
            this.pause();
            this.cycle();
            return;
        }
        var direction = index > this.currentIndex?Direction.NEXT :Direction.PREVIOUS
        this.slide(direction, index)
    };
    Slider.prototype.slide = function (direction, index) {
        var _this = this;
        var $items = this.$element.find(Selector.ITEM);
        var $currentItem = $items.eq(this.currentIndex - 1);
        var $nextItem = $items.eq(index - 1);
        var isCycling = this.timer;

        if ($nextItem.hasClass('active'))
            return (this.isSliding = false);

        var directionalClassName;
        var orderClassName;
        var eventDirectionName;

        if (direction === Direction.NEXT) {
            directionalClassName = ClassName.LEFT;
            orderClassName = ClassName.NEXT;
            eventDirectionName = Direction.LEFT;
        }
        else {
            directionalClassName = ClassName.RIGHT;
            orderClassName = ClassName.PREV;
            eventDirectionName = Direction.RIGHT;
        }
        this.isSliding = true
        isCycling && this.pause();

        $nextItem.addClass(orderClassName)
        $nextItem[0].offsetWidth // force reflow

        $currentItem.addClass(directionalClassName);
        $nextItem.addClass(directionalClassName);

        $currentItem.one("webkitTransitionEnd", function () {
            $nextItem.removeClass([directionalClassName, orderClassName].join(' ')).addClass(ClassName.ACTIVE);
            $currentItem.removeClass([ClassName.ACTIVE, orderClassName, directionalClassName].join(' '));
            _this.isSliding = false;
            setTimeout(function () {
            }, 0)
        }).emulateTransitionEnd(TRANSITION_DURATION);

        if (this.config.enableIndicator) {
            this.setActiveIndicator(index);
        }
        this.currentIndex = index;

        isCycling && this.cycle();
        return this;
    };
    Slider.prototype.setActiveIndicator = function (index) {
        this.$indicators.eq(this.currentIndex - 1).removeClass("active");
        this.$indicators.eq(index - 1).addClass("active");
    };
    Slider.prototype.pause = function () {
        this.isPaused = true;

        if (this.$element.find([ClassName.NEXT,ClassName.PREV].join(",")).length) {
            this.$element.trigger("webkitTransitionEnd");
            this.cycle(true)
        }
        clearInterval(this.timer);
    };

    $.fn.slider = function (options) {
        this.eq(0).data("slider") || this.eq(0).data("slider", new Slider(this.eq(0), options));
        return this.eq(0).data("slider");
    };
}(jQuery));