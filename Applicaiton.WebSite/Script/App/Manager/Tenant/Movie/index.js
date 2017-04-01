(function () {
    appModule.controller('app.manager.movie.index', ['$scope', 'infrastructure.services.app.movie',
        function ($scope, moiveService) {
            var vm = this;
            vm.movies = [];

            function getMovies() {
                moiveService.getAll({}).success(function (result) {
                    vm.movies = result.items;
                });
            }
            getMovies();
        }
    ]);
})();