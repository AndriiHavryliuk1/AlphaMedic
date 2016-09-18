var app = angular.module('alphaMedicApp');

app.controller('ApplicationController', function($rootScope, $route, $scope, $location, $http, $window, jwtHelper, USER_ROLES) {
    $scope.LogOut = function() {

        $rootScope.isAuthorized = false;
        localStorage.removeItem('token');
        //localStorage.$reset();
        $window.location.href = "#/main";
        $rootScope.isDoctor = false;
        $rootScope.isPatient = false;
        $rootScope.isReceptionist = false;
        $rootScope.isAdmin = false;
        $rootScope.canRegister = true;
        $rootScope.isHeadDepartment = false;
        $rootScope.isHospitalDean = false;
        $route.reload();
        $rootScope.$emit('navabarButtonsShow', {
            role: "guest"
        });
    };

    $rootScope.getPath = function () {
          return $location.path();
}


/*
    $scope.GetChecked = function() {
        var res = "";
        switch ($location.path()) {
            case "/signUp":
                res = "signUp";
                break;
            case "/signIn":
                res = "signIn";
                break;
            case "/registrationForAppointment":
                res = "registrationForAppointment";
                break;
            case "/schedule":
                res = "schedule";
                break;
            case "/doctors":
                res = "doctors";
                break;
            case "/departments":
                res = "departments";
                break;
            case "/patients":
                res = "patients";
                break;
            case "/administratorCabinet":
            case "/receptionistCabinet":
            case "/patientCabinet":
            case "/doctorCabinet":
                res = "Cabinet";
                break;

            default:
                res = "";
                break;
        }
        return res;
        //  Object.getPrototypeOf($route.current) === $route.routes["/users/:id"];
        //    return FilterService.getNavCheck();
    };
*/
    $scope.Cabinet = function() {
        var x = jwtHelper.decodeToken(localStorage.getItem('token'));
        switch (x.role) {
            case USER_ROLES.admin:
                $window.location.href = "#/administratorCabinet";
                break;
            case USER_ROLES.receptionist:
                $window.location.href = "#/receptionistCabinet";
                break;
            case USER_ROLES.patient:
                $window.location.href = "#/patientCabinet";
                break;
            case USER_ROLES.doctor:
                $window.location.href = "#/doctorCabinet";
                break;
            case USER_ROLES.hospitalDean:
                $window.location.href = "#/doctorCabinet";
                break;
            case USER_ROLES.headDepartment:
                $window.location.href = "#/doctorCabinet";
                break;
        }
    };
});
