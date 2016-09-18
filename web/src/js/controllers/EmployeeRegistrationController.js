function check(input) {
    if (input.value != document.getElementById('password').value) {
        input.setCustomValidity('Password Must be Matching.');
    } else {
        // input is valid -- reset the error message
        input.setCustomValidity('');
    }
};
var app = angular.module('alphaMedicApp');

app.controller('EmployeeRegistrationController', function(URL_FOR_REST, sha256, USER_ROLES, $scope, $http, $window, $rootScope,$compile) {

    $scope.failed = false;
    $scope.succed = false;
    $scope.Type = USER_ROLES.doctor;
    $scope.isTypeDoctor = true;
    $scope.departmentsList = null;
    $scope.DepartmentId;
    $scope.OnChangeType = function() {
        if ($scope.Type === USER_ROLES.doctor) {
            $scope.isTypeDoctor = true;
            $http.get(URL_FOR_REST.url + "api/Departments")
                .success(function(responce) {
                    $scope.departmentsList = responce;
                    $scope.DepartmentId =responce[0].DepartmentId;

                });
        } else
            $scope.isTypeDoctor = false;
    }
    $scope.OnChangeType();


    $scope.submit = function() {
        $scope.employee.ClaimType = "ClaimTypes.Role";
        $scope.employee.EmployeeType = $scope.Type;
        $scope.employee.DepartmentId = $scope.DepartmentId;
        $scope.employee.Active = true;
        $scope.employee.Password = sha256.convertToSHA256($scope.employee.Password);
        switch ($scope.Type) {
            case USER_ROLES.doctor:
                var url = URL_FOR_REST.url + "api/doctors";
                break;
            case USER_ROLES.receptionist:
                var url = URL_FOR_REST.url + "api/employees/receptionists";
                break;
            case USER_ROLES.admin:
                var url = URL_FOR_REST.url + "api/employees/administrators";
                break;
        }
        $http
            .post(url, $scope.employee)
            .success(function() {
                $scope.succed = true;
                $scope.failed = false;
                $scope.employee=null;
                $scope.regiserEmployeeForm.$setPristine();

            })
            .error(function() {
                $scope.succed = false;
                $scope.failed = true;
            });
    }
});
