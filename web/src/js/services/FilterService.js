angular.module('alphaMedicApp')

.service('FilterService', function($rootScope) {
    // private variable
    var property = '';
    var doctor = '';
    var procedureType = '';
    var periodFrom = '';
    var periodTill = '';
    var page = '';
    var userId = '';
    return {
        getDepartment: function() {
            return property;
        },
        setDepartment: function(value) {
            property = value;
        },
        getUserId: function() {
            return userId;
        },
        setUserId: function(value) {
            userId = value;
        },
        getDoctor: function() {
            return doctor;
        },
        setDoctor: function(value) {
            doctor = value;
        },
        getProcedure: function() {
            return procedureType;
        },
        setProcedure: function(value) {
            procedureType = value;
        },
        getPeriodFrom: function() {
            return periodFrom;
        },
        setPeriodFrom: function(value) {
            periodFrom = value;
        },
        getPeriodTill: function() {
            return periodTill;
        },
        setPeriodTill: function(value) {
            periodTill = value;
        },
        getPage: function() {
            return page;
        },
        setPage: function(value) {
            page = value;
        }
    };
})
