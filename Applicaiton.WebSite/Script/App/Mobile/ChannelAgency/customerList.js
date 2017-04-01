(function () {
    appModule.controller('app.mobile.channelAgency.customerList',
        ['$scope', 'infrastructure.services.app.channelAgencyForFront', 'appSession',
        function ($scope, channelAgencyService, appSession) {
            var vm = this;
            vm.customers = null;
            vm.depth = document.getElementById("depth").value;
            var $channelAgencyCustomerList = $('#customerList');

            $customerList.table({
                paging: true,
                pageSize: 5,
                initializeLoad: false,
                actions: {
                    listAction: {
                        method: channelService.getCustomersOfChannelAgency,
                        beforeCallback: function () {
                            infrastructure.ui.setBusy();
                        },
                        callback: function (result) {
                            infrastructure.ui.clearBusy();
                            vm.customers = result.items;
                        },
                        getTotalCountFromResult: function (result) {
                            return result.totalCount;
                        }
                    }
                }
            });
            function createRequestParams() {
                var params = { depth: vm.depth };
                return params;
            }
            vm.getCustomers = function () {
                $customerList.table().load(createRequestParams());
            };
            function initialize() {
                vm.getCustomers();
            }
            initialize();
        }
        ]);
})();