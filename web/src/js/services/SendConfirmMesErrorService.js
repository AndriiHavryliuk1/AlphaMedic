angular.module('alphaMedicApp').service('SendConfirmMesErrorService', function ($http, URL_FOR_REST, $window) {
    this.SendMailError = function(UserId){
      $http.get(URL_FOR_REST.url + "api/Patients/forRegistrationConfirm/" + UserId, {
              headers: {
                  'Content-type': 'application/json'
              }
          })
          .success(function(data, status, headers, config) {
            alert("We have some trouble with our server please try again!");
            $window.location.href = "#/signUp";
            $http.delete(URL_FOR_REST.url + "api/Patients/" + UserId, data, {
                    headers: {
                        'Content-type': 'application/json'
                    }
                })
                .success(function(data, status, headers, config) {

                })
                .error(function(data, status, headers, config) {

                })
          })
          .error(function(data, status, headers, config) {



          })
    }
});
