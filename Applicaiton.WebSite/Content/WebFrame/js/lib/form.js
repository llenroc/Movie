(function ($) {

    if (!$) {
        return;
    }

    $.fn.serializeObject = function () {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {

            if (o[this.name]) {

                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            }
            else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    }

    $.fn.serializeFormToObject = function () {
        //serialize to array
        var data = $(this).serializeArray();

        //add also disabled items
        $(':disabled[name]', this)
            .each(function () {
                data.push({ name: this.name, value: $(this).val() });
            });

        //map to object
        var obj = {};
        data.map(function (x) { obj[x.name] = x.value; });

        return obj;
    };

    $.extend({
        StandardPost: function (url, args) {
            var body = $(document.body),
                form = $("<form method='post'></form>"),
                input;
            form.attr({ "action": url });
            $.each(args, function (key, value) {
                input = $("<input type='hidden'>");
                input.attr({ "name": key });
                input.val(value);
                form.append(input);
            });
            form.appendTo(document.body);
            form.submit();
            document.body.removeChild(form[0]);
        }
    });
})(jQuery);