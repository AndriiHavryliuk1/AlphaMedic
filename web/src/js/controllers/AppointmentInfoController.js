var app = angular.module('alphaMedicApp');
app.controller('AppointmentInfoController', function($rootScope, URL_FOR_REST, USER_ROLES, $scope, $window, $http, $routeParams, jwtHelper) {
    var x = jwtHelper.decodeToken(localStorage.getItem("token"));

    $scope.hideSetResultButton = true;
    $scope.showProcedure = false;
    $scope.Decline = function() {
        $http.delete(URL_FOR_REST.url + "api/appointments/" + $routeParams["id"])
            .success(function(responce) {
                alert("Success! Appointment declined!");
                $window.location.href = "#/patientCabinet";
                SendMailService.SendMail(URL_FOR_REST, $scope.appointment, true, $routeParams["id"]);
            })
            .error(function(responce) {
                $scope.failed = true;
            });
    };

    $scope.DefineDiagnosis = function() {
        $http.post(URL_FOR_REST.url + "api/examinations/" + $scope.procedure.ProcedureId, $scope.Diagnosis, {
                headers: {
                    'Content-type': 'application/json'
                }
            })
            .success(function() {
                alert("Success! Diagnosis was added");
                $scope.procedure.Diagnosis = $scope.Diagnosis;
                $scope.hideDiagnosisButton = true;
                $scope.showDiagnosis = true;
                $scope.showFinishButton = true;
            })
            .error(function() {
                alert("Error! Can't add diagnosis");
                $scope.hideDiagnosisButton = false;
            });
    };

    $scope.LoadMedications = function() {
        $http.get(URL_FOR_REST.url + "api/medications")
            .success(function(responce) {
                $scope.meds = responce;
            });
    }

    $scope.AddMedications = function() {
        $scope.procedure.Medications = $scope.medications;
        $scope.procedure.MedicalHistoryId = $scope.appointment.PatientId;
        $http.put(URL_FOR_REST.url + "api/treatments/" + $scope.procedure.ProcedureId, $scope.procedure, {
                headers: {
                    'Content-type': 'application/json'
                }
            })
            .success(function() {
                alert("Success! Medications were added");
                $scope.hideAddMedicationButton = true;
            })
            .error(function() {
                alert("Error! Can't add medications");
                $scope.hideAddMedicationButton = false;
            });
    };

    $scope.FinishAppointment = function() {
        $scope.appointment.State = 2;
        $scope.stateString = "Finished";
        $http.put(URL_FOR_REST.url + "api/appointments/" + $routeParams["id"], $scope.appointment)
            .success(function() {

            })
            .error(function() {

            });
    }

    $scope.Result = function() {
        $http.put(URL_FOR_REST.url + "api/treatments/" + $scope.procedure.ProcedureId, $scope.procedure, {
                headers: {
                    'Content-type': 'application/json'
                }
            })
            .success(function() {
                alert("Success! Result was setted");
                $scope.hideSetResultButton = true;
                $scope.showFinishButton = true;
            })
            .error(function() {
                alert("Error! Can't set result");
                $scope.hideSetResultButton = false;
            });
    };

    $http.get(URL_FOR_REST.url + "api/appointments/" + $routeParams["id"])
        .success(function(responce) {
            $scope.appointment = responce;
            $scope.stateString = $scope.appointment.State;
            $scope.isDoctorAssigned = responce.DoctorFullName != null;
            var now = new Date;
            $scope.CanDeleteAppointment = x.role == USER_ROLES.patient && Date.parse(responce.Date) > now.setDate(now.getDate() + 1);
            $scope.showProcedure = responce.ProcedureType != null;
            if ($scope.showProcedure)
                $http.get(URL_FOR_REST.url + "api/procedures/" + $routeParams["id"])
                .success(function(responce) {
                    $scope.procedure = responce;
                    var userTimezoneOffset = new Date().getTimezoneOffset() * 60000;
                    $scope.IsProcedureAvailable = (Date.parse(new Date(Date.parse($scope.procedure.Date) + userTimezoneOffset)) - Date.parse(new Date()) < 0);
                    $scope.hideDiagnosisButton = !($rootScope.isDoctor == true && responce.Diagnosis == null && responce.Type == "Examination" && $scope.IsProcedureAvailable);
                    $scope.hideAddMedicationButton = !($rootScope.isDoctor == true && responce.Type == "Treatment" && responce.Medications.length == 0 && $scope.IsProcedureAvailable);
                    $scope.hideSetResultButton = !($rootScope.isDoctor == true && responce.Type == "Treatment" && responce.Result == null && $scope.IsProcedureAvailable);
                    $scope.showDiagnosis = responce.Diagnosis != null;
                    $scope.showFinishButton = (responce.Diagnosis != null || responce.Result != null) && $scope.appointment.State != "Finished";
                    $scope.showResult = responce.Result != null;
                });
            else {
                $scope.hideDiagnosisButton = true;
                $scope.hideAddMedicationButton = true;
            }
        });
});
