(function () {
    var controllerId = 'app.mobile.layout';
    appModule.controller(controllerId, ['$scope', function ($scope) {
        var vm = this;
        vm.footerMenu = infrastructure.nav.menus.FooterMenu;
        initializeWxJs();
    }]);
})();