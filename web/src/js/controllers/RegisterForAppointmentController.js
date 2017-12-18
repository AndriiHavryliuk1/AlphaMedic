var app = angular.module('alphaMedicApp');

app.controller('RegisterForAppointmentController', function(URL_FOR_REST, jwtHelper, $timeout, $filter, FilterService, $scope, $http, $window) {

    $scope.appointment = {
        DoctorId: FilterService.getDoctor(),
        PatientId: '',
        Description: '',
        Duration: '',
        Checked: false
    }
    $scope.chooseDep = FilterService.getDepartment();
    $scope.appointment.DoctorId = FilterService.getDoctor();
    $http.get(URL_FOR_REST.url + "api/departments")
        .success(function(responce) {
            $scope.departments = responce;
            FilterService.setDepartment('');
        });
    $scope.changeDepartment = function() {

        $http.get(URL_FOR_REST.url + "api/Departments/" + $scope.chooseDep + "/doctors")
            .success(function(responce) {
                $scope.doctors = responce;
                FilterService.setDepartment('');
                FilterService.setDoctor('');
            });
    };

    $scope.$watch('chooseDep', function() {
        if (FilterService.getDepartment() != '')
            $scope.changeDepartment();
    });




    $scope.submit = function() {
        $scope.succed = false;
        $scope.failed = false;
        $scope.dateFail = false;

        $scope.appointment.Date = $filter('date')($scope.appointment.Date, "yyyy-MM-dd HH:mm");
        if (localStorage.getItem('token') != undefined) {
            $scope.appointment.PatientId = jwtHelper.decodeToken(localStorage.getItem('token')).id;
            $http
                .post(URL_FOR_REST.url + "api/appointments/", $scope.appointment, {
                    headers: {
                        'Content-type': 'application/json'
                    }
                })
                .success(function(data, status, headers, config) {
                    $scope.succed = true;
                    $timeout(function () {
                      $window.location.href = "/#/patientCabinet";
                    }, 2000);
                })
                .error(function(data, status, headers, config) {
                    if (data.Message == "Wrong date")
                        $scope.dateFail = true;
                    else $scope.failed = true;
                });
        } else {
            alert("You can't register for appointment! Please, sign in or sign up");
            FilterService.setDepartment('');
            $window.location.href = "/#/signIn";
        }
    };

});
