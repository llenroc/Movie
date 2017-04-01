var app = app || {};
(function () {
    var appLocalizationSource = infrastructure.localization.getSource('Application');
    app.localize = function () {
        return appLocalizationSource.apply(this, arguments);
    };
})();