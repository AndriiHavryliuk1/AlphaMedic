var app = angular.module('alphaMedicApp');
app.filter('docTypeFilter', function() {
    return function(param) {
        switch (param) {
            case 2:
                return "Doctor";
                break;
            case 5:
                return "HeadDepartment";
                break;
            case 4:
                return "HospitalDean";
        }
    }
});


app.controller('ListOfDoctorsAdminController', function(URL_FOR_REST, $rootScope, $location, FilterService, $scope, $http, PaginationArrayService, PagginationService, ChangeUserInfoService) {


    $rootScope.pagingInfo = {
        page: FilterService.getPage() == '' ? 1 : FilterService.getPage(),
        itemsPerPage: 20,
        sortBy: 'DoctorId',
        reverse: false,
        search: $location.search().search == undefined ? '' : $location.search().search, //$routeParams.value == undefined ? '' : $routeParams.value,
        totalItems: 0,
        isActive: $location.search().isActive == undefined ? '' : $location.search().isActive
    };


    $scope.doctortypes = [{
        id: 2,
        value: "Doctor"
    }, {
        id: 5,
        value: "HeadDepartment"
    }, {
        id: 4,
        value: "HospitalDean"
    }];

    $scope.loadList = function() {
        $http.get(URL_FOR_REST.url + "api/Doctors", {
                params: $rootScope.pagingInfo
            })
            .success(function(responce) {
                $scope.doctors = responce.data;
                $rootScope.pagingInfo.totalItems = responce.count;
                $scope.pages = Math.ceil(responce.count / $rootScope.pagingInfo.itemsPerPage);
                $scope.paginArray = PaginationArrayService.Array($scope.pages, $rootScope.pagingInfo.page);
            })

        $location.search('search', $rootScope.pagingInfo.search);
      //  $location.search('page', $rootScope.pagingInfo.page);
      FilterService.setPage($rootScope.pagingInfo.page);
        $location.search('isActive', $rootScope.pagingInfo.isActive);
        if ($rootScope.pagingInfo.search == '') {
            $location.search('search', null);
        }
        if ($rootScope.pagingInfo.isActive == '') {
            $location.search('isActive', null);
        }

    }

    $scope.loadList();


    $rootScope.loadAdminList = function() {
        $scope.loadList()
    }

    $http.get(URL_FOR_REST.url + "api/Departments")
        .success(function(responce) {
            $scope.departments = responce;
        });

    $scope.ChangeUser = function() {
        ChangeUserInfoService.ChangeDoctor($scope.user, $scope.user.UserId);

    };

    $scope.GetData = function(doctor) {
        $scope.user = doctor;
    };


});
