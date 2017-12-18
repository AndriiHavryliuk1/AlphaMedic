var app = angular.module('alphaMedicApp');

app.controller('ApplicationController', function($rootScope, $route, $scope, $location, $http, $window, jwtHelper, USER_ROLES,URL_FOR_REST) {
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

    $rootScope.getPath = function() {
        var res;
        switch ($location.path()) {
            case "/administratorCabinet":
            case "/receptionistCabinet":
            case "/patientCabinet":
            case "/doctorCabinet":
                res = "/Cabinet";
                break;
            default:
                res = $location.path();
        }
        return res;
    }


    $scope.$on('$locationChangeSuccess', function(event) {
        $rootScope.getPath2 = $location.path();
    });

    $scope.getDoctorId= function(id)
    {
      $http.get(URL_FOR_REST.url + "api/doctors/"+id)
      .success(function(responce) {
          $rootScope.ScheduleId = responce.ScheduleId;
      })
    }

    if ($rootScope.isAuthorized) {
        var x = jwtHelper.decodeToken(localStorage.getItem('token'));
        switch (x.role) {
            case USER_ROLES.admin:
                {
                    $rootScope.isAdmin = true;
                    break;
                }
            case USER_ROLES.receptionist:
                {
                    $rootScope.isReceptionist = true;
                    break;
                }
            case USER_ROLES.patient:
                {
                    $rootScope.isPatient = true;
                    break;
                }
            case USER_ROLES.doctor:
                {
                    $rootScope.isDoctor = true;
                    $scope.getDoctorId(x.id);
                    break;
                }
            case USER_ROLES.hospitalDean:
                {
                    $rootScope.isDoctor = true;
                    $rootScope.isHospitalDean = true;
                    $scope.getDoctorId(x.id);
                    break;
                }
            case USER_ROLES.headDepartment:
                {
                    $rootScope.isDoctor = true;
                    $rootScope.isHeadDepartment = true;
                    $scope.getDoctorId(x.id);
                    break;
                }
        }
    }


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
