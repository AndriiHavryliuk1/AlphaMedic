var app = angular.module('alphaMedicApp');
app.controller('DepartmentInfoController', function(URL_FOR_REST, $scope, $http,$rootScope, $routeParams, FilterService,ChangeUserInfoService,fileUploadService) {
    $scope.goToListOfDoctor = function() {
        FilterService.setDepartment($routeParams["id"]);
      //  $scope.$parent.ListDoctors = "api/Departments/" + $routeParams["id"] + "/doctors";
    }
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
        LoadDepartmentsInfo();
    }

    $scope.isHidePatients = localStorage.getItem('token') == undefined ? true : false;

    $scope.HidePat = $rootScope.isPatient || $scope.isHidePatients || ($rootScope.isDoctor && !$rootScope.isHospitalDean && !$rootScope.isHeadDepartment);
    $scope.goToListOfPatients = function(){
      FilterService.setDepartment($routeParams["id"]);
    //  $location.path('/registrationForAppointment');
    }


    LoadDepartmentsInfo = function() {
        $http.get(URL_FOR_REST.url + "api/Departments/" + $routeParams["id"] + "?all=" + $scope.AllFeedbacks)
            .success(function(responce) {
                if (responce.FeedbacksCount > 3) $scope.showButtonAllFeedback = true;
                $scope.dep = responce;
              //  FilterService.setDepartment($routeParams["id"]);
            })
    }
    LoadDepartmentsInfo();

    $scope.ChangeDepartment = function() {
        //upload photo
        var id = $scope.dep.DepartmentId;
        if (typeof $scope.picture != "undefined") {
            fileUploadService.uploadFileToUrl($scope.picture, URL_FOR_REST.url + "api/image/department/" + id).then(function(response) {
                $scope.dep.URLImage = response.data;
            });
        }
        ChangeUserInfoService.ChangeDepartment($scope.dep, URL_FOR_REST, $http);
    }

});
