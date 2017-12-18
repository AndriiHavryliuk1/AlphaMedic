var app = angular.module('alphaMedicApp');
app.controller('ListOfMedicationsController', function(URL_FOR_REST, $scope, $http,$timeout) {
    $http.get(URL_FOR_REST.url+"api/medications")
        .success(function(responce) {
            $scope.medications = responce;
            $scope.changedPrice=0;
            });


    $scope.addMedication=function()
    {
      $http.post(URL_FOR_REST.url+"api/medications",$scope.newMedications)
          .success(function(data) {
              $scope.headSucced = true;
              $scope.headFailed = false;
              $scope.medications.push(data);
              $timeout(function() {
                $scope.headSucced=false;
              }, 2000);
          })
          .error(function() {
              $scope.headSucced = false;
              $scope.headFailed = true;
              $timeout(function() {
                $scope.headFailed=false;
              }, 2000);
          });

    };

});
