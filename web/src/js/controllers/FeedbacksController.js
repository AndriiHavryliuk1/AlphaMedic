var app = angular.module('alphaMedicApp');

app.controller('FeedbacksController', function($window, $routeParams, $http, $scope, URL_FOR_REST, USER_ROLES, jwtHelper) {
    $scope.LeaveFeedback = function() {
        //  var base64Url = token.split('.')[1];
        var x = localStorage.getItem('token') == undefined ? null : jwtHelper.decodeToken(localStorage.getItem('token'));

        var feedback = {
            PatientId: (x == null || x.role != USER_ROLES.patient) ? null : x.id,
            Description: $scope.Feedback.Description,
            Date: new Date(),
            DoctorId: ($scope.doctor == undefined || $scope.doctor.UserId == undefined) ? null : $scope.doctor.UserId,
            DepartmentId: ($scope.dep == undefined || $scope.dep.DepartmentId == undefined) ? null : $scope.dep.DepartmentId
        }

        $http.post(URL_FOR_REST.url + "api/Feedbacks", feedback, {
                headers: {
                    'Content-type': 'application/json'
                }
            })
            .success(function(data, status, headers, config) {

                if (data.PatientId != null) {

                    $http.get(URL_FOR_REST.url + "api/Patients/" + data.PatientId)
                        .success(function(responce) {
                            data.Patient = responce;
                            data.PatientFullName = data.Patient.Name + " " + data.Patient.Surname;
                            data.PatientURLImage = data.Patient.URLImage;
                            if ($scope.$parent.dep != undefined) {
                                $scope.$parent.dep.Feedbacks.push(data);
                            } else $scope.$parent.doctor.Feedbacks.push(data);

                        });
                } else {

                    data.Patient = new Object();
                    data.PatientFullName = "Anonymous";
                    data.PatientURLImage = URL_FOR_REST.url+ "img/patients/profileAvatar.jpg";
                    if ($scope.$parent.dep != undefined) {
                        $scope.$parent.dep.Feedbacks.push(data);
                    } else $scope.$parent.doctor.Feedbacks.push(data);
                }
                alert("You`ve just left new feedback!");
            })
    }
});
