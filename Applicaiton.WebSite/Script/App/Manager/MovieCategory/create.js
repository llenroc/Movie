(function () {
    var controllerId = 'app.manager.movieCategory.create';
    appModule.controller(controllerId,
        ['$scope', 'infrastructure.services.app.movieCategory', '$stateParams', '$state', function ($scope, movieCategoryService, $stateParams, $state) {
            var vm = this;
            var $movieCategoryCreateForm = $("form");
            vm.create = function () {
                var movieCategory = $movieCategoryCreateForm.serializeObject();
                movieCategoryService.create(movieCategory).success(function () {
                    infrastructure.notify.info('SavedSuccessfully');
                    infrastructure.event.trigger('infrastructure.createMoiverModalSaved');
                    $state.go("movieCategory");
                });
            };
        }
        ]);
})();