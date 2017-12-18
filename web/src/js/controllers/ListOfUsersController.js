var app = angular.module('alphaMedicApp');
//admin cabinet patinet list
app.controller('ListOfAllPatientsController', function(URL_FOR_REST, $scope, $http, $rootScope, $location, FilterService, PaginationArrayService, PagginationService, ChangeUserInfoService, fileUploadService) {

    $rootScope.pagingInfo = {
        page: FilterService.getPage() == '' ? 1 : FilterService.getPage(),
        itemsPerPage: 15,
        sortBy: 'UserId',
        reverse: false,
        search: $location.search().search == undefined ? '' : $location.search().search, //$routeParams.value == undefined ? '' : $routeParams.value,
        totalItems: 0,
        isActive: $location.search().isActive == undefined ? '' : $location.search().isActive
    };

    $rootScope.loadAdminListPatient = function() {
        $scope.loadList()
    }

    $scope.loadList = function() {
        $scope.allPatients = null;
        $http.get(URL_FOR_REST.url + "api/Patients/allPatients", {
                params: $rootScope.pagingInfo
            })
            .success(function(responce) {
                $scope.allPatients = responce.data;

                $rootScope.pagingInfo.totalItems = responce.count;
                $scope.pages = Math.ceil(responce.count / $rootScope.pagingInfo.itemsPerPage);
                $scope.paginArray = PaginationArrayService.Array($scope.pages, $rootScope.pagingInfo.page);

            });

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


    $scope.ChangeUser = function() {
        ChangeUserInfoService.ChangeUser($scope.patient, $scope.bufferUser,  $scope.bufferUser.UserId,$scope)

        if ($scope.picture != "undefined") {
            fileUploadService.uploadFileToUrl($scope.picture, URL_FOR_REST.url + "api/image/" + $scope.bufferUser.UserId).then(function(response) {
                $scope.bufferUser.URLImage = response.data;
            });
        }
    };

    $scope.GetData = function(patient, passParam) {
        $scope.hidePasswordFields = passParam;
        $scope.patient = patient;
        $scope.bufferUser =angular.copy(patient);

    };
});
