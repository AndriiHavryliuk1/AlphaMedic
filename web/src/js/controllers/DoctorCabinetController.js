var app = angular.module('alphaMedicApp');

app.controller('DoctorCabinetController', function(URL_FOR_REST, $scope, $http, $routeParams, jwtHelper, ChangeUserInfoService, fileUploadService) {

    $http.get(URL_FOR_REST.url + "api/Doctors/" + jwtHelper.decodeToken(localStorage.getItem('token')).id + "/Appointments")
        .success(function(responce) {
            $scope.doctorAppointments = responce;
            $scope.user = responce;
        })




    $scope.ChangePassword = function() {
        ChangeUserInfoService.ChangePassword($scope.ChangePass, jwtHelper.decodeToken(localStorage.getItem('token')).id);
    }


    $scope.ChangeUser = function() {
        //upload photo
        var id = jwtHelper.decodeToken(localStorage.getItem('token')).id;
        if (typeof $scope.picture != "undefined") {
            fileUploadService.uploadFileToUrl($scope.picture, URL_FOR_REST.url + "api/image/" + id).then(function(response) {
                $scope.user.URLImage = response.data;
            });
        }
        ChangeUserInfoService.ChangeUser($scope.user, $scope.user, jwtHelper.decodeToken(localStorage.getItem('token')).id);
    }
});
