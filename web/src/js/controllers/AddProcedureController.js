var app = angular.module('alphaMedicApp');
app.controller('AddProcedureController', function(URL_FOR_REST, $scope, $http, $routeParams) {
    $scope.Type = "Examination";
    $http.get(URL_FOR_REST.url + "api/departments")
        .success(function(responce) {
            $scope.departments = responce;
        });
    $scope.changeDepartment = function() {

        $http.get(URL_FOR_REST.url + "api/Departments/" + $scope.chooseDep + "/doctors")
            .success(function(responce) {
                $scope.doctors = responce;
            });
    };

    $scope.submit = function() {
        $scope.succed = false;
        $scope.failed = false;
        $scope.procedure.MedicalHistoryId = $routeParams["id"];

        var appointment = {
            "Name": $scope.procedure.Name,
            "Description": $scope.procedure.Description,
            "DoctorId": $scope.procedure.DoctorId,
            "PatientId": $scope.procedure.MedicalHistoryId,
            "Date": $scope.procedure.Date
        }
        $http.post(URL_FOR_REST.url + "api/appointments", appointment, {
                headers: {
                    'Content-type': 'application/json'
                }
            })
            .success(function(data, status, headers, config) {
                $scope.procedure.ProcedureId = data.Id;
                switch ($scope.Type) {
                    case "Vaccination":
                        $scope.url = URL_FOR_REST.url + "api/vaccinations/";
                        break;
                    case "Treatment":
                        $scope.url = URL_FOR_REST.url + "api/treatments/";
                        break;

                    case "Examination":
                        $scope.url = URL_FOR_REST.url + "api/examinations/";
                        break;
                }
                $http
                    .post($scope.url, $scope.procedure, {
                        headers: {
                            'Content-type': 'application/json'
                        }
                    })
                    .success(function(data, status, headers, config) {
                        $scope.succed = true;
                    })
                    .error(function(data, status, headers, config) {
                        $scope.failed = true;
                    });
            })
            .error(function(data, status, headers, config) {
                $scope.failed = true;
            });
    };

});
