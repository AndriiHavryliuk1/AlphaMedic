app.directive('medicationpriceEnter', function($http, URL_FOR_REST, $timeout) {
    return function(scope, element, attrs) {
        element.bind("keydown keypress", function(event) {
            if (event.which === 13) {
                scope.$apply(function() {
                    var price = attrs.value;
                    $http({
                            url: URL_FOR_REST.url + "api/medications/" + attrs.medicationpriceEnter,
                            headers: {
                                'Content-Type': 'application/json'
                            },
                            method: 'PATCH',
                            data: [{
                                op: "replace",
                                path: "/Price",
                                value: price
                            }]
                        }).success(function(data, status, headers, config) {
                            scope.succed = true;
                            $timeout(function() {
                                scope.succed = false;
                            }, 2000);
                        })
                        .error(function(data, status, headers, config) {
                            scope.failed = true;
                            $timeout(function() {
                                scope.failed = false;
                            }, 2000);
                            console.log(data);
                        });
                });
                event.preventDefault();
            }
        });
    };
});

app.directive('durationPicker', function($http, URL_FOR_REST, $timeout) {
    return function(scope, element, attrs) {
        element.bind("focusout", function(event) {
            scope.$apply(function() {
                scope.appointment.Duration = formatDuration(scope.appointment.Duration);
            });
            event.preventDefault();
        });
        element.bind("keydown keypress", function(event) {
            if (event.which === 13) {
                scope.$apply(function() {
                    scope.appointment.Duration = formatDuration(scope.appointment.Duration);
                });
                event.preventDefault();
            }
        });
    };
});


var formatDuration = function(number) {

    var timeData = number.split(":");
    number = timeData[0].concat(timeData[1]);

    var a = parseInt(number);
    if (isNaN(a)) {
        return number = "00:10";

    }

    if (typeof(a) === 'number') {
        var s = a.toString();
        switch (s.length) {
            case 2:
                s = "00" + s;
                break;
            case 3:
                s = "0" + s;
                break;
            case 4:
                break;
            default:
                s = "0010";

        }

        var h = s.substr(0, 2);
        var m = s.substr(2, 2);

        h = parseInt(h);
        m = parseInt(m);

        if (m > 59) {
            h = h + 1;
            m = m - 60;
        }
        m = m.toString();

        if (m.length == 1) {
            m = "0" + m;
        }

        if (h.length == 1) {
            h = "0" + h;
        }
        number = h + ":" + m;
        return number;
    }
}
