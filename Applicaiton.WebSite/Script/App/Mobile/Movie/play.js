(function () {
    var controllerId = 'app.mobile.movie.play';
    appModule.controller(controllerId, ['$scope', 'infrastructure.services.app.movie', '$sce', function ($scope, movieService, $sce) {

        //controller 依赖 $scope, $sce
        $scope.trustSrc = function (url) {
            return $sce.trustAsResourceUrl(url);
        }
        var vm = this;
        vm.id = document.getElementById("id").value;
    }]);
})();