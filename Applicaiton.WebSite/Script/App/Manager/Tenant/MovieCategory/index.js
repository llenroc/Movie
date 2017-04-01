(function () {
    appModule.controller('app.manager.movieCategory.index',
        ['$scope', 'infrastructure.services.app.movieCategory',
        function ($scope, moiveService) {
            var vm = this;
            vm.movieCategorys = [];
            vm.delete = function (index) {
                moiveService.delete({id:vm.movieCategorys[index].id}).success(function (result) {
                    vm.movieCategorys.splice(index, 1);
                });
            };
            vm.getMovieCategorys=function() {
                moiveService.getAll({}).success(function (result) {
                    vm.movieCategorys = result.items;
                });
            }
            vm.getMovieCategorys();
        }
        ]);
})();