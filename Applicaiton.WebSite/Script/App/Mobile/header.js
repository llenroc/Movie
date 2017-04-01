(function () {
    var controllerId = 'app.mobile.header';
    appModule.controller(controllerId, ['$scope', 'appSession', function ($scope, appSession) {
        var vm = this;
        vm.currentUser = appSession.user;
        vm.currentUserAvatar = vm.currentUser ? vm.currentUser.avatar : "/Content/Images/avatar.png";
    }]);
})();