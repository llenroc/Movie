(function () {
    var controllerId = 'app.manager.product.index';
    appModule.controller(controllerId,
        ['$scope', 'infrastructure.services.app.productForTenant', '$state', function ($scope, productForTenantService, $state) {
            var vm = this;
            vm.products = [];
            var $productTable = $('#productTable');
            var productTable = $productTable.table({
                paging: true,
                sorting: true,
                multiSorting: true,
                actions: {
                    listAction: {
                        method: productForTenantService.getAllOfPage,
                        callback: function (result) {
                            vm.products = result.items;
                        },
                        getTotalCountFromResult: function (result) {
                            return result.totalCount;
                        }
                    }
                },
                fields: {
                    name: {                                                                                            
                    },
                    specifications: {
                        disabled:true
                    },
                    creationTime: {
                    },
                    status: {
                    },
                }
            });
            vm.getProductLink = function (product) {
                return "http://" + window.location.host + "/Mobile/Product/Detail?id=" + product.id;
            };
            vm.delete = function (product) {
                infrastructure.message.confirm(
                    app.localize('UserProductWarningMessage', product.name),
                    function () {
                        productForTenantService.delete({
                            id: product.id
                        }).then(function () {
                            productTable.load();
                            infrastructure.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                );
            };
            vm.onProduct = function (product) {
                productForTenantService.onProduct({
                    id: product.id
                }).then(function () {
                    productTable.load();
                    infrastructure.notify.success(app.localize('SuccessfullySet'));
                });
            };
            vm.offProduct = function (product) {
                productForTenantService.offProduct({
                    id: product.id
                }).then(function () {
                    productTable.load();
                    infrastructure.notify.success(app.localize('SuccessfullySet'));
                });
            };
        }
    ]);
})();