(function () {
    var controllerId = 'app.mobile.product.index';
    appModule.controller(controllerId, ['$scope', 'infrastructure.services.app.productForFront', 'infrastructure.services.app.shopCartForFront', 'infrastructure.services.app.session', 'infrastructure.services.app.share',
        function ($scope, productService, shopCartService, sessionService, shareService) {
            var vm = this;
            vm.products = null;
            vm.shop =null;
            vm.getProducts = function () {
                productService.getAllOfPage({
                    pageIndex:1
                }).success(function (result) {
                    vm.products = result.items;
                });
            };
            vm.initializeShare = function () {
                sessionService.getShopInformations().success(function (result) {
                    vm.shop = result;
                    var sharePicture = vm.shop.sharePicture;
                    sharePicture = "http://" + window.location.host + sharePicture;
                    shareService.getPreShare().success(function (result) {
                        initShare({
                            preShareData: result,
                            title: vm.shop.shareTitle,
                            imgUrl: sharePicture,
                            desc: vm.shop.shareDescription,
                        });
                    });
                });
            };
            function initialize() {
                vm.getProducts();
                vm.initializeShare();
            }
            initialize();
        }]);
})();