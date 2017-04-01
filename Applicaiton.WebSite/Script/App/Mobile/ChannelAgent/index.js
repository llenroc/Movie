(function () {
    appModule.controller('app.mobile.channelAgent.index',
        ['$scope', 'infrastructure.services.app.channelAgentForFront', 'infrastructure.services.app.channelAgencyForFront', 'infrastructure.services.app.channelAgencyApplyOrder', 'appSession',
        function ($scope, channelAgentService,channelAgencyService,channelAgencyApplyOrderService, appSession) {
            var vm = this;
            vm.channelAgents = null;
            vm.channelAgency = null;
            vm.isApplying = false;
            vm.applyingChannelAgencyApply = null;
            vm.getMyChannelAgentInfo = function () {
                channelAgencyService.getMyChannelAgentInfo().success(function (result) {
                    vm.channelAgency = result.channelAgency;
                    vm.applyingChannelAgencyApply = result.applyingChannelAgencyApply;
                });
            };
            vm.isInApplying = function (channelAgent) {

                if (!vm.applyingChannelAgencyApply) {
                    return false;
                }
                else {
                    return vm.applyingChannelAgencyApply.channelAgentId == channelAgent.id;
                }
            };
            vm.viewOrder = function () {
                window.location.href = "/Mobile/Order/Detail?id=" + vm.applyingChannelAgencyApply.orderId;
            };
            vm.apply = function (channelAgent) {
                vm.isApplying = true;
                infrastructure.ui.setBusy();
                channelAgencyApplyOrderService.createOrder({
                    channelAgentId: channelAgent.id
                }).success(function (result) {
                    window.location.href = "/Mobile/Pay/Index?id=" + result.id;
                }).error(function () {
                    vm.isApplying = false;
                }).finally(function () {
                    infrastructure.ui.clearBusy();
                });
            };
            vm.canApply = function (channelAgent) {

                if (vm.applyingChannelAgencyApply) {
                    return false;
                }

                if (vm.channelAgency) {
                    return channelAgent.level < vm.channelAgency.channelAgent.level;
                }
                else {
                    return true;
                }
            };
            vm.getChannelAgents = function () {
                channelAgentService.getAll().success(function (result) {
                    vm.channelAgents = result.items;
                });
            };
            function initialize() {
                vm.getMyChannelAgentInfo();
                vm.getChannelAgents();
            }
            initialize();
        }
        ]);
})();