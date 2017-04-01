(function () {
    var controllerId = 'app.mobile.spread.qrcode';
    appModule.controller(controllerId, ['$scope', 'infrastructure.services.app.spread', 'appSession',
        function ($scope, spreadService, appSession) {
            var vm = this;
            vm.qrcode = null;
            vm.user = appSession.user;
            vm.getQrcode = function () {
                spreadService.getQrcode().success(function (result) {
                    vm.qrcode = result;
                });
            };
            function initiallize() {
                vm.getQrcode();
            }
            initiallize();
        }]);
})();