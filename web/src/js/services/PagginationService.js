angular.module('alphaMedicApp').service('PagginationService', function ($rootScope,$filter,$location) {
      $rootScope.search = function(loadList) {
          $rootScope.pagingInfo.page = 1;
          loadList();
      };

      $rootScope.nextPage = function(loadList, pages) {
          if ($rootScope.pagingInfo.page < pages) {
              $rootScope.pagingInfo.page++;
              loadList();
              backToTop();
          }
      };

      $rootScope.firstPage = function(loadList) {
          if ($rootScope.pagingInfo.page > 1) {
              $rootScope.pagingInfo.page = 1;
              loadList();
              backToTop();
          }
      };

      $rootScope.lastPage = function(loadList, pages) {
          if ($rootScope.pagingInfo.page < pages) {
              $rootScope.pagingInfo.page = pages;
              loadList();
              backToTop();
          }
      };

      $rootScope.selectPage = function(loadList, prop) {
          if ($rootScope.pagingInfo.page == prop)
              return;
          $rootScope.pagingInfo.page = prop;
          loadList();
          backToTop();
      }

      $rootScope.previousPage = function(loadList) {
          if ($rootScope.pagingInfo.page > 1) {
              $rootScope.pagingInfo.page--;
              loadList();
              backToTop();
          }
      };

      backToTop = function() {
          $("html, body").animate({scrollTop: 0}, 50);
      }

      this.ChangeURL = function(loadList, doctors, preArray, url, pagingInfo){
            if (doctors == preArray)
                return;
            if ($location.path() != url) {
                $location.search('search', null);
                $location.search('department', null);
                $location.search('page', null);
                return;
            }
            $rootScope.pagingInfo = pagingInfo;
            $("html, body").animate({ scrollTop: 0 }, 50);
            loadList();
      };




          $rootScope.checkClear = '';
          $rootScope.Clear = function($event) {
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

          $rootScope.SearchByDate = function(loadList) {
              $rootScope.pagingInfo.page = 1;
              CheckDate();
              if ($rootScope.pagingInfo.periodFrom != '' && $rootScope.pagingInfo.periodTill != '' && $filter('date')($rootScope.pagingInfo.periodTill, "yyyy-MM-dd") < $rootScope.pagingInfo.periodFrom) {
                  alert("Input incorect date!");
                  return;
              }
              loadList();
          }

          CheckDate = function() {
              var dateFrom = angular.element('#dateFrom');
              var dateTill = angular.element('#dateTill');
              $rootScope.pagingInfo.periodFrom = dateFrom["0"].value == '' ? dateFrom["0"].value : $rootScope.pagingInfo.periodFrom;
              $rootScope.pagingInfo.periodTill = dateTill["0"].value == '' ? dateTill["0"].value : $rootScope.pagingInfo.periodTill;

          }
});
