var app = angular.module('alphaMedicApp');
app.controller('DoctorsInfoController', function(URL_FOR_REST, $scope, $http,jwtHelper, $routeParams,FilterService) {
    $scope.AllFeedbacks = false;
    $scope.ButtonName = "Show all feedbacks";
    $scope.AllFeedbacksFunc = function() {
        if ($scope.AllFeedbacks) {
            $scope.AllFeedbacks = false;
            $scope.ButtonName = "Show all feedbacks";
        } else {
            $scope.AllFeedbacks = true;
            $scope.ButtonName = "Show last feedbacks";
        }
        LoadDoctorsInfo();

    }

    $scope.isHidePatients = localStorage.getItem('token') == undefined ? true : false;

    $scope.goToListOfPatients = function(){
      FilterService.setDoctor($routeParams["id"]);
      FilterService.setDepartment($scope.doctor.DepartmentId);
    //  $location.path('/registrationForAppointment');
    }

    LoadDoctorsInfo = function() {
        $http.get(URL_FOR_REST.url + "api/Doctors/" + $routeParams["id"] + "?all=" + $scope.AllFeedbacks)
            .success(function(responce) {
                $scope.doctor = responce;
                if (responce.FeedbacksCount > 3) $scope.showButtonAllFeedback = true;

            })
    }


    LoadDoctorsInfo();
});
