var app = angular.module('alphaMedicApp');

app.controller('StatisticDepartmentDataController', function($scope, $http, $routeParams,URL_FOR_REST) {
    $http.get(URL_FOR_REST.url+"api/departments").then(function(responce) {
        $scope.statDepartments = responce.data;
        return $http.get("jsons/docForDeps.json");
    }).then(function(responce) {

        if ($scope.statDepartments != null) {
            var keys = Object.keys($scope.statDepartments);
            for (var i = 0; i < keys.length; i++) {
                if ($scope.statDepartments[i].DepartmentId == $routeParams["id"]) {
                    $scope.selectedOption = $scope.statDepartments[i].Name;

                    break;

                }
            }
        } else {
            $scope.selectedOption = "All";
        }
        var statDoctorsar = [];
        if ($routeParams["id"] == 0) {
            $scope.statDoctors = responce.data;
            $scope.selectedOption = "All";
        } else {
            for (var i = 0; i < responce.data.length; i++) {

                if (responce.data[i].DepartmentId == $routeParams["id"]) {
                    statDoctorsar.push(responce.data[i]);
                }
            }
            $scope.statDoctors = statDoctorsar;
        }
        $scope.$broadcast('changeSelectedOption', $scope);
    });
});
