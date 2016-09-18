var app = angular.module('alphaMedicApp');
app.controller('ConfirmRegistrationController', function(URL_FOR_REST, USER_ROLES, $rootScope, $scope, $window, $http, $routeParams, jwtHelper) {






    $scope.confirmed = $routeParams["id"] != undefined ? true : false;
    var state = true;
    if ($routeParams["id"] != undefined) {
        //  FilterService.setUserId('');
        $http.put(URL_FOR_REST.url + "api/Patients/changeState/" + $routeParams["id"], state, {
                headers: {
                    'Content-type': 'application/json'
                }
            })
            .success(function(data, status, headers, config) {
                $scope.confirmed = true;

            })
            .error(function(data, status, headers, config) {
                alert("We have some trouble with our server please try again!");
            })
    }





});
