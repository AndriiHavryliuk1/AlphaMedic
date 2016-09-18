var app = angular.module('alphaMedicApp');
app.directive("settings", function(USER_ROLES, jwtHelper, DOCTOR_TYPES) {
    return {
        restrict: 'A',
        templateUrl: function(elm, attr) {
            //  if (jwtHelper.decodeToken(localStorage.getItem('token')).role == USER_ROLES.admin)

            switch (attr["settings"]) {
                case "password":
                    return 'src/views/templates/AdminSettingsTemplate.html';
                    break;
                case "employee":
                    return 'src/views/templates/EmployeeSettingsTemplate.html';
                    break;
                case "doctor":
                    return 'src/views/templates/DoctorSettingsTemplate.html';
                    break;
                case "department":
                    return 'src/views/templates/DepartmentSettingsTemplate.html';
                    break;
                default:
                    return 'src/views/templates/SettingsTemplate.html';

            }


        }


    }
});
