(function () {
    var controllerId = 'app.mobile.walletRecord.index';
    appModule.controller(controllerId, ['$scope', 'infrastructure.services.app.walletRecord',
        function ($scope, walletRecordService) {
            var vm = this;
            vm.walletRecords = [];
            var $walletRecordList = $('#walletRecordList');

            $walletRecordList.table({
                paging: true,
                actions: {
                    listAction: {
                        method: walletRecordService.getAllOfPage,
                        callback: function (result) {
                            vm.walletRecords = result.items;
                        },
                        getTotalCountFromResult: function (result) {
                            return result.totalCount;
                        }
                    }
                }
            });
            vm.localize = function (text) {
                return app.localize(text);
            };
            function createRequestParams() {
                var params = {};
                $filterForm.serializeArray().map(function (x) { params[x.name] = x.value; });
                return params;
            }
        }]);
})();