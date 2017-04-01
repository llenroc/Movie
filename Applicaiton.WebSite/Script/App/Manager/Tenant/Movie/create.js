(function () {
    var controllerId = 'app.manager.movie.create';
    appModule.controller(controllerId,
        ['$scope', 'infrastructure.services.app.movie', 'infrastructure.services.app.movieCategory', '$stateParams', '$state',
            function ($scope, movieService, movieCategoryService, $stateParams, $state) {
                var vm = this;
                vm.movie = {
                    isCode: false,
                    shouldBeMemberForPlay:false
                };
                vm.movieCategorys = null;
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
                vm.create = function () {
                    movieService.create(vm.movie).success(function (result) {
                        infrastructure.notify.info('SavedSuccessfully');
                        infrastructure.event.trigger('infrastructure.createMoiverModalSaved');
                        $state.go("movie.index");
                    });
                };
            }
        ]);
})();