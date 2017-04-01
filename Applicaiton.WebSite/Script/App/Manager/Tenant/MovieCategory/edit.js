(function () {
    var controllerId = 'app.manager.movieCategory.edit';
    appModule.controller(controllerId,
        ['$scope', 'infrastructure.services.app.movieCategory', '$stateParams', '$state',
            function ($scope, movieCategoryService, $stateParams, $state) {
                var vm = this;
                vm.movieCategory = null;
                movieCategoryService.get({ id: $stateParams.id }).success(function (result) {
                    vm.movieCategory = result;
                });
                vm.update = function () {
                    movieCategoryService.update(vm.movieCategory).success(function (result) {
                        infrastructure.notify.success('SavedSuccessfully');
                        infrastructure.event.trigger('infrastructure.updatemovieModalSaved');
                        $state.go("movieCategory.index");
                    }).error(function (result) {
                        infrastructure.notify.error(result.error.message);
                    });
                };
            }
        ]);
})();