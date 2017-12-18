var app = angular.module('alphaMedicApp');

app.controller('AppointmentInfoReceptionistController', function(URL_FOR_REST, USER_ROLES, $timeout, $filter, $scope, $route, $window, $http, $routeParams, jwtHelper, SendMailService) {
    $scope.showProcedure = false;
    $scope.Type = "Examination";
    $scope.FailDate = false;
    $scope.GlbDoctorId = null;
    $http.get(URL_FOR_REST.url + "api/Departments")
        .success(function(responce) {
            $scope.departments = responce;
        });
    $http.get(URL_FOR_REST.url + "api/appointments/" + $routeParams["id"])
        .success(function(responce) {

            $scope.appointment = responce;
            $scope.stateString = $scope.appointment.State;
            $scope.appointment.Date = $filter('date')($scope.appointment.Date, "yyyy-MM-dd HH:mm");
            if (!$scope.appointment.Checked) {
                $scope.appointment.Checked = true;


                $http.put(URL_FOR_REST.url + "api/appointments/" + $routeParams["id"], $scope.appointment)
                    .success(function(responce) {})
                    .error(function(responce) {
                        $scope.failed = true;
                    });
            }

            $scope.isDoctorSetted = responce.DoctorId != null;
            if ($scope.isDoctorSetted) {
                $scope.GlbDoctorId = responce.DoctorId;
            }
            $scope.isEdit = responce.State != "Accepted";
            $scope.showProcedure = responce.ProcedureType != null;
            if ($scope.showProcedure)
                $http.get(URL_FOR_REST.url + "api/procedures/" + $routeParams["id"])
                .success(function(responce) {
                    $scope.procedure = responce;
                    $scope.procedure.DoctorFullName = $scope.appointment.DoctorFullName;
                    $scope.procedure.DoctorId = $scope.appointment.DoctorId;

                });
            $scope.getDuration();
        });

    $scope.changeDepartment = function() {
        $http.get(URL_FOR_REST.url + "api/Departments/" + $scope.chooseDep + "/doctors")
            .success(function(responce) {
                $scope.doctors = responce;
            });
    }

    $scope.getDuration = function() {
            if ($scope.appointment.DoctorId != null && $scope.appointment.DoctorId != undefined) {
                $scope.GlbDoctorId = $scope.appointment.DoctorId;
            }
            if ($scope.GlbDoctorId != null) {
                $http.get(URL_FOR_REST.url + "api/Doctors/" + $scope.GlbDoctorId + "/durations").
                success(function(responce) {
                    $scope.durations = responce;
                })
            }

        }
        //Buttons
    $scope.CreateProcedure = function() {
        if ($scope.appointment.Date == null) {
            alert("You need to fill Date field, before you want to set prcedure.");
            return;
        }
        $scope.Type = $scope.procedure.Type;
        $scope.procedure.MedicalHistoryId = $scope.appointment.PatientId;
        $scope.procedure.ProcedureId = $routeParams["id"];
        $scope.procedure.Date = $scope.appointment.Date;
        $scope.procedure.DoctorId = $scope.appointment.DoctorId;
        switch ($scope.procedure.Type) {
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

        $scope.procedure.Date = $filter('date')($scope.procedure.Date, "yyyy-MM-dd HH:mm");
        $http
            .post($scope.url, $scope.procedure, {
                headers: {
                    'Content-type': 'application/json'
                }
            })
            .success(function(data, status, headers, config) {
                $timeout(function() {
                    $scope.showProcedure = true;
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
            $scope.appointment.Date = $filter('date')($scope.appointment.Date, "yyyy-MM-dd HH:mm");
            $http.post(URL_FOR_REST.url + "api/appointments/checkdateforapp", $scope.appointment)
                .success(function(responce) {
                    //  alert("Success! Date");
                    $scope.FailDate = false;
                    $http.put(URL_FOR_REST.url + "api/appointments/" + $routeParams["id"], $scope.appointment)
                        .success(function(responce) {
                            alert("Success! Appointment confirmed.\nWe sent notification on your email!");
                            $route.reload();
                            SendMailService.SendMail(URL_FOR_REST, $scope.appointment, 0, $routeParams["id"]);

                        })
                        .error(function(responce) {
                            $scope.failed = true;
                        });

                })
                .error(function(responce) {
                    //  alert("Error! Change date!");
                    $scope.FailDate = true;

                });


        } else {
            alert("Procedure hasn't been set");
        }
    }
    $scope.Decline = function() {
        $http.delete(URL_FOR_REST.url + "api/appointments/" + $routeParams["id"])
            .success(function(responce) {
                alert("Success! Appointment declined!");
                SendMailService.SendMail(URL_FOR_REST, $scope.appointment, 1, $routeParams["id"]);
                $window.location.href = "#/receptionistCabinet";
            })
            .error(function(responce) {
                $scope.failed = true;
            });
    };



});
