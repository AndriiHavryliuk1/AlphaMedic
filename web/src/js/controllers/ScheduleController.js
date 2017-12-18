var app = angular.module('alphaMedicApp');
app.controller('ScheduleController', function(URL_FOR_REST, $timeout, $scope, $http, $routeParams) {

    $http.get(URL_FOR_REST.url + "api/doctors/schedule/" + $routeParams["id"])
        .success(function(responce) {
            $scope.doctor = responce;
            $scope.schedule = {
                ScheduleId: $scope.doctor.ScheduleId,
                StartWorkingTime: $scope.doctor.StartWorkingTime,
                FinishWorkingTime: $scope.doctor.FinishWorkingTime
            };
        });

    $scope.EditWorkingHours = function() {
        $http.put(URL_FOR_REST.url + "api/scheduleUpdate/" + $routeParams["id"], $scope.schedule)
            .success(function() {
                $scope.succed = true;
                $scope.doctor.StartWorkingTime = $scope.schedule.StartWorkingTime;
                $scope.doctor.FinishWorkingTime = $scope.schedule.FinishWorkingTime;
                $timeout(function() {
                    $scope.succed = false;
                }, 2000);
            }).error(function() {
                $scope.failed = true;
                $timeout(function() {
                    $scope.failed = false;
                }, 2000);
            });
    };
    $http.get(URL_FOR_REST.url + "api/schedule/" + $routeParams["id"])
        .success(function(responce) {
            $scope.events = responce;
        }).then(function() {
            $('#calendar').fullCalendar({
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,agendaWeek,agendaDay'
                },
                selectable: true,
                selectHelper: true,
                select: function(start, end) {
                    var title = prompt('Event Title:');
                    var eventData;
                    if (title) {
                        eventData = {
                            title: title,
                            start: start,
                            end: end
                        };
                        $('#calendar').fullCalendar('renderEvent', eventData, true); // stick? = true
                    }
                    $('#calendar').fullCalendar('unselect');
                },
                editable: true,
                eventLimit: true,
                events: $scope.events
            });
        });
});
