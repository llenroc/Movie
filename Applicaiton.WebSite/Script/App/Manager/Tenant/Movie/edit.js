(function () {
    var controllerId = 'app.manager.movie.edit';
    appModule.controller(controllerId,
        ['$scope', 'infrastructure.services.app.movie', 'infrastructure.services.app.movieCategory', '$stateParams', '$state',
            function ($scope, movieService, movieCategoryService, $stateParams, $state) {
                var vm = this;
                vm.movie = null;
                vm.movieCategorys = null;
                movieService.get({ id: $stateParams.id }).success(function (result) {
                    vm.movie = result;
                });
                movieCategoryService.getAll().success(function (result) {
                    vm.movieCategorys = result.items;
                });
                vm.addPicture = function () {

                    if (vm.movie.pictures == null) {
                        vm.movie.pictures = [];
                    }
                    vm.movie.pictures.push({
                        path: ""
                    });
                };
                vm.update = function () {
                    movieService.update(vm.movie).success(function (result) {
                        infrastructure.notify.success('SavedSuccessfully');
                        infrastructure.event.trigger('infrastructure.updatemovieModalSaved');
                        $state.go("movie.index");
                    }).error(function (result) {
                        infrastructure.notify.error(result.error.message);
                    });
                };
            }
        ]);
})();