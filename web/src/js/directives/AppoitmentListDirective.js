var app = angular.module('alphaMedicApp');
app.directive("appointmentList", function() {
    return {
        link: function(scope, element, attrs) {
            scope.$watch('patientAppointments', function() {
                scope.data = scope[attrs["appointmentList"]];
            }, true);
            scope.$watch('doctorAppointments', function() {
                if (scope[attrs["appointmentList"]] != null) {
                    scope.data = scope[attrs["appointmentList"]];
                }
            });
        },
        restrict: 'A',
        templateUrl: function(element, attrs) {
            if (attrs["appointmentList"] == 'patientAppointments') {
                return "src/views/templates/patientAppoitmetTemplate.html";
            } else
            if (attrs["appointmentList"] == "doctorAppointments") {
                return "src/views/templates/doctorAppoitmentTemplate.html";
            }
        }
    }
});
