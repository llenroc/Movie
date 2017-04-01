(function () {
    var controllerId = 'app.manager.order.detail';
    appModule.controller(controllerId,
        ['$scope', 'infrastructure.services.app.orderAdmin', '$state', '$stateParams',
            function ($scope, orderService, $state, $stateParams) {
                var vm = this;
                vm.order = null;
                function initialize() {
                    orderService.get({ id: $stateParams.id }).success(function (result) {
                        vm.order = result;
                    });
                }
                initialize();
            }
        ]);
})();