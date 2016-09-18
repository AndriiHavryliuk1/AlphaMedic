
angular.module('alphaMedicApp')

.factory('PatientService', ['$http', 'URL_FOR_REST'
    function ($http){
        var service = {};

        service.GetPatients = function (pagingInfo) {
            return $http.get(URL_FOR_REST.url + 'api/Patients', { params: pagingInfo });
        };

        return service;
    }]);
