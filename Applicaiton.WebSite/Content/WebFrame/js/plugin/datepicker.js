(function () {
    var Precision = {
        Year: 0,
        Month: 1,
        Day: 2,
        Hour: 3,
        Minute: 4
    };
    var ViewType = {
        Years:0,
        Year: 1,
        Month: 2,
        Day: 3,
        Hour: 4,
    };
    function DatePicker(element, options) {
        var _this = this;
        this.config = $.extend({}, DatePicker.defaultConfig, options);
        this.currentView = ViewType.Month;
        this.currentDate;
        this.currentYearsIndex;

        if (element) {

            if (element instanceof jQuery) {
                this.$element = element;
            }
            else {
                this.$element = $(element)
            }
        }
        this.initialize();
    };
    DatePicker.defaultConfig = {
        format: 'yyyy-MM-dd HH:mm:ss.S',
        preClass: ' icon-chevron-left',
        nextClass: 'icon-chevron-right',
        autoClose:true,
        precision: Precision.Minute,
        defaultDate: new Date(),
        onPicker: function (date) {
            console.log(date);
        }
    };
    DatePicker.prototype.initialize = function () {
        var _this = this;
        this.currentDate = this.config.defaultDate;
        this.initializeYearsArray();
        this.$box = this.createElement();
        this.renderMonthView();
      
        if (this.$element.is('input')) {
            this.$input = this.$element;
            this.dropdown = this.$box.hide().appendTo(document.body).dropdown({
                $positionTarget: this.$element,
                onShow:function(){
                    _this.renderMonthView();
                },
                onHide: function () {
      
                }
            });
            this.$element.on('click', function (event) {
                _this.dropdown.show();
                event.stopPropagation();
            });
        }
        else if (this.$element.is('div')) {

        }
    };
    DatePicker.prototype.initializeYearsArray = function (argument) {
        this.yearsArray = [];
        var currentYear = this.currentDate.getFullYear();
        var startYear = currentYear - 100;
        var endYear = currentYear + 100;
        this.currentYearsIndex = 5;

        for (var i = startYear; i <= endYear; i = i + 20) {
            var yearsArray = [];

            for (var j = i; j < i + 20; j++) {
                yearsArray.push(j);
            }
            this.yearsArray.push(yearsArray);
        }
    };

    DatePicker.prototype.createElement = function () {
        this.$box = $("<div>").addClass("date-picker");
        this.createHeader().appendTo(this.$box);
        this.$viewBox = $("<div>").addClass("view-box").appendTo(this.$box);
        return this.$box;
    };
    DatePicker.prototype.createHeader = function () {
        var _this = this;
        var $header = $("<div>").addClass("header");
        this.$buttonPre = $("<button>").addClass("button-icon pre").addClass(this.config.preClass).appendTo($header).click(function () {
            _this.pre();
        });
        this.$switch = $("<button>").addClass("button-text switch").appendTo($header);
        this.$buttonNext = $("<button>").addClass("button-icon next").addClass(this.config.nextClass).appendTo($header).click(function () {
            _this.next();
        });
        return $header;
    };

    DatePicker.prototype.pre = function () {

        if (this.currentView == ViewType.Month) {
            this.currentDate.setMonth(this.currentDate.getMonth() - 1);
            this.renderMonthView();
        }
        else if (this.currentView == ViewType.Years) {

            if (this.currentYearsIndex >0) {
                this.currentYearsIndex--;
                this.renderYearsView();
            }
        }
    };
    DatePicker.prototype.next = function () {

        if (this.currentView == ViewType.Month) {
            this.currentDate.setMonth(this.currentDate.getMonth() + 1);
            this.renderMonthView();
        }
        else if (this.currentView == ViewType.Years) {

            if (this.currentYearsIndex < this.yearsArray.length-1) {
                this.currentYearsIndex++;
                this.renderYearsView();
            }
        }
    };

    DatePicker.prototype.onPicker = function (date) {
        var dateString = date.format(this.config.format);
        this.config.onPicker.call(this, dateString);

        if (this.$input) {
            this.$input.val(dateString).trigger("change");

            if (this.config.autoClose) {
                this.dropdown.hide();
            }
        }
    };

    DatePicker.prototype.setHeaderOfYearsView = function (index) {
        var yeasArray = this.yearsArray[index];
        this.$switch.text(yeasArray[0] + "-" + yeasArray[yeasArray.length-1]).unbind();
    };
    DatePicker.prototype.setHeaderOfYearView = function (year) {
        var _this = this;
        var date = new Date();
        date.setYear(year);
        this.$switch.text(date.format("yyyy")).unbind().click(function (argument) {
            _this.renderYearsView();
        });
    };
    DatePicker.prototype.setHeaderOfMonthView = function (year, month) {
        var _this = this;
        var date = new Date(year, month);
        this.$switch.text(date.format("yyyy-MM")).unbind().click(function (argument) {
            _this.renderYearView();
        });
    };
    DatePicker.prototype.setHeaderOfDayView = function (year, month, day) {
        var _this = this;
        var date = new Date(year, month, day);
        this.$switch.text(date.format("yyyy-MM-dd")).unbind().click(function (argument) {
            _this.renderMonthView();
        });
    };
    DatePicker.prototype.setHeaderOfHourView = function (year, month, day, hour) {
        var _this = this;
        var date = new Date(year, month, day, hour);
        this.$switch.text(date.format("yyyy-MM-dd HH")).unbind().click(function (argument) {
            _this.renderDayView();
        });
    };

    DatePicker.prototype.renderYearsView = function () {
        this.currentView = ViewType.Years;
        this.$viewBox.empty();
        this.$yearsView = $("<div>").appendTo(this.$viewBox);
        this.setHeaderOfYearsView(this.currentYearsIndex);
        this.$yearsView.addClass("years-view").append(this.renderYears(this.currentYearsIndex));
        return this.$yearsView;
    };
    DatePicker.prototype.renderYears = function (yearsArrayIndex) {
        var yearsArray = this.yearsArray[yearsArrayIndex];
        var $ul = $("<ul>").addClass("clearfix year-list");

        for (var i = 0; i < yearsArray.length ; i++) {
            this.createYearItem(yearsArray[i]).appendTo($ul);
        }
        return $ul;
    };
    DatePicker.prototype.createYearItem = function (year) {
        var _this = this;
        var date = new Date();
        date.setYear(year);
        var $li = $("<li>").addClass("year-item");
        $li.text(year).on("click", function () {
            _this.currentDate.setYear(year);

            if (_this.config.precision == Precision.Year) {
                _this.onPicker.call(_this, date);
            }
            else {
                _this.currentDate.setYear(year);
                _this.renderYearView();
            }
        });
        return $li;
    };

    DatePicker.prototype.renderYearView = function () {
        this.currentView = ViewType.Year; 
        var year = this.currentDate.getFullYear();
        this.$viewBox.empty();
        this.$yearView = $("<div>").appendTo(this.$viewBox);
        this.setHeaderOfYearView(year);
        this.$yearView.addClass("year-view").append(this.renderMonths(year));
        return this.$dayView;
    };
    DatePicker.prototype.renderMonths = function (year) {
        var $ul = $("<ul>").addClass("clearfix month-list");

        for (var i = 0; i < 12 ; i++) {
            this.createMonthItem(year, i).appendTo($ul);
        }
        return $ul;
    };
    DatePicker.prototype.createMonthItem = function (year, month) {
        var _this = this;
        var date = new Date();
        date.setYear(year);
        date.setMonth(month);
       
        var $li = $("<li>").addClass("month-item");
        $li.text(date.getMonth() + 1).on("click", function () {
            _this.currentDate.setYear(year);
            _this.currentDate.setMonth(month);

            if (_this.config.precision == Precision.Month) {
                _this.onPicker.call(_this, date);
            }
            else {
                _this.currentDate.setMonth(month);
                _this.renderMonthView();
            }
        });
        return $li;
    };

    DatePicker.prototype.renderMonthView = function () {
        this.currentView = ViewType.Month;
        var year = this.currentDate.getFullYear(),
            month = this.currentDate.getMonth();
        this.$viewBox.empty();
        this.$weeks = this.renderWeeks().appendTo(this.$viewBox);
        this.$monthView = $("<div>").appendTo(this.$viewBox);
        this.setHeaderOfMonthView(year, month);
        this.$monthView.addClass("month-view").append(this.renderDays(year, month));
        return this.$monthView;
    };
    DatePicker.prototype.renderWeeks = function (argument) {
        var $ul = $("<ul>").addClass("week-list clearfix");
        var weeks = ["Sunda", "Mon", "Tues", "Wed", "Thur", "Fri", "Sat"];

        weeks.forEach(function (week) {
            var $li = $("<li>").text(week).addClass("week-item").appendTo($ul);
        });
        return $ul;
    }
    DatePicker.prototype.renderDays = function (year, month) {
        var $ul = $("<ul>").addClass("clearfix days-list");
        var firtDayInWeek = this.getFirstDayInWeekOfMonth(year, month);
        var daysCount = this.getDaysCountOfMonth(year, month);
        var index = 0;
        var day = firtDayInWeek - index;

        if (firtDayInWeek > 0) {

            for (; index < firtDayInWeek; index++, day++) {

                $ul.append(this.createDayItem(year, month, day, false));
            }
        }

        for (; index < daysCount; index++, day++) {
            $ul.append(this.createDayItem(year, month, day, true));
        }

        var balanceCount = (index + 1) % 7;

        if (balanceCount > 0) {

            for (var blance = 7 - balanceCount; blance >= 0; index++, blance--, day++) {
                $ul.append(this.createDayItem(year, month, day, false));
            }
        }
        return $ul;
    };
    DatePicker.prototype.createDayItem = function (year, month, day, isCurrentMonth) {
        var _this = this;
        var date = new Date();
        date.setYear(year);
        date.setMonth(month);
        date.setDate(day);
        var $li = $("<li>").addClass("day-item");
        $li.text(date.getDate()).on("click", function () {
            _this.currentDate.setYear(year);
            _this.currentDate.setMonth(month);
            _this.currentDate.setDate(day);

            if (_this.config.precision == Precision.Day) {
                _this.onPicker.call(_this, date);
            }
            else {
                _this.renderDayView.call(_this);
            }
        });

        isCurrentMonth && $li.addClass("day-of-current-month");
        return $li;
    };

    DatePicker.prototype.renderDayView = function () {
        this.currentView = ViewType.Day;
        var year = this.currentDate.getFullYear(),
            month = this.currentDate.getMonth(),
            day = this.currentDate.getDate();

        this.$viewBox.empty();
        this.$dayView = $("<div>").appendTo(this.$viewBox);
        this.setHeaderOfDayView(year, month, day);
        this.$dayView.addClass("day-view").append(this.renderHours(year, month, day));
        return this.$dayView;
    };
    DatePicker.prototype.renderHours = function (year, month, day) {
        var $ul = $("<ul>").addClass("clearfix hours-list");

        for (var i = 0; i < 24; i++) {
            this.createHourItem(year, month, day, i).appendTo($ul);
        }
        return $ul;
    };
    DatePicker.prototype.createHourItem = function (year, month, day, hour) {
        var _this = this;
        var date = new Date();
        date.setYear(year);
        date.setMonth(month);
        date.setDate(day);
        date.setHours(hour);
        var $li = $("<li>").addClass("hour-item");
        $li.text(date.getHours()).on("click", function () {
            _this.currentDate.setYear(year);
            _this.currentDate.setMonth(month);
            _this.currentDate.setDate(day);
            _this.currentDate.setHours(hour);

            if (_this.config.precision == Precision.Hour) {
                _this.onPicker.call(_this, date);
            }
            else {
                _this.renderHourView.call(_this);
            }
        });
        return $li;
    };

    DatePicker.prototype.renderHourView = function () {
        this.currentView = ViewType.Hour;
        var year = this.currentDate.getFullYear(),
            month = this.currentDate.getMonth(),
            day = this.currentDate.getDate();
        hour = this.currentDate.getHours();
        this.$viewBox.empty();
        this.$hourView = $("<div>").appendTo(this.$viewBox);
        this.setHeaderOfHourView(year, month, day, hour);
        this.$hourView.addClass("hour-view").append(this.renderMinutes(year, month, day, hour));
        return this.$dayView;
    };
    DatePicker.prototype.renderMinutes = function (year, month, day, hour) {
        var $ul = $("<ul>").addClass("clearfix minute-list");

        for (var i = 0; i < 60 ; i++) {
            this.createMinuteItem(year, month, day, hour, i).appendTo($ul);
        }
        return $ul;
    };
    DatePicker.prototype.createMinuteItem = function (year, month, day, hour, minute) {
        var _this = this;
        var date = new Date();
        date.setYear(year);
        date.setMonth(month);
        date.setDate(day);
        date.setHours(hour);
        date.setMinutes(minute);
        var $li = $("<li>").addClass("minute-item");
        $li.text(date.getMinutes()).on("click", function () {
            _this.currentDate.setYear(year);
            _this.currentDate.setMonth(month);
            _this.currentDate.setDate(day);
            _this.currentDate.setHours(hour);
            _this.currentDate.setMinutes(minute);

            _this.onPicker.call(_this, date);
        });
        return $li;
    };

    DatePicker.prototype.getDaysCountOfMonth = function (year, month) {
        month = parseInt(month, 10);
        var d = new Date(year, month, 0);
        return d.getDate();
    };
    DatePicker.prototype.getFirstDayInWeekOfMonth = function (year, month) {
        var d = new Date(year, month, 1);
        return d.getDay();
    };
    DatePicker.prototype.dispose = function () {

    };
    window.DatePicker = DatePicker;

    $.fn.datepicker = function (options) {
        var default_settings = {

        };
        var settings = $.extend({}, default_settings, options);
        this.eq(0).data("datepicker") || this.eq(0).data("datepicker", new DatePicker(this.eq(0), options));
        return this.eq(0).data("datepicker");
    };
})();