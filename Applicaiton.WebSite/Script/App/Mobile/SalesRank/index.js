(function () {
    var controllerId = 'app.mobile.salesRank.index';
    appModule.controller(controllerId, ['$scope', 'infrastructure.services.app.userForFront' , 'appSession',
        function ($scope, userService, appSession) {
            var vm = this;
            vm.myRank=null;
            vm.users = null;
            vm.user = appSession.user;
            vm.getRankInfo = function () {
                userService.getRankInfo().success(function (result) {
                    vm.myRank = result.myRank;
                    vm.users = result.items;
                });
            };
            function initialize() {
                vm.getRankInfo();
            }
            initialize();
        }]);
})();