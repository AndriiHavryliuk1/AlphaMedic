var app = angular.module('alphaMedicApp');

app.controller('ReceptionistCabinetController', function(URL_FOR_REST, $window, $rootScope, $filter, PagginationService, PaginationArrayService, $scope, $http, $location, $routeParams, jwtHelper, ChangeUserInfoService, fileUploadService) {
  $scope.ChangePass = {
      OldPass: null,
      NewPass: null
  };
    $rootScope.pagingInfo = {
        page: $location.search().page == undefined ? 1 : $location.search().page,
        itemsPerPage: 7,
        totalItems: 0,
        state: $location.search().state == undefined ? '' : $location.search().state,
        periodFrom: $location.search().periodFrom == undefined ? '' : $location.search().periodFrom,
        periodTill: $location.search().periodTill == undefined ? '' : $location.search().periodTill,
        doctor: $location.search().doctor == undefined ? '' : $location.search().doctor,
        department: $location.search().department == undefined ? '' : $location.search().department
    };
    $scope.$watch('pagingInfo.department', function() {

        $scope.changeDepartment();
        $scope.loadList();
    });


    $http.get(URL_FOR_REST.url + "api/users/" + jwtHelper.decodeToken(localStorage.getItem('token')).id)
        .success(function(responce) {
            $scope.recept = responce;
            $scope.user = {
                UserId: $scope.recept.UserId,
                Name: $scope.recept.Name,
                Surname: $scope.recept.Surname,
                Gender: $scope.recept.Gender,
                DateOfBirth: $scope.recept.DateOfBirth,
                Phone: $scope.recept.Phone,
                Address: $scope.recept.Address
            }
            $scope.bufferUser=angular.copy($scope.recept);
        })

    $http.get(URL_FOR_REST.url + "api/Departments")
        .success(function(responce) {
            $scope.departments = responce;
        });

        $scope.$on('$locationChangeStart', function(e) {
          $rootScope.pagingInfo = {
              page: $location.search().page == undefined ? 1 : $location.search().page,
              itemsPerPage: 7,
              totalItems: 0,
              state: $location.search().state == undefined ? '' : $location.search().state,
              periodFrom: $location.search().periodFrom == undefined ? '' : $location.search().periodFrom,
              periodTill: $location.search().periodTill == undefined ? '' : $location.search().periodTill,
              doctor: $location.search().doctor == undefined ? '' : $location.search().doctor,
              department: $location.search().department == undefined ? '' : $location.search().department
          };
            PagginationService.ChangeURL($scope.loadList, $scope.appointments, $rootScope.preArray, '/receptionistCabinet', $rootScope.pagingInfo);
        });


    $scope.loadList = function() {
        $http.get(URL_FOR_REST.url + "api/appointments", {
                params: $rootScope.pagingInfo
            })
            .success(function(responce) {
                $scope.appointments = responce.data;
                $rootScope.pagingInfo.totalItems = responce.count;
                $scope.pages = Math.ceil(responce.count / $rootScope.pagingInfo.itemsPerPage);
                $scope.paginArray = PaginationArrayService.Array($scope.pages, $rootScope.pagingInfo.page);
            });
        $rootScope.preArray = $scope.appointments;
        $location.search('page', $rootScope.pagingInfo.page);
        $location.search('department', $rootScope.pagingInfo.department);
        $location.search('doctor', $rootScope.pagingInfo.doctor);
        $location.search('state', $rootScope.pagingInfo.state);
        $location.search('periodFrom', $filter('date')($rootScope.pagingInfo.periodFrom, "yyyy-MM-dd"));
        $location.search('periodTill', $filter('date')($rootScope.pagingInfo.periodTill, "yyyy-MM-dd"));

        if ($rootScope.pagingInfo.department == '') {
            $location.search('department', null);
        }
        if ($rootScope.pagingInfo.periodFrom == '') {
            $location.search('periodFrom', null);
        }
        if ($rootScope.pagingInfo.periodTill == '') {
            $location.search('periodTill', null);
        }
        if ($rootScope.pagingInfo.doctor == '') {
            $location.search('doctor', null);
        }
        if ($rootScope.pagingInfo.state == '') {
            $location.search('state', null);
        }

    }

    $scope.loadList();


    $scope.showDoctorSelect = false;
    $scope.changeDepartment = function() {
        if ($rootScope.pagingInfo.department != '') {
            $scope.showDoctorSelect = true;
            $http.get(URL_FOR_REST.url + "api/Departments/" + $rootScope.pagingInfo.department + "/doctors")
                .success(function(responce) {
                    $scope.doctors = responce;
                });
        } else {
            $scope.showDoctorSelect = false;
            $rootScope.pagingInfo.doctor = '';
        }
    };

    $scope.ChangePassword = function() {
        ChangeUserInfoService.ChangePassword($scope.ChangePass, jwtHelper.decodeToken(localStorage.getItem('token')).id,$scope);
    }

    $scope.checkClear = '';

    $scope.check = function() {
        angular.element('#dateFrom').val($filter('date')($rootScope.pagingInfo.periodFrom, "yyyy-MM-dd"));
        angular.element('#dateTill').val($filter('date')($rootScope.pagingInfo.periodTill, "yyyy-MM-dd"));
    }

    $scope.ChangeUser = function() {
        //upload photo
        if (typeof $scope.picture != "undefined") {
            fileUploadService.uploadFileToUrl($scope.picture, URL_FOR_REST.url + "api/image/" +
            jwtHelper.decodeToken(localStorage.getItem('token')).id).then(function(response) {
                $scope.user.URLImage = response.data;
            });
        }
        ChangeUserInfoService.ChangeUser($scope.recept, $scope.bufferUser, jwtHelper.decodeToken(localStorage.getItem('token')).id,$scope);
    }
});
