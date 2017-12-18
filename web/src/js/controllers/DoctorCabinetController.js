var app = angular.module('alphaMedicApp');

app.controller('DoctorCabinetController', function($rootScope,URL_FOR_REST, $scope, $http, $routeParams, jwtHelper, ChangeUserInfoService, fileUploadService) {
  $scope.ChangePass = {
      OldPass: null,
      NewPass: null
  };

    $http.get(URL_FOR_REST.url + "api/Doctors/" + jwtHelper.decodeToken(localStorage.getItem('token')).id + "/Appointments")
        .success(function(responce) {
            $scope.doctorAppointments = responce;
            $scope.user = responce;
            $scope.bufferUser =  angular.copy(responce);
            $rootScope.ScheduleId=responce.ScheduleId;
            $scope.AnyAppointments = $scope.doctorAppointments == null || $scope.doctorAppointments.length == 0;
        })


    $scope.ChangePassword = function() {
        ChangeUserInfoService.ChangePassword($scope.ChangePass, jwtHelper.decodeToken(localStorage.getItem('token')).id,$scope);
    }


    $scope.ChangeUser = function() {
        //upload photo
        var id = jwtHelper.decodeToken(localStorage.getItem('token')).id;
        if (typeof $scope.picture != "undefined") {
            fileUploadService.uploadFileToUrl($scope.picture, URL_FOR_REST.url + "api/image/" + id).then(function(response) {
                $scope.user.URLImage = response.data;
            });
        }
      $scope.notifyError=false;
      $scope.notifySuccess=false;

      ChangeUserInfoService.ChangeUser($scope.user,$scope.bufferUser , jwtHelper.decodeToken(localStorage.getItem('token')).id,$scope);


    /*  $scope.notify=result.state;
      $scope.message=result.message*/
    }
});
