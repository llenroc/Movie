(function () {
    appModule.directive('pictureUploader', function () {
        return {
            restrict: 'A',
            scope: {
                path:"="
            },
            link: function ($scope, element) {
                var $pictureUploader = element.pictureUploader();
                $pictureUploader.setPath = function (path) {
                    $scope.$apply(function () {
                        $scope.path = path;
                    })
                    
                }
            }
        };
    });
})();