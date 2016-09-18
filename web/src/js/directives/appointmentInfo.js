var app = angular.module('alphaMedicApp');
app.directive("appointmentInfo", function(jwtHelper, $http, USER_ROLES) {
    return {
        restrict: 'A',
        templateUrl: function(scope, element, attrs) {
            var x = jwtHelper.decodeToken(localStorage.getItem('token'));
            if (x.role == USER_ROLES.receptionist)
                return 'src/views/templates/AppointmentInfoReceptionist.html';
            else return 'src/views/templates/AppointmentInfoOthers.html';
        }
    };
});
