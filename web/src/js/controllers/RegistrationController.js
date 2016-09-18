var app = angular.module('alphaMedicApp');

app.controller('RegistrationController', function(URL_FOR_REST, USER_ROLES, sha256, SendConfirmMesErrorService, $scope, $http, $window, $rootScope, fileUploadService) {

    $scope.loading = false;

    $scope.submit = function() {
        $scope.loading = true;
        $scope.patient.Active = false;
        $scope.patient.Password = sha256.convertToSHA256($scope.patient.Password);
        $http.post(URL_FOR_REST.url + "api/Patients", $scope.patient, {
                headers: {
                    'Content-type': 'application/json'
                }
            })
            .success(function(data, status, headers, config) {
                var user = data;

                $http.post(URL_FOR_REST.url + "api/Patients/" + data.UserId + "/confirmRegistration", user, {
                        headers: {
                            'Content-type': 'application/json'
                        }
                    })
                    .success(function(data, status, headers, config) {
                        $window.location.href = "#/confirmRegistration";
                        $scope.loading = false;
                    })
                    .error(function(data, status, headers, config) {
                        SendConfirmMesErrorService.SendMailError(user.UserId);
                        $scope.loading = false;
                        //  alert("We have some trouble with our server please try again!")
                    });
                $window.location.href = "#/confirmRegistration";
            })
            .error(function(data, status, headers, config) {
                localStorage.removeItem('token');
                $rootScope.isAuthorized = false;
                $scope.failed = true;
                $scope.loading = false;
            });
    };

});
