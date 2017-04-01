;(function($){
    var methods={
        number: function (validate,value) {

            if (/^-?(?:\d+|\d{1,3}(?:,\d{3})+)?(?:\.\d+)?$/.test(value)) {
                this.success();
            }
            else {
                this.error();
            }
        },
        digits: function (validate, value) {

            if (/^\d+$/.test(value)) {
                this.success();
            }
            else {
                this.error();
            }
        },
        required: function (validate, value) {

            if(value&&value.length){
                this.success();
            }
            else {
                this.error();
            }
        },
        email: function (validate, value) {
            var reg=/^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/;
            
            if(reg.test(value)){
                this.success();
            }
            else{
                this.error();
            }
        },
        url: function (validate, value) {
            var reg=/^(?:(?:(?:https?|ftp):)?\/\/)(?:\S+(?::\S*)?@)?(?:(?!(?:10|127)(?:\.\d{1,3}){3})(?!(?:169\.254|192\.168)(?:\.\d{1,3}){2})(?!172\.(?:1[6-9]|2\d|3[0-1])(?:\.\d{1,3}){2})(?:[1-9]\d?|1\d\d|2[01]\d|22[0-3])(?:\.(?:1?\d{1,2}|2[0-4]\d|25[0-5])){2}(?:\.(?:[1-9]\d?|1\d\d|2[0-4]\d|25[0-4]))|(?:(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)(?:\.(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)*(?:\.(?:[a-z\u00a1-\uffff]{2,})).?)(?::\d{2,5})?(?:[/?#]\S*)?$/i;
            
            if(reg.test(value)){
                this.success();
            }
            else{
                this.error();
            }
        },
        date: function (validate, value) {
            var reg = !/Invalid|NaN/;

            if(reg.test(value)){
                this.success();
            }
            else{
                this.error();
            }
        },
        number: function (validate, value) {
            var reg = /^(?:-?\d+|-?\d{1,3}(?:,\d{3})+)?(?:\.\d+)?$/;

            if(reg.test(value)){
                this.success();
            }
            else{
                this.error();
            }
        },
        digits: function (validate, value) {
            var reg = /^\d+$/;

            if (reg.test(value)) {
                this.success();
            }
            else {
                this.error();
            }
        },
        creditcard: function (value, element) {

            // accept only spaces, digits and dashes
            if (/[^0-9 \-]+/.test(value)) {
                this.error();
                return false;
            }
            var nCheck = 0,
				nDigit = 0,
				bEven = false,
				n, cDigit;

            value = value.replace(/\D/g, "");

            // Basing min and max length on
            // http://developer.ean.com/general_info/Valid_Credit_Card_Types
            if (value.length < 13 || value.length > 19) {
                this.error();
                return false;
            }

            for (n = value.length - 1; n >= 0; n--) {
                cDigit = value.charAt(n);
                nDigit = parseInt(cDigit, 10);

                if (bEven) {

                    if ((nDigit *= 2) > 9) {
                        nDigit -= 9;
                    }
                }
                nCheck += nDigit;
                bEven = !bEven;
            }

            if ((nCheck % 10) === 0) {
                this.success();
                return true;
            }
            this.error();
            return false;
        },
        minlength: function (validate, value) {

            if(!value||value.length<validate.condition){
                this.error();
            }
            else{
                this.success();
            }
        },
        maxlength: function (validate, value) {

            if (value == null||value=="") {
                this.success();
                return;
            }

            if (value.length > validate.condition) {
                this.error();
            }
            else {
                this.success();
            }
        },
        remote: function (validate, value) {
            var self=this;
            var data={};
            data[self.inputValidate.$input.attr("name")] = value;

            this.ajax=$.ajax({
                url: validate.condition,
                data: data,
                type:"POST",
                dataType: "json",
                beforeSend: function(){
                    self.inputValidate.$element.addClass("loading");
                },
                success: function (result) {

                    if (!result.success) {
                        self.error();
                    }
                    else {
                        self.success();
                    }
                },
                error: function (xhr, error) {
                    $.error(xhr.statusText);
                },
                complete: function () {
                    self.inputValidate.$element.removeClass("loading");
                }
            });
            return true;
        }
    };

    var ValidateStatus = {
        unValidate: 0,
        success: 1,
        error: -1,
        validating:2
    };

    function Validate(inputValidate, name, method, condition) {
        this.inputValidate = inputValidate;
        this.name = name;
        this.method = method;
        this.condition = condition;
        this.status = ValidateStatus.unValidate;
    };
    Validate.prototype.validate = function () {
        this.status = ValidateStatus.validating;
        this.method.call(this,this, this.inputValidate.$input.val());
    };
    Validate.prototype.success = function () {
        this.status = ValidateStatus.success;
        this.inputValidate.validateSuccess();
    };
    Validate.prototype.error = function () {
        this.status = ValidateStatus.error;
        this.inputValidate.validateError(this);
    };
    Validate.prototype.getErrorMessage = function () {
        return this.name+" is invalid";
    };

    function InputValidate($element, options) {
        var self = this;
        this.$element = $element;
        this.element = $element.get(0);
        this.settings = $.extend({}, this.defaultSettings, options);
        this.status = ValidateStatus.unValidate;//0未验证，1通过验证，-1验证失败,2验证中
        this.validates = [];//验证组
        this.initialize();
    };
    InputValidate.prototype.defaultSettings = {
        highlight:null,
        passCallback: function () {
            console.log("passed!");
        },
        unPassCallback: function () {
            console.log("unPassed!");
        },
        methods: methods
    };
    InputValidate.prototype.initialize = function () {
        var self = this;

        if (self.$element.is("input,textarea")) {
            this.$input = this.$element;
        }
        else if (self.$element.is(".input-wrapper")) {
            this.$input = this.$element.find("input,textarea");
        }

        if (this.validates.length == 0) {
            this.pass();
        }

        for (var i in methods) {

            if (typeof (this.$element.attr("data-" + i)) != "undefined") {
                this.validates.push(new Validate(this, i, methods[i], this.element.dataset[i]));
            }
        }
        this.bindEvent();
    };
    InputValidate.prototype.bindEvent = function () {
        var self = this;

        if (this.$input) {
            this.$input.on("input", function () {
                self.validate();
            }).on("focus", function () {
                self.reset();
            });
        }
    };
    InputValidate.prototype.validate = function () {
        this.reset();

        if (this.validates.length == 0) {
            this.validateSuccess();
        }

        for (var i = 0; i < this.validates.length; i++) {
            this.validates[i].validate();
        }
    };
    InputValidate.prototype.reset = function () {

        if(this.ajax){
            this.ajax.abort();
        }
        this.status=ValidateStatus.unValidate;
        this.$element.removeClass("error").removeClass("loading").removeClass("success");
    };
    InputValidate.prototype.pass=function(){
        this.settings.passCallback();
    };
    InputValidate.prototype.check = function () {

        if (this.validates.length) {

            for (var i = 0; i < this.validates.length; i++) {

                if (this.validates[i].status == ValidateStatus.validating) {
                    return false;
                }
                else if (this.validates[i].status == ValidateStatus.unValidate) {
                    return false;
                }
                else if (this.validates[i].status == ValidateStatus.error) {
                    this.status = ValidateStatus.error;
                    this.$element.removeClass("success").addClass("error");
                    return false;
                }
            }
        }
        this.status = ValidateStatus.success;
        this.$element.removeClass("error").addClass("success");
        this.pass();
        return true;
    };
    InputValidate.prototype.validateSuccess = function (validate) {
        this.check();
    };
    InputValidate.prototype.validateError = function (validate) {
        this.status == ValidateStatus.error;
        this.$element.addClass("error").removeClass("success").removeClass("loading");

        if (this.settings.highlight) {
            this.settings.highlight.call(this);
        }
        this.showMessage(validate.getErrorMessage());
        this.settings.unPassCallback();
    };
    InputValidate.prototype.showMessage=function(message){
        $.message(message);
    };

    $.fn.inputValidate=function(options){
        var self=this;

        var default_settings={

        };
        this.settings = $.extend({}, default_settings, options);
        this[0].inputValidate = this[0].inputValidate ? this[0].inputValidate : new InputValidate(this, this.settings);
        return this[0].inputValidate;
    };

    function FormValidate($element, options){
        var self = this;
        this.$element = $element;

        this.validateItems = [];
        this.status = 0;//0未验证，1通过验证，-1验证失败,2验证中
        this.settings = $.extend({}, this.defaultSettings, options);
        
        this.submiting = false;
        this.autoSubmit = false;
        this.initialized = false;
        this.initialize();
    };
    FormValidate.prototype.defaultSettings = {
        debug: false,
        methods: methods,
        highlight:null,
        onsubmit: null,
        lockSubmitHandler:false
    };
    FormValidate.prototype.initialize = function (){
        var self = this;

        // Add novalidate tag if HTML5.
        this.$element.attr("novalidate", "novalidate");

        this.$buttonSubmit = this.$element.find("[type='submit']").on("click", function (event){
            self.submit.call(self, event);
            event.stopPropagation();
            return false;
        });
        self.$element.submit(function (event) {
            return self.submitForm.call(self, event);
        });
        var $dataValidates = this.$element.find('[data-validate]');
        $dataValidates.each(function (index, element) {
            self.validateItems.push($(this).inputValidate({
                methods: self.settings.methods,
                highlight:self.settings.highlight,
                passCallback: function () {
                    self.check();
                },
                unPassCallback: function () {
                    self.unPass();
                }
            }));
        });

        if (this.settings.lockSubmitHandler) {
            this.disableGo();
        }
        this.initialized = true;
    };
    FormValidate.prototype.check = function () {
        
        if (!this.initialized) {
            return;
        }
        if (this.chainValidate()) {
            this.pass();
            return true;
        }
        return false;
    };
    FormValidate.prototype.unPass = function () {
        this.status = ValidateStatus.error;

        if (this.settings.lockSubmitHandler) {
            this.disableGo();
        }
    };
    FormValidate.prototype.pass = function (){
        this.status = ValidateStatus.success;

        if (this.settings.lockSubmitHandler){
            this.enableGo();
        }

        if (this.autoSubmit) {
            this.submitForm();
            this.autoSubmit = false;
        }
    };
    FormValidate.prototype.getData = function () {
        return this.$element.serializeObject();
    };
    FormValidate.prototype.submitForm= function (event) {

        if (this.submiting) {
            return false;
        }
        this.setLoading();

        if (this.settings.onsubmit) {
            this.settings.onsubmit.call(this, this.$element[0], event);
            return false;
        }
        this.$element.get(0).submit();
        return false;
    };
    FormValidate.prototype.submit = function (){

        if (this.validate(true)) {
            this.submitForm();
        }
    };
    FormValidate.prototype.disableGo=function(){
        this.$buttonSubmit.attr("disabled", true);
        return this;
    };
    FormValidate.prototype.enableGo = function () {

        if (!this.submiting) {
            this.$buttonSubmit.attr("disabled", false);
        }
        return this;
    };
    FormValidate.prototype.setLoading = function () {
        this.submiting = true;
        this.$buttonSubmit.addClass("button-loading").attr("disabled", true);
        return this;
    };
    FormValidate.prototype.clearLoading = function () {
        this.submiting = false;
        this.$buttonSubmit.removeClass("button-loading").attr("disabled", false);
        return this;
    };
    FormValidate.prototype.chainValidate = function () {

        if (this.validateItems.length == 0) {
            return true;
        }
        for (var i = 0; i < this.validateItems.length; i++) {

            if (this.validateItems[i].status == ValidateStatus.unValidate) {
                return false;
            }
            else if (this.validateItems[i].status == ValidateStatus.error) {
                return false;
            }
            else if (this.validateItems[i].status == ValidateStatus.validating) {
                return false;
            }
        }
        return true;
    };
    FormValidate.prototype.validate = function (autoSubmit) {
  
        if (this.validateItems.length == 0) {
            return true;
        }
        var unValidates = [];

        for (var i = 0; i < this.validateItems.length;i++) {
            
            if (this.validateItems[i].status == ValidateStatus.unValidate) {
                unValidates.push(this.validateItems[i]);
            }
            else if (this.validateItems[i].status == ValidateStatus.error) {
                return false;
            }
            else if (this.validateItems[i].status == ValidateStatus.validating) {
                return false;
            }
        }

        if (unValidates.length) {

            if (autoSubmit) {
                this.autoSubmit = true;
            }
            for (var i = 0; i < unValidates.length; i++) {
                unValidates[i].validate();
            }
            return false;
        }
        return true;
    };

    $.fn.formValidate=function(options){
        var self=this;

        // if nothing is selected, return nothing; can't chain anyway
        if (!this.length) {

            if ( options && options.debug && window.console ) {
                console.warn( "Nothing selected, can't validate, returning nothing." );
            }
            return;
        }
        var validator=this[0].formValidate;

        if(!validator){
            this[0].formValidate=validator=new FormValidate(this.eq(0),options);
        }
        return this[0].formValidate;
    };
}(jQuery));