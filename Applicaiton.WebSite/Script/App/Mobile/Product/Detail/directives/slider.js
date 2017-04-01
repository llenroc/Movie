/* Used by user and role permission settings. 
 */
appModule.directive('slider', [function () {
    return {
        scope: {
            productData: '='
        },
        link: function ($scope, element, attr) {
            $scope.$watch('data', function () {

                if (!$scope.productData || !$scope.productData.specifications.length) {
                    return;
                }
                $(element).slider({
                    enableControl:false
                });
            });
        }
    };
}]);