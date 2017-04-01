(function () {
    var controllerId = 'app.manager.tenant.setting.index';
    appModule.controller(controllerId,
        ['$scope', 'infrastructure.services.app.tenantSettings', function ($scope, tenantSettingsService) {
            var vm = this;
            vm.settings = null;
            vm.distributionWhenEnum = {
                Payed: "Payed",
                Receipt: "Receipt",
                Complete: "Complete"
            };
            vm.decreaseStockWhen = {
                Create: "Create",
                Pay: "Pay",
            };
            vm.loading = false;
            vm.getSettings = function () {
                vm.loading = true;
                tenantSettingsService.getAllSettings().then(function (result) {
                    vm.settings = result.data;
                }).finally(function () {
                    vm.loading = false;
                });
            };

            vm.saveAll = function () {
                tenantSettingsService.updateAllSettings(vm.settings).then(function () {
                    infrastructure.notify.info(app.localize('SavedSuccessfully'));
                });
            };
            vm.init = function () {
                vm.getSettings();
            };
            vm.init();
        }
        ]);
})();