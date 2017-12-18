angular.module('alphaMedicApp').service('SendMailService', ['$http', function ($http) {
    this.SendMail = function(URL_FOR_REST,appoint, del, id){
      var emailPostDto = {'appointment': appoint, 'EmailState': del};
      $http.post(URL_FOR_REST.url + "api/appointments/" + id + "/sendmail" , emailPostDto  )
        .success(function(data){
        })
        .error(function(){
        });
    }
}]);
