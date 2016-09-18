var app = angular.module('alphaMedicApp');
app.controller('ListOfMedicationsController', function(URL_FOR_REST, $scope, $http) {
    $http.get(URL_FOR_REST.url+"api/medications")
        .success(function(responce) {
            $scope.medications = responce;
            $scope.changedPrice=0;
            });

  
    $scope.addMedication=function()
    {
      $http.post(URL_FOR_REST.url+"api/medications",$scope.newMedications)
          .success(function(data) {
              $scope.succed = true;
              $scope.failed = false;
              $scope.medications.push(data);
          })
          .error(function() {
              $scope.succed = false;
              $scope.failed = true;
          });

    };

});
