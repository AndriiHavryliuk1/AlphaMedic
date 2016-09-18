var app = angular.module('alphaMedicApp');

app.controller('ListOfDoctorsPatientsController', function(URL_FOR_REST, $scope, $http, $routeParams) {
    $http.get(URL_FOR_REST.url + "api/Patients/" + $routeParams["id"] + "/Patients")
        .success(function(responce) {
            $scope.patients = responce.Patients;
        });
    $scope.showDoctorSelect = false;
});
