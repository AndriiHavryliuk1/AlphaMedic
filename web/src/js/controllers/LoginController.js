var app = angular.module('alphaMedicApp');

app.controller('LoginController', function($rootScope, $interval, $scope, sha256, $route, $http, $window, jwtHelper, USER_ROLES, URL_FOR_REST) {

    $scope.loading = false;

    $scope.submit = function() {
        $scope.failed = false;
        $scope.user.Password = sha256.convertToSHA256($scope.Password);
        var body = "grant_type=password&username=" + $scope.user.Email + "&password=" + $scope.user.Password + "&client=desktop";

        $http
            .post(URL_FOR_REST.url + "Token", body, {
                headers: {
                    'Content-Type': 'x-www-form-urlencoded'
                }
            })
            .success(function(data, status, headers, config) {
                if (data.access_token != undefined) {
                    localStorage.setItem('token', data.access_token);
                    $rootScope.isAuthorized = true;
                    var x = jwtHelper.decodeToken(data.access_token);
                    $rootScope.canRegister = x.role == USER_ROLES.patient;


                    switch (x.role) {
                        case USER_ROLES.admin:
                            {
                                $rootScope.isAdmin = true;
                                $window.location.href = "#/administratorCabinet";
                                break;
                            }
                        case USER_ROLES.receptionist:
                            {
                                $rootScope.isReceptionist = true;
                                $window.location.href = "#/receptionistCabinet";
                                break;
                            }
                        case USER_ROLES.patient:
                            {
                                $rootScope.isPatient = true;
                                $window.location.href = "#/patientCabinet";
                                break;
                            }
                        case USER_ROLES.doctor:
                            {
                                $rootScope.isDoctor = true;
                                $window.location.href = "#/doctorCabinet";
                                break;
                            }
                        case USER_ROLES.hospitalDean:
                            {
                                $rootScope.isDoctor = true;
                                $rootScope.isHospitalDean = true;
                                $window.location.href = "#/doctorCabinet";
                                break;
                            }
                        case USER_ROLES.headDepartment:
                            {
                                $rootScope.isDoctor = true;
                                $rootScope.isHeadDepartment = true;

                                $window.location.href = "#/doctorCabinet";
                                break;
                            }
                    }
                        $rootScope.$emit('navabarButtonsShow', x);
                }
            })
            .error(function(data, status, headers, config) {
                localStorage.removeItem('token');
                $rootScope.isAuthorized = false;
                $scope.failed = true;
            });
    };


    $scope.setCheck = function() {
        $scope.loading = true;
    }

    $scope.recoveryPass = function() {
        $scope.failed = false;
        $scope.loading = true;
        $http.post(URL_FOR_REST.url + "api/users/recovery?email=" + $scope.user.Email, {
                headers: {
                    'Content-type': 'application/json'
                }
            })
            .success(function(data, status, headers, config) {
                $scope.loading = false;
                alert("Your password has beed changed!");

                $window.location.href = "#/signIn";

            })
            .error(function(data, status, headers, config) {
                $scope.loading = false;
                $scope.failed = true;
                //  alert("Incorrect email or you don`t register!");
                $window.location.href = "#/recoveryPassword";

            })


    }
});
