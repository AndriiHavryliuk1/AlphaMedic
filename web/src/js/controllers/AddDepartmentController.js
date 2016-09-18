var app = angular.module('alphaMedicApp');
app.controller('AddDepartmentController', function(URL_FOR_REST, $scope, $http, $routeParams, fileUploadService, $timeout) {
    $scope.newDepartment = null;

    $scope.submit = function() {
        $http.post(URL_FOR_REST.url + "api/departments", $scope.newDepartment)
            .success(function(data) {
                if (typeof $scope.picture != "undefined") {
                    fileUploadService.uploadFileToUrl($scope.picture, URL_FOR_REST.url + "api/image/department/" + data).then(function(response) {});
                }
                $scope.succed = true;
                $scope.failed = false;
                $timeout(function() {
                    $scope.succed = false;
                }, 2000);
                $scope.newDepartment = null;
                $scope.picture = null;
                $scope.addDepartmentForm.$setPristine();
            })
            .error(function() {
                $scope.succed = false;
                $scope.failed = true;

                $timeout(function() {
                    $scope.failed = false;
                }, 5000);
            });
    }
});
