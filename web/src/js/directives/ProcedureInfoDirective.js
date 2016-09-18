var app = angular.module('alphaMedicApp');
app.directive("procedureInfo", function(jwtHelper, $http, USER_ROLES) {
    return {
        restrict: 'A',
        templateUrl: 'src/views/templates/ProcedureInfoTemplate.html'
    };
});
