var app = angular.module('alphaMedicApp');

app.controller('ListOfDoctorsController', function(URL_FOR_REST, $scope, $http, FilterService, PagginationService, $rootScope, $location, PaginationArrayService, $location, $routeParams, FilterService) {

    $rootScope.pagingInfo = {
        page: $location.search().page == undefined ? 1 : $location.search().page,
        itemsPerPage: 10,
        sortBy: 'DoctorId',
        reverse: false,
        search: $location.search().search == undefined ? '' : $location.search().search,
        totalItems: 0,
        department: FilterService.getDepartment() == '' ? $location.search().department : FilterService.getDepartment()
    };

    $scope.goToAppointment = function(depId, docId) {
        FilterService.setDepartment(depId);
        FilterService.setDoctor(docId);
        $location.search('search', null);
        $location.search('department', null);
        $location.search('page', null);
        $location.path('/registrationForAppointment');

    }



$scope.$on('$locationChangeStart', function(e) {
    $rootScope.pagingInfo = {
        page: $location.search().page == undefined ? 1 : $location.search().page,
        itemsPerPage: 10,
        sortBy: 'DoctorId',
        reverse: false,
        search: $location.search().search == undefined ? '' : $location.search().search,
        totalItems: 0,
        department: FilterService.getDepartment() == '' ? $location.search().department : FilterService.getDepartment()
    };
    PagginationService.ChangeURL($scope.loadList, $scope.doctors, $rootScope.preArray, '/doctors', $rootScope.pagingInfo);
});


    $scope.loadList = function() {
        $http.get(URL_FOR_REST.url + "api/Doctors", {
                params: $rootScope.pagingInfo
            })
            .success(function(responce) {
                $scope.doctors = responce.data;
                $scope.AnyElementOfList = responce.count == 0;
                $rootScope.pagingInfo.totalItems = responce.count;
                $scope.pages = Math.ceil(responce.count / $rootScope.pagingInfo.itemsPerPage);
                FilterService.setDepartment('');
                $scope.paginArray = PaginationArrayService.Array($scope.pages, $rootScope.pagingInfo.page);

            });

        $rootScope.preArray = $scope.doctors;
        if ($rootScope.pagingInfo.search == '') {
            $location.search('search', null);
            $location.search('page', $rootScope.pagingInfo.page);
            $location.search('department', $rootScope.pagingInfo.department);
        } else {
            $location.search('search', $rootScope.pagingInfo.search);
            $location.search('page', $rootScope.pagingInfo.page);
            $location.search('department', $rootScope.pagingInfo.department);
        }
        if ($rootScope.pagingInfo.department == '') {
            $location.search('department', null);
        }
    }


    $scope.loadList();







});
