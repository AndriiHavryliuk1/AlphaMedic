var app = angular.module('alphaMedicApp');

app.controller('AppointmentInfoReceptionistController', function(URL_FOR_REST, USER_ROLES, $timeout, $scope, $route, $window, $http, $routeParams, jwtHelper, SendMailService) {
    $scope.showProcedure = false;
    $scope.Type = "Examination";

    $http.get(URL_FOR_REST.url + "api/Departments")
        .success(function(responce) {
            $scope.departments = responce;
        });
    $http.get(URL_FOR_REST.url + "api/appointments/" + $routeParams["id"])
        .success(function(responce) {
            $scope.appointment = responce;

            $scope.isDoctorSetted = responce.DoctorId != null;
            $scope.isEdit = responce.State != "Accepted";
            $scope.showProcedure = responce.ProcedureType != null;
            if ($scope.showProcedure)
                $http.get(URL_FOR_REST.url + "api/procedures/" + $routeParams["id"])
                .success(function(responce) {
                    $scope.procedure = responce;
                    $scope.procedure.DoctorFullName = $scope.appointment.DoctorFullName;
                    $scope.procedure.DoctorId = $scope.appointment.DoctorId;
                });
        });

    $scope.changeDepartment = function() {
            $http.get(URL_FOR_REST.url + "api/Departments/" + $scope.chooseDep + "/doctors")
                .success(function(responce) {
                    $scope.doctors = responce;
                });
        }
        //Buttons
    $scope.CreateProcedure = function() {
        if ($scope.appointment.DoctorId == null || $scope.appointment.Date == null) {
            alert("You need to fill Doctor and Date fields, before you want to set prcedure.");
            return;
        }
        $scope.procedure.MedicalHistoryId = $scope.appointment.PatientId;
        $scope.procedure.ProcedureId = $routeParams["id"];
        $scope.procedure.Date = $scope.appointment.Date;
        $scope.procedure.DoctorId = $scope.appointment.DoctorId;
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
                $timeout(function() {
                    $route.reload();
                }, 1000);
            })
            .error(function(data, status, headers, config) {
                if (status == 409) {
                    alert("Error! Procedure has already existed!");
                    $route.reload();
                    return;
                }
                alert("Error! Procedure hasn't been added. Please try again later.");
            });
    };
    $scope.ChangeDoctor = function() {
        $scope.isDoctorSetted = false;
        $scope.appointment.DoctorId = null;
    }
    $scope.Reset = function() {
        $route.reload();
    }
    $scope.EnableEdit = function() {
        $scope.isEdit = true;
    }
    $scope.Confirm = function() {
        if ($scope.showProcedure) {
            $scope.appointment.State = "Accepted";
            $http.put(URL_FOR_REST.url + "api/appointments/" + $routeParams["id"], $scope.appointment)
                .success(function(responce) {
                    alert("Success! Appointment confirmed.");
                    $route.reload();
                    SendMailService.SendMail(URL_FOR_REST, $scope.appointment, false, $routeParams["id"]);
                })
                .error(function(responce) {
                    $scope.failed = true;
                });
        } else {
            alert("Procedure hasn't been set");
        }
    }
    $scope.Decline = function() {
        $http.delete(URL_FOR_REST.url + "api/appointments/" + $routeParams["id"])
            .success(function(responce) {
                alert("Success! Appointment declined!");
                $window.location.href = "#/receptionistCabinet";
                SendMailService.SendMail(URL_FOR_REST, $scope.appointment, true, $routeParams["id"]);
            })
            .error(function(responce) {
                $scope.failed = true;
            });
    };

});
