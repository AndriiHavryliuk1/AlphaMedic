var app = angular.module('alphaMedicApp');
app.controller('AdminCabinetController', function(URL_FOR_REST, $scope, $rootScope, FilterService, $location, $http, ChangeUserInfoService, PagginationService, changeStateService, jwtHelper) {
    $scope.needSearch = true;
    $scope.selectedView = "doctorsView";
    $scope.searchUser = "";
    $scope.isActiveShow = true;
    $scope.ChangePass = {
        OldPass: null,
        NewPass: null
    }

    $http.get(URL_FOR_REST.url + "api/employees/" + jwtHelper.decodeToken(localStorage.getItem('token')).id)
        .success(function(responce) {
            $scope.admin = responce;
        });

    $scope.ChangeState = function(user, state, controllerUrl) {
        changeStateService.patch(user, state, URL_FOR_REST.url + controllerUrl, $http);
    };

    $scope.ChangePassword = function() {
        ChangeUserInfoService.ChangePassword($scope.ChangePass, jwtHelper.decodeToken(localStorage.getItem('token')).id);
    };

    $scope.AdminFilter = function() {
        if ($scope.selectedView == 'doctorsView') {
            $rootScope.search($rootScope.loadAdminList);
        }
    }

    $scope.selectView = function(data, params) {
        $location.search('search', null);
        $location.search('isActive', null);
        FilterService.setPage(1);
        $scope.selectedView = data;
        $scope.isActiveShow = params;
    };
});
