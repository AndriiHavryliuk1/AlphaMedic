var app = angular.module('alphaMedicApp');

app.controller('ListOfPatientsController', function(URL_FOR_REST, $scope, $http, $location, $rootScope, PagginationService, $routeParams, PaginationArrayService, FilterService) {


    $rootScope.pagingInfo = {
        page: $location.search().page == undefined ? 1 : $location.search().page,
        itemsPerPage: 10,
        sortBy: 'UserId',
        reverse: false,
        search: $location.search().search == undefined ? '' : $location.search().search,
        totalItems: 0,
        doctor: FilterService.getDoctor() == '' ? $location.search().doctor : FilterService.getDoctor(),
        department: FilterService.getDepartment() == '' ? $location.search().department : FilterService.getDepartment()
    };

    $scope.showDoctorSelect = $rootScope.pagingInfo != undefined && $rootScope.pagingInfo.department != undefined;
    $scope.changeDepartment = function() {
        if ($rootScope.pagingInfo.department === "") {
            $scope.doctors = null;
            $scope.showDoctorSelect = false;
            $rootScope.pagingInfo.doctor = '';
            $scope.loadList();
        } else {
            $scope.showDoctorSelect = true;
            $http.get(URL_FOR_REST.url + "api/Departments/" + $rootScope.pagingInfo.department + "/doctors")
                .success(function(responce) {
                    $scope.doctors = responce;
                    $scope.loadList();
                });
        }
    };

    $scope.$on('$locationChangeStart', function(e) {
      $rootScope.pagingInfo = {
          page: $location.search().page == undefined ? 1 : $location.search().page,
          itemsPerPage: 10,
          sortBy: 'UserId',
          reverse: false,
          search: $location.search().search == undefined ? '' : $location.search().search,
          totalItems: 0,
          doctor: FilterService.getDoctor() == '' ? $location.search().doctor : FilterService.getDoctor(),
          department: FilterService.getDepartment() == '' ? $location.search().department : FilterService.getDepartment()
      };
        PagginationService.ChangeURL($scope.loadList, $scope.patients, $rootScope.preArray, '/patients', $rootScope.pagingInfo);
    });

    $scope.loadList = function() {
        $scope.patients = null;
        var url = "?page=" + $rootScope.pagingInfo.page +
            "&itemsPerPage=" + $rootScope.pagingInfo.itemsPerPage + "&sortBy=" + $rootScope.pagingInfo.sortBy +
            "&reverse=" + $rootScope.pagingInfo.reverse + "&search=" + $rootScope.pagingInfo.search +
            "&doctor=" + $rootScope.pagingInfo.doctor + "&department=" + $rootScope.pagingInfo.department;

        $http.get(URL_FOR_REST.url + "api/Patients", {
                params: $rootScope.pagingInfo
            })
            .success(function(responce) {
                $scope.patients = responce.data;
                $scope.AnyElementOfList = responce.count == 0;
                $rootScope.pagingInfo.totalItems = responce.count;
                $scope.pages = Math.ceil(responce.count / $rootScope.pagingInfo.itemsPerPage);
                FilterService.setDepartment('');
                FilterService.setDoctor('');
                $scope.paginArray = PaginationArrayService.Array($scope.pages, $rootScope.pagingInfo.page);
            });
            $rootScope.preArray = $scope.patients;
        if ($rootScope.pagingInfo.search == '') {
            $location.search('search', null)
            $location.search('page', $rootScope.pagingInfo.page);
            $location.search('doctor', $rootScope.pagingInfo.doctor);
            $location.search('department', $rootScope.pagingInfo.department);
        } else {
            $location.search('search', $rootScope.pagingInfo.search);
            $location.search('page', $rootScope.pagingInfo.page);
            $location.search('department', $rootScope.pagingInfo.department);
            $location.search('doctor', $rootScope.pagingInfo.doctor);
        }
        if ($rootScope.pagingInfo.department == '') {
            $location.search('department', null);
        }

        if ($rootScope.pagingInfo.doctor == '') {
            $location.search('doctor', null);
        }
    }

    if ($scope.showDoctorSelect === true) $scope.changeDepartment();

    $scope.$watch('pagingInfo.department', function() {
        if (FilterService.getDoctor() != '')
            $scope.changeDepartment();
    });

    $scope.loadList();

    $http.get(URL_FOR_REST.url + "api/Departments")
        .success(function(responce) {
            $scope.departments = responce;


        });
});
