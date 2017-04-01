(function () {
    appModule.controller('app.manager.movieCategory.index', ['$scope', 'infrastructure.services.app.movieCategory',
        function ($scope, moiveService) {
            var vm = this;
            vm.movieCategorys = [];

            function getmovieCategorys() {
                moiveService.getAll({}).success(function (result) {
                    vm.movieCategorys = result.items;
                });
            }
            getmovieCategorys();
        }
    ]);
})();