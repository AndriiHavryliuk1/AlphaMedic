var app = angular.module('alphaMedicApp');
app.controller('PatientInfoController', function(URL_FOR_REST, $scope, $http, $routeParams) {
    $http.get(URL_FOR_REST.url+"api/patients/" + $routeParams["id"])
        .success(function(responce) {
            $scope.patient = responce;
        });
});
