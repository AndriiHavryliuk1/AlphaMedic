var app = angular.module('alphaMedicApp');

app.controller('ListOfEmployeesController', function(URL_FOR_REST,$scope, $http,ChangeUserInfoService,fileUploadService) {
    $http.get(URL_FOR_REST.url+"api/employees")
        .success(function(responce) {
            $scope.employees = responce;
        })

        $scope.ChangeUser = function ()
        {
            ChangeUserInfoService.ChangeEmployee($scope.user, $scope.user.UserId);
            if ($scope.picture != "undefined") {
                    fileUploadService.uploadFileToUrl($scope.picture, URL_FOR_REST.url + "api/image/" + $scope.user.UserId).then(function(response) {
                    $scope.user.URLImage = response.data;
                });
            }
        };

        $scope.GetData=function(employee)
        {
          $scope.user=employee;
        };

});
