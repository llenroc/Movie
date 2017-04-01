(function () {
    var controllerId = 'app.manager.expressCompany.edit';
    appModule.controller(controllerId,
        ['$scope', 'infrastructure.services.app.expressCompany', '$stateParams', '$state',
            function ($scope, expressCompanyService, $stateParams, $state) {
            var vm = this;
            vm.expressCompany = null;
            expressCompanyService.get({ id: $stateParams.id }).success(function (result) {
                vm.expressCompany = result;
            });
            vm.update = function (){
                expressCompanyService.update(vm.expressCompany).success(function (result) {
                    infrastructure.notify.success(app.localize('SavedSuccessfully'));
                    $state.go("expressCompany.index");
                }).error(function (result) {
                    infrastructure.notify.error(result.error.message);
                });
            };
        }
        ]);
})();