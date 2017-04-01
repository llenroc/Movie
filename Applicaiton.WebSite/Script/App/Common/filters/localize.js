(function () {
    appModule.filter('localize', function () {
        return function (date) {
            return app.localize(date);
        };
    });
})();