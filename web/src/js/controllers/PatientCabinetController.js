var app = angular.module('alphaMedicApp');

app.controller('patientCabinet', function(URL_FOR_REST, $scope, $http, $routeParams,$filter, jwtHelper, $location, $window, ChangeUserInfoService, fileUploadService) {

    $scope.ChangePass = {
        OldPass: null,
        NewPass: null
    }

    $http.get(URL_FOR_REST.url + "api/patients/" + jwtHelper.decodeToken(localStorage.getItem('token')).id)
        .success(function(responce) {
            $scope.patient = responce;
            $scope.user = {
                UserId: $scope.patient.UserId,
                Name: $scope.patient.Name,
                Surname: $scope.patient.Surname,
                Gender: $scope.patient.Gender,
                DateOfBirth: $scope.patient.DateOfBirth,
                Phone: $scope.patient.Phone,
                Address: $scope.patient.Address
            }
        });


    $http.get(URL_FOR_REST.url + "api/patients/" + jwtHelper.decodeToken(localStorage.getItem('token')).id + "/appointments")
        .success(function(responce) {
            $scope.patientAppointments = responce;
            $scope.AnyAppointments = responce == null;
        });

    $scope.ChangePassword = function() {
        ChangeUserInfoService.ChangePassword($scope.ChangePass, jwtHelper.decodeToken(localStorage.getItem('token')).id);
    }




    $scope.ChangeUser = function() {
      //upload photo
        if ($scope.picture != "undefined") {
            fileUploadService.uploadFileToUrl($scope.picture, URL_FOR_REST.url + "api/image/" +
            jwtHelper.decodeToken(localStorage.getItem('token')).id).then(function(response) {
                $scope.patient.URLImage = response.data;
            });
        }


        ChangeUserInfoService.ChangeUser($scope.patient, $scope.user, jwtHelper.decodeToken(localStorage.getItem('token')).id);

    }

});
