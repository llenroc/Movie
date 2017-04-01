(function () {
    var controllerId = 'app.manager.channel.channelAgent.createOrEdit';
    appModule.controller(controllerId,
        ['$scope', 'infrastructure.services.app.channelAgentForEnd', '$stateParams', '$state',
            function ($scope, channelAgentService, $stateParams, $state) {
                var vm = this;
                vm.channelAgent = null;
                vm.saving = false;
                function init() {
                    channelAgentService.getChannelAgentForCreateOrEdit({
                        id: $stateParams.id
                    }).success(function (result) {
                        vm.channelAgent = result;
                    });
                }
                init();
                vm.createOrEdit = function () {
                    vm.saving = true;
                    channelAgentService.createOrEdit(vm.channelAgent).then(function () {
                        infrastructure.notify.success(app.localize('SaveSuccessfully'));
                        $state.go("channel.channelAgent.index");
                    }).finally(function () {
                        vm.saving = false;
                    });
                };
            }
        ]);
})();