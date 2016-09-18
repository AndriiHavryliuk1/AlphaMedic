var app = angular.module('alphaMedicApp');

app.constant('USER_ROLES', {
    admin: 'Administrator',
    receptionist: 'Receptionist',
    patient: 'Patient',
    doctor: 'Doctor',
    hospitalDean:'HospitalDean',
    headDepartment:'HeadDepartment'
})

app.constant('URL_FOR_REST', {
    url: 'http://localhost:63741/'
})

app.constant('DOCTOR_TYPES',
    [{
        "id": "0",
        "value": "Doctor"
    },
    {
        "id": "1",
        "value": "HeadDepartment"
    },
     {
        "id": "2",
        "value": "HospitalDean"
    }]

)
