var app = angular.module('alphaMedicApp');
app.directive("custompagination", function(USER_ROLES, jwtHelper, DOCTOR_TYPES) {
    return {
        restrict: 'A',
        templateUrl: 'src/views/templates/PaginationTemplate.html'

        }
    });
