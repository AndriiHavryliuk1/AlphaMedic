angular.module('alphaMedicApp').
service('ChangeUserInfoService', function(sha256, $http, URL_FOR_REST, $timeout, DOCTOR_TYPES) {

    var notifyManager = function(scope, successState, errorState, message, timeout) {
        scope.notifySuccess = successState;
        scope.notifyError = errorState;
        scope.message = message;

        if (successState == true) {
            $timeout(function() {
                scope.notifySuccess = false;
            }, timeout);
        }
        if (errorState == true) {
            $timeout(function() {
                scope.notifyError = false;
            }, timeout);
        }
    }

    this.ChangePassword = function(ChangePass, id, scope) {
        if (ChangePass.NewPass != null && ChangePass.OldPass != null) {
            ChangePass.NewPass = sha256.convertToSHA256(ChangePass.NewPass);
            ChangePass.OldPass = sha256.convertToSHA256(ChangePass.OldPass);
            $http.put(URL_FOR_REST.url + "api/users/changepass/" + id, ChangePass)
                .success(function(data, status, headers, config) {

                    notifyManager(scope, true, false, "Success", 4000);
                    ChangePass.NewPass = null;
                    ChangePass.OldPass = null;
                    //  alert("You have jast change your setting!");
                }).error(function(data, status) {

                    ChangePass.NewPass = null;
                    ChangePass.OldPass = null;

                    notifyManager(scope, false, true, data.Message, 4000);
                });
        }
    }


    this.ChangeUser = function(prewUser, user, id, scope) {

        $http.put(URL_FOR_REST.url + "api/users/" + id, user)
            .success(function(data, status, headers, config) {
                prewUser.Name = user.Name;
                prewUser.Surname = user.Surname;
                if (user.Gender == "0" || user.Gender == "1") {
                    prewUser.Gender = user.Gender == 0 ? "Male" : "Famale";
                }
                prewUser.DateOfBirth = user.DateOfBirth;
                prewUser.Phone = user.Phone;
                prewUser.Address = user.Address;

                notifyManager(scope, true, false, "Success", 4000);

            }).error(function(data, status) {
                notifyManager(scope, false, true, data.Message, 4000);

                scope.user = prewUser;
                scope.message = data.Message;
            });

    }
    this.ChangeEmployee = function(prevEmployye, employee, id, scope) {

        $http.put(URL_FOR_REST.url + "api/employees/" + id, employee)
            .success(function(data, status, headers, config) {

                notifyManager(scope, true, false, "Success", 4000);

            }).error(function(data, status) {
                notifyManager(scope, false, true, data.Message, 4000);
                employee.Name = prevEmployye.Name;
                employee.Surname = prevEmployye.Surname;
                employee.DateOfBirth = prevEmployye.DateOfBirth;
                employee.Phone = prevEmployye.Phone;
                employee.Address = prevEmployye.Address;
                employee.Email = prevEmployye.Email;
                employee.EmploymentRecordBookNumber = prevEmployye.EmploymentRecordBookNumber;
            });;
    }
    this.ChangeDoctor = function(prevdoctor, doctor, id, scope) {
        $http({
                url: URL_FOR_REST.url + "api/Doctors/" + doctor.UserId,
                headers: {
                    'Content-Type': 'application/json'
                },
                method: 'PATCH',
                data: [{
                        op: "replace",
                        path: "/Name",
                        value: doctor.Name
                    }, {
                        op: "replace",
                        path: "/Surname",
                        value: doctor.Surname
                    },

                    {
                        op: "replace",
                        path: "/Education",
                        value: doctor.Education
                    }, {
                        op: "replace",
                        path: "/DoctorType",
                        value: doctor.DoctorTypeInt
                    }, {
                        op: "replace",
                        path: "/UserClaimId",
                        value: doctor.DoctorTypeInt
                    }, {
                        op: "replace",
                        path: "/DepartmentId",
                        value: doctor.SelectedDepartment.DepartmentId
                    }
                ]
            }).success(function(data, status, headers, config) {
                notifyManager(scope, true, false, "Success", 4000);
                switch (doctor.DoctorTypeInt) {
                    case 2:
                        doctor.DoctorType = "Doctor";
                        break;
                    case 4:
                        doctor.DoctorType = "HospitalDean";
                        break;
                    case 5:
                        doctor.DoctorType = "HeadDepartment";
                        break;
                }
                doctor.DepartmentName=doctor.SelectedDepartment.Name;


            })
            .error(function(data, status, headers, config) {
                notifyManager(scope, false, true, data.Message, 4000);
                doctor.Name = prevdoctor.Name;
                doctor.Surname = prevdoctor.Surname;
                doctor.Education = prevdoctor.Education;
                doctor.DoctorTypeInt = prevdoctor.DoctorTypeInt;
                doctor.DepartmentId = prevdoctor.DepartmentId;
                doctor.DepartmentName=prevdoctor.DepartmentName;
                console.log(data);
                alert(error);

            });
    }

    this.ChangeDepartment = function(department) {
        $http({
                url: URL_FOR_REST.url + "api/departments/" + department.DepartmentId,
                headers: {
                    'Content-Type': 'application/json'
                },
                method: 'PATCH',
                data: [{
                    op: "replace",
                    path: "/Name",
                    value: department.Name
                }, {
                    op: "replace",
                    path: "/Description",
                    value: department.Description
                }]

            }).success(function(data, status, headers, config) {})
            .error(function(data, status, headers, config) {
                alert('something gone wrong');
                console.log(data);

            });
    }

});
