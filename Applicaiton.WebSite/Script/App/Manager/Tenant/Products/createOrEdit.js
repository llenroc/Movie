(function () {
    var controllerId = 'app.manager.product.createOrEdit';
    appModule.controller(controllerId,
        ['$scope', '$uiModal', 'infrastructure.services.app.productForTenant', '$stateParams', '$state',
            function ($scope,$uiModal, productForTenantService, $stateParams, $state) {
                var vm = this;
                vm.product = null;
                vm.productCategorys = [];
                vm.templates = [{
                    id:1,
                    name:app.localize("DetailTemplate"),
                }, {
                    id: 2,
                    name: app.localize("ArticleTemplate"),
                }];
                vm.virtualProductTypes = [{
                    value: 0,
                    name: app.localize("VirtualCard"),
                }, {
                    value: 1,
                    name: app.localize("Coupons"),
                }];
                vm.saving = false;
                vm.buyWhens = [
                    { name: "NoLimit", value: 0 },
                    { name: "First", value: 1 },
                    { name: "Next", value: 2 },
                ];
                var $editor=$("#content").richTextEditor({
                    onImageUpload: function (files, editor) {
                        infrastructure.ui.setBusy();
                        var formData = new FormData();
                        var file = files[0];
                        formData.append("file", file);
                        var xhr = new XMLHttpRequest();
                        xhr.open('POST', "/Manager/Home/Upload", true);
                        xhr.onreadystatechange = function () {

                            if (xhr.readyState == 4) {

                                if (xhr.status == 200) {
                                    var result = JSON.parse(xhr.responseText);
                                    var path;

                                    if (result.__infrastructure) {
                                        path = result.result.path;
                                    }
                                    else {
                                        path = result.path;
                                    }
                                    editor.insertImage(path);
                                }
                            }
                            infrastructure.ui.clearBusy();
                        }
                        xhr.send(formData);
                    }
                });
                vm.specificationPropertys = [];
                vm.specifications = [];
                vm.distributions = [];
                vm.openMediaIdOfImageSelector = function () {
                    var modalInstance = $uiModal.open({
                        fullPage: true,
                        autoCreateFrame: false,
                        templateUrl: '/Areas/Manager/Views/Tenant/PublicWechat/Common/mediaIdOfImageSelectorModal.cshtml',
                        controller: "app.manager.publicWechat.common.mediaIdOfImageSelectorModal as vm",
                    });
                    modalInstance.result.then(function (result) {
                        vm.product.masterQrcode = result.media_id;
                    });
                };
                vm.getSpecificationPropertyName = function (propertyValue) {

                    vm.specificationPropertys.forEach(function (specificationProperty) {

                        if (specificationProperty.id == propertyValue.specificationPropertyId) {
                            return specificationProperty.name;
                        }
                    })
                };
                vm.addSpecificationProperty = function (specificationProperty) {

                    if (!specificationProperty) {
                        return;
                    }

                    for (var i = 0; i < vm.product.specificationPropertys.length; i++) {

                        if (vm.product.specificationPropertys[i].id == specificationProperty.id) {
                            return false;
                        }
                    }
                    vm.product.specificationPropertys.push(specificationProperty);
                    vm.specifications.forEach(function (specification) {
                        specification.propertyValues.push({
                            specificationProperty: specificationProperty,
                            specificationPropertyId: specificationProperty.id,
                            value: ""
                        });
                    });
                };
                vm.removeSpecificationProperty = function (index) {

                    if (vm.specifications.length) {
                        infrastructure.message.error(app.localize("ThereHasSpecification"));
                        return;
                    }
                    infrastructure.message.confirm(app.localize("AreYouSure"), function () {

                        $scope.$apply(function () {
                            vm.product.specificationPropertys.splice(index, 1);
                        });
                    });
                };
                vm.addSpecification = function () {
                    var specification = {
                        stock: 0,
                        propertyValues: []
                    };
                    vm.product.specificationPropertys.forEach(function (specificationProperty) {
                        specification.propertyValues.push({
                            specificationProperty: specificationProperty,
                            specificationPropertyId: specificationProperty.id,
                            value: ""
                        });
                    });
                    vm.specifications = vm.specifications || [];
                    vm.specifications.push(specification);
                };
                vm.removeSpecification = function (index) {
                    var specification = vm.specifications[index];

                    infrastructure.message.confirm(app.localize("AreYouSure"), function () {

                        $scope.$apply(function () {

                            if (specification.id) {
                                productForTenantService.removeSpecification({
                                    id: specification.id
                                }).then(function (result) {
                                    vm.specifications.splice(index, 1);
                                });
                            }
                            else {
                                vm.specifications.splice(index, 1);
                            }
                            vm.specifications.splice(index, 1);
                        });
                    });
                };
                vm.addDistribution = function () {
                    var distribution = {
                        level: 0,
                        money: 0,
                        ratio: 0
                    };
                    vm.distributions = vm.distributions || [];
                    vm.distributions.push(distribution);
                };
                vm.removeDistribution = function (index) {
                    var distribution = vm.distributions[index];

                    if (distribution.id) {
                        productForTenantService.removeDistribution({
                            id: distribution.id
                        }).then(function (result) {
                            vm.distributions.splice(index, 1);
                        });
                    }
                    else {
                        vm.distributions.splice(index, 1);
                    }
                };
                vm.createOrEdit = function () {
                    vm.saving = true;
                    var product = vm.product;

                    product.content = $editor.getContent();
                    productForTenantService.createOrEdit({
                        product: vm.product,
                        specifications: vm.specifications,
                        distributions: vm.distributions
                    }).then(function () {
                        infrastructure.notify.success(app.localize('SaveSuccessfully'));
                        $state.go("product.index");
                    }).finally(function () {
                        vm.saving = false;
                    });
                };

                function init() {
                    productForTenantService.getProductForCreateOrEdit({
                        id: $stateParams.id
                    }).then(function (result) {
                        vm.product = result.data.product;
                        $editor.setContent(vm.product.content);
                        vm.product.specificationPropertys = vm.product.specificationPropertys ? vm.product.specificationPropertys : [];
                        vm.productCategorys = result.data.productCategorys;
                        vm.specificationPropertys = result.data.specificationPropertys;
                        vm.specifications = result.data.specifications || [];
                        vm.distributions = result.data.distributions;
                    });
                }
                init();
            }
        ]);
})();