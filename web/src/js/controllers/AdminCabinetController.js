var app = angular.module('alphaMedicApp');
app.controller('AdminCabinetController', function(URL_FOR_REST, $scope, $rootScope, FilterService, $location, $http, ChangeUserInfoService, PagginationService, changeStateService, jwtHelper) {
    $rootScope.needSearch = $rootScope.needSearch != undefined ? $rootScope.needSearch : true;
    $rootScope.selectedView = $rootScope.selectedView != undefined ? $rootScope.selectedView : 'doctorsView';
    $scope.searchUser = "";
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
        ChangeUserInfoService.ChangePassword($scope.ChangePass, jwtHelper.decodeToken(localStorage.getItem('token')).id,$scope);
    };

    $scope.AdminFilter = function() {

        switch ($rootScope.selectedView) {
            case 'doctorsView':
                $rootScope.search($rootScope.loadAdminList);
                break;
            case 'patientsView':
                $rootScope.search($rootScope.loadAdminListPatient);
                break;
        }



    }

    $scope.selectView = function(data, params) {
        $location.search('search', null);
        $location.search('isActive', null);
        FilterService.setPage(1);
        $rootScope.selectedView = data;
      //  FilterService.setAdminCab(data);
        $rootScope.needSearch = params;

    };
});
