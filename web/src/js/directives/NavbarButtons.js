app.directive("navbarButtons", function($rootScope, jwtHelper, USER_ROLES) {
    return {
        restrict: "A",
        link: function($scope, $element, attrs) {


            $rootScope.$on('navabarButtonsShow', function(event, data) {
                addButtons(data, $rootScope, $element, USER_ROLES);

            });

        },
        template: function(element, attr) {

            var x = localStorage.getItem('token') != undefined ? jwtHelper.decodeToken(localStorage.getItem('token')) : "guest";
            addButtons(x, $rootScope, element, USER_ROLES);
        }


    }
});

var addButtons = function(data, $rootScope, $element, USER_ROLES) {
        if (data.role == USER_ROLES.doctor || data.role == USER_ROLES.hospitalDean || data.role == USER_ROLES.headDepartment) {

            $rootScope.isDoctor = true;
        }

        $rootScope.canRegister = data.role == USER_ROLES.patient || data.role == "guest" || data == "guest";
        $element.empty();

        $element.append(buutonsCode(data.id, $rootScope.isDoctor, $rootScope.canRegister,null));

        $rootScope.$watch("ScheduleId",function(){

            if($rootScope.ScheduleId!=null)
            {
              $element.empty();

              $element.append(buutonsCode(data.id, $rootScope.isDoctor, $rootScope.canRegister,$rootScope.ScheduleId));
            }

        })
    }
    /*
    ng-class="GetChecked() == 'departments' ? 'active' : ''"
    ng-class="GetChecked() == 'doctors' ? 'active' : ''"
    ng-show="isDoctor" ng-class="GetChecked() == 'patients' ? 'active' : ''"
    ng-show="isDoctor" ng-class="GetChecked() == 'schedule' ? 'active' : ''"
    ng-class="GetChecked() == 'registrationForAppointment' ? 'active' : ''"
    */
var buutonsCode = function(id, isDoctor, canRegister,scheduleId) {
    var button1 = `<ul class="nav navbar-nav col-lg-7 col-md-7 col-sm-7">
                    <li ng-class="getPath2 == '/departments' ? 'active' : ''" > <a   href="#/departments">Departments</a>
                </li>`;

    var button2 = "";
    if (!isDoctor) {
        button2 = ` <li ng-class="getPath2 == '/doctors' ? 'active' : ''">
                        <a  href="#/doctors?page=1">Doctors</a>
                </li>`;
    }
    var button3 = "";
    if (isDoctor) {
        button3 = ` <li  ng-class="getPath2 == '/patients' ? 'active' : ''">
                          <a   href="#/patients?page=1&doctor=` + id + `">Patients</a>
                    </li >`;
    }

    var button4 = "";
    if (isDoctor) {
        button4 = ` <li ng-class="getPath2 == '/schedule' ? 'active' : ''">
                        <a href="#/schedule/` + scheduleId + `">Schedule</a>
                    </li>`;
    }

    var end = "";
    if (canRegister) {
        end = `<li  ng-class="getPath2 == '/registrationForAppointment' ? 'active' : ''" >
                    <a href="#/registrationForAppointment">Register for appointment</a>
                </li>
            </ul>`;
    } else {
        end = `  </ul>`;
    }
    return button1 + button2 + button3 + button4 + end;
}
