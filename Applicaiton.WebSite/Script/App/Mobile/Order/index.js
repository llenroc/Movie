(function () {
    var controllerId = 'app.mobile.order.index';
    appModule.controller(controllerId, ['$scope', 'infrastructure.services.app.order',
        function ($scope,orderService) {
            var vm = this;
            vm.orders = [];
            vm.orderStatus = 1;
            var $ordersList = $('#ordersList');

            $ordersList.table({
                paging: true,
                pageSize: 5,
                initializeLoad:false,
                actions: {
                    listAction: {
                        method: orderService.getAllOfPage,
                        beforeCallback:function(){
                            infrastructure.ui.setBusy();
                        },
                        callback: function (result) {
                            infrastructure.ui.clearBusy();
                            vm.orders = result.items;
                        },
                        getTotalCountFromResult: function (result) {
                            return result.totalCount;
                        }
                    }
                }
            });

            function createRequestParams(orderStatus) {
                vm.orderStatus = orderStatus;
                var params = { orderStatus:orderStatus};
                return params;
            }
            vm.getOrders = function (orderStatus) {
                $ordersList.table().load(createRequestParams(orderStatus));
            };
            function initialize() {
                vm.getOrders(vm.orderStatus);
            };
            initialize();
        }]);
})();