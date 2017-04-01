(function () {
    var controllerId = 'app.manager.movie.create';
    appModule.controller(controllerId,
        ['$scope', 'infrastructure.services.app.movie', 'infrastructure.services.app.movieCategory', '$stateParams', '$state', function ($scope, movieService,movieCategoryService, $stateParams, $state) {
            var vm = this;
            vm.movie = {};
            var $movieInformationForm = $("form");

            vm.movieCategorys = null;
            movieCategoryService.getAll().success(function (result) {
                vm.movieCategorys = result.items;
            });

            vm.create = function () {
                var movie = $movieInformationForm.serializeObject();
                movieService.create(movie).success(function () {
                    infrastructure.notify.info('SavedSuccessfully');
                    infrastructure.event.trigger('infrastructure.createMoiverModalSaved');
                    $state.go("movie");
                });
            };
        }
        ]);
})();