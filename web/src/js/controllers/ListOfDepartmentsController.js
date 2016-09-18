var app = angular.module('alphaMedicApp');

app.controller('ListOfDepartmentsController', function(URL_FOR_REST, $scope, $http) {
    $http.get(URL_FOR_REST.url+"api/Departments")
        .success(function(responce) {
            $scope.deps = responce;
        })
});
