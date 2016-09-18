var app = angular.module('alphaMedicApp', ["ngRoute", "angular.filter","angular-encryption", "angular-jwt", "ui.bootstrap.datetimepicker","ngAnimate", "ngSanitize", "ui.bootstrap"]);
app.config(function($routeProvider) {
    $routeProvider.when('/', {
        templateUrl: 'src/views/Main.html',
        controller: ''
    });

    $routeProvider.when('/main', {
        templateUrl: 'src/views/Main.html',
        controller: ''
    });

    $routeProvider.when('/error', {
        templateUrl: 'src/views/ErrorPage.html',
        controller: ''
    });

    $routeProvider.when('/error/:id', {
        templateUrl: 'src/views/ErrorPage.html',
        controller: ''
    });

    $routeProvider.when('/departments', {
        templateUrl: 'src/views/ListOfDepartments.html',
        controller: 'ListOfDepartmentsController'
    });

    $routeProvider.when('/registrationForAppointment', {
        templateUrl: 'src/views/RegistrationForAppointment.html',
        controller: ''
    });

    $routeProvider.when('/doctors', {
        templateUrl: 'src/views/ListOfDoctors.html',
        controller: 'ListOfDoctorsController',
        reloadOnSearch: false
    });

    $routeProvider.when('/confirmRegistration', {
        templateUrl: 'src/views/ConfirmRegistration.html',
        controller: 'ConfirmRegistrationController'
    });

    $routeProvider.when('/confirmRegistration/:id', {
        templateUrl: 'src/views/ConfirmRegistration.html',
        controller: 'ConfirmRegistrationController'
    });

    $routeProvider.when('/signUp', {
        templateUrl: 'src/views/Registration.html',
        controller: ''
    });

    $routeProvider.when('/signIn', {
        templateUrl: 'src/views/SignIn.html',
        controller: ''
    });

    $routeProvider.when('/patientCabinet', {
        templateUrl: 'src/views/PatientPersonalCabinet.html',
        controller: 'patientCabinet'
    });

    $routeProvider.when('/appointmentInfo/:id', {
        template: '<div appointment-info></div>',
        controller: ''
    });

    $routeProvider.when('/patients/:id', {
        templateUrl: 'src/views/PatientInfo.html',
        controller: 'PatientInfoController',
        reloadOnSearch: false
    });

    $routeProvider.when('/medicalHistory/:id', {
        templateUrl: 'src/views/MedicalHistory.html',
        controller: 'MedicalHistoryController',
        reloadOnSearch: false
    });

    $routeProvider.when('/recoveryPassword', {
        templateUrl: 'src/views/RecoveryPassword.html',
        controller: ''
    });


  //  "medicalhistory/" + $routeParams["id"] + "/page/" + $scope.searchInfo.page

    $routeProvider.when('/addProcedure/:id', {
        templateUrl: 'src/views/AddProcedure.html',
        controller: ''
    });

    $routeProvider.when('/schedule/:id', {
        templateUrl: 'src/views/Schedule.html',
        controller: 'ScheduleController'
    });

    $routeProvider.when('/departments/:id', {
        templateUrl: 'src/views/DepartmentInfo.html',
        controller: 'DepartmentInfoController'
    });

    $routeProvider.when('/doctors/:id', {
        templateUrl: 'src/views/DoctorInfo.html',
        controller: 'DoctorsInfoController'
    });

    $routeProvider.when('/patients', {
        templateUrl: 'src/views/ListOfPatients.html',
        controller: 'ListOfPatientsController',
        reloadOnSearch: false
    });

    $routeProvider.when('/patients/:id/patients', {
        templateUrl: 'src/views/ListOfPatients.html',
        controller: 'ListOfDoctorsPatientsController',
        reloadOnSearch: false
    });


    $routeProvider.when('/doctorCabinet', {
        templateUrl: 'src/views/DoctorPersonalCabinet.html',
        controller: 'DoctorCabinetController'
    });

    $routeProvider.when('/statistics/:id', {
        templateUrl: 'src/views/Statistics.html',
        controller: 'StatisticDepartmentDataController'
    });

    $routeProvider.when('/receptionistCabinet', {
        templateUrl: 'src/views/receptionistPersonalCabinet.html',
        controller: 'ReceptionistCabinetController',
        reloadOnSearch: false
    });

    $routeProvider.when('/administratorCabinet', {
        templateUrl: 'src/views/AdministratorCabinet.html',
        controller: 'AdminCabinetController'
    });

    $routeProvider.otherwise({
        redirectTo: '/error/404'
    });
});
app.config(function Config($httpProvider, jwtInterceptorProvider) {
    jwtInterceptorProvider.tokenGetter = function(jwtHelper, $http) {
        return localStorage.getItem('token');
    }
    $httpProvider.interceptors.push('jwtInterceptor');
});
