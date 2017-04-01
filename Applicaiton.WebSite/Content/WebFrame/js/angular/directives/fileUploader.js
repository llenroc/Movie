(function () {
    appModule.directive('fileUploader', function () {
        return {
            restrict: 'A',
            scope: {
                path:"="
            },
            link: function ($scope, element) {
                var $fileUploader = element.fileUploader();
                $fileUploader.setPath = function (path) {
                    $scope.$apply(function () {
                        $scope.path = path;
                    })
                    
                }
            }
        };
    });
})();