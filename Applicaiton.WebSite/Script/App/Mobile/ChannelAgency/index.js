(function () {
    appModule.controller('app.mobile.channelAgency.index',
        ['$scope', 'infrastructure.services.app.channelAgencyForFront', 'appSession',
        function ($scope, channelAgencyService, appSession) {
            var vm = this;
            vm.user = appSession.user;
            vm.channelAgency = null;
            vm.getChannelAgency = function () {
                channelAgencyService.getChannelAgency().success(function (result) {
                    vm.channelAgency = result;
                });
            }
            function initialize() {
                vm.getChannelAgency();
            }
            initialize();
        }
        ]);
})();