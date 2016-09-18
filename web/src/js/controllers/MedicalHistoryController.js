var app = angular.module('alphaMedicApp');
app.controller('MedicalHistoryController', function(URL_FOR_REST, $scope, $rootScope,PaginationArrayService, PagginationService, FilterService, $http, $location, $routeParams, $filter) {
  $scope.GoToAddNewProcedure = function()
  {
    $location.search('page', null);
    $location.path("/addProcedure/"+$routeParams["id"]);
  }
  $scope.ViewProcedure = function(proc)
  {
      $scope.procedure = proc;
      $scope.ShowDiagnosis = proc.Diagnosis != null;
      $scope.showResult = proc.Result != null;
      $scope.ShowMedications = proc.Medications != undefined && proc.Medications.length != 0;
  }

  $rootScope.pagingInfo = {
      page: $location.search().page == undefined ? 1 : $location.search().page,
      itemsPerPage: 7,
      totalItems: 0,
      procedure: $location.search().procedure == undefined ? '' : $location.search().procedure,
      periodFrom: $location.search().periodFrom == undefined ? '' : $location.search().periodFrom,
      periodTill: $location.search().periodTill == undefined ? '' : $location.search().periodTill
  };

    $scope.checkClear = '';
    $scope.Clear = function($event) {
        if ($event.keyCode == 8) {
            switch ($scope.checkClear) {
                case 'periodFrom':
                    angular.element('#dateFrom').val('');
                    break;
                case 'periodTill':
                    angular.element('#dateTill').val('');
                    break;
            }
            return true;
        }
        else if ($event.keyCode == undefined)return false;
        return false;
    }





    $http.get(URL_FOR_REST.url + "api/patients/" + $routeParams["id"])
        .success(function(responce) {
            $scope.Patient = responce;
        }).error(function() {
            $location.path('/main');
        });


    $scope.AddWarningLabel = function() {

        var wLabel = {
            "Description": $scope.NewWarningLabel,
            "medicalHistoryId": $routeParams["id"]
        };

        $http.post(URL_FOR_REST.url + "api/warninglabels", wLabel)
            .success(function(responce) {
                $scope.medicalHistory.WarningLabels.push(responce.Description);
            }).error(function() {
                $location.path('/main');
            });
    };

    $scope.check = function() {
        angular.element('#dateFrom').val($filter('date')($rootScope.pagingInfo.periodFrom, "yyyy-MM-dd"));
        angular.element('#dateTill').val($filter('date')($rootScope.pagingInfo.periodTill, "yyyy-MM-dd"));
    }



    $scope.$on('$locationChangeStart', function(e) {
      $rootScope.pagingInfo = {
          page: $location.search().page == undefined ? 1 : $location.search().page,
          itemsPerPage: 7,
          totalItems: 0,
          procedure: $location.search().procedure == undefined ? '' : $location.search().procedure,
          periodFrom: $location.search().periodFrom == undefined ? '' : $location.search().periodFrom,
          periodTill: $location.search().periodTill == undefined ? '' : $location.search().periodTill
      };
        PagginationService.ChangeURL($scope.loadList, $scope.medicalHistory.Procedures, $rootScope.preArray, '/medicalHistory/' + $routeParams["id"], $rootScope.pagingInfo);
    });


    $scope.loadList = function() {
        $http.get(URL_FOR_REST.url + "api/medicalhistory/" + $routeParams["id"], {
                params: $rootScope.pagingInfo
            })
            .success(function(responce) {
                $scope.medicalHistory = responce.data;
                $rootScope.pagingInfo.totalItems = responce.count;
                $scope.pages = Math.ceil(responce.count / $rootScope.pagingInfo.itemsPerPage);
                $scope.paginArray = PaginationArrayService.Array($scope.pages, $rootScope.pagingInfo.page);
            })

            $scope.medicalHistory = new Object();
            $rootScope.preArray = $scope.medicalHistory.Procedures;
            $location.search('page', $rootScope.pagingInfo.page);
            $location.search('procedure', $rootScope.pagingInfo.procedure);
            $location.search('periodFrom', $filter('date')($rootScope.pagingInfo.periodFrom, "yyyy-MM-dd"));
            $location.search('periodTill', $filter('date')($rootScope.pagingInfo.periodTill, "yyyy-MM-dd"));
            if ($rootScope.pagingInfo.procedure == '') { $location.search('procedure', null); }

            if ($rootScope.pagingInfo.periodFrom == '') { $location.search('periodFrom', null); }

            if ($rootScope.pagingInfo.periodTill == '') { $location.search('periodTill', null); }

    };

    $scope.loadList();

    $scope.Search = function() {
        $rootScope.pagingInfo.page = 1;
        CheckDate();
        if ($rootScope.pagingInfo.periodFrom != '' && $rootScope.pagingInfo.periodTill != '' && $rootScope.pagingInfo.periodTill < $rootScope.pagingInfo.periodFrom) {
            alert("Input incorect date!");
            return;
        }
        $scope.loadList();
    }



    CheckDate = function() {
        var dateFrom = angular.element('#dateFrom');
        var dateTill = angular.element('#dateTill');
        $rootScope.pagingInfo.periodFrom = dateFrom["0"].value == '' ? dateFrom["0"].value : $rootScope.pagingInfo.periodFrom;
        $rootScope.pagingInfo.periodTill = dateTill["0"].value == '' ? dateTill["0"].value : $rootScope.pagingInfo.periodTill;

    }

});
