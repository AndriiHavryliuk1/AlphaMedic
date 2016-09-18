angular.module('alphaMedicApp').
service('ChangeUserInfoService',  function(sha256, $http, URL_FOR_REST) {

  this.ChangePassword = function(ChangePass, id){
    ChangePass.NewPass = sha256.convertToSHA256(ChangePass.NewPass);
    ChangePass.OldPass = sha256.convertToSHA256(ChangePass.OldPass);
    $http.put(URL_FOR_REST.url+"api/users/changepass/"+ id, ChangePass)
        .success(function (data, status, headers, config) {
          alert("You have jast changed your password!");

        //  alert("You have jast change your setting!");
        })
  }


  this.ChangeUser = function(prewUser, user, id){

    $http.put(URL_FOR_REST.url+"api/users/"+id, user)
        .success(function (data, status, headers, config) {
          prewUser.Name = user.Name;
          prewUser.Surname = user.Surname;
          if(user.Gender=="0" || user.Gender=="1"){
          prewUser.Gender = user.Gender==0?"Male":"Famale" ;
          }
          prewUser.DateOfBirth = user.DateOfBirth;
          prewUser.Phone = user.Phone;
          prewUser.Address = user.Address;
        });
  }
  this.ChangeEmployee = function(employee, id){

    $http.put(URL_FOR_REST.url+"api/employees/"+id, employee)
        .success(function (data, status, headers, config) {

        }).error(function(data,status) {
              alert(data.Message);
        });;
  }
  this.ChangeDoctor = function(doctor, id){
    $http({
            url:URL_FOR_REST.url+"api/Doctors/" + doctor.UserId,
            headers: {
                'Content-Type': 'application/json'
            },
            method: 'PATCH',
            data: [{
                op: "replace",
                path: "/Name",
                value: doctor.Name
            },
            {
                op: "replace",
                path: "/Surname",
                value: doctor.Surname
            },

            {
                op: "replace",
                path: "/Education",
                value: doctor.Education
            } ,
            {
                op: "replace",
                path: "/DoctorType",
                value: doctor.DoctorTypeInt
            } ,
            {
              op: "replace",
              path: "/UserClaimId",
              value: doctor.DoctorTypeInt
            },
            {
                op: "replace",
                path: "/DepartmentId",
                value: doctor.DepartmentId
            }    ]
        }).success(function(data, status, headers, config) {
        })
        .error(function(data, status, headers, config) {
            alert('something gone wrong');
            console.log(data);

        });
  }

  this.ChangeDepartment = function(department)
  {
    $http({
            url:URL_FOR_REST.url+"api/departments/" + department.DepartmentId,
            headers: {
                'Content-Type': 'application/json'
            },
            method: 'PATCH',
            data: [{
                op: "replace",
                path: "/Name",
                value: department.Name
            },
            {
                op: "replace",
                path: "/Description",
                value: department.Description
            }]

        }).success(function(data, status, headers, config) {
        })
        .error(function(data, status, headers, config) {
            alert('something gone wrong');
            console.log(data);

        });
  }

});
