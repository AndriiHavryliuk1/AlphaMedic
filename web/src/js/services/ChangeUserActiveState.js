angular.module('alphaMedicApp').
service("changeStateService", function() {
    this.patch = function(user, state, route,$http) {
        user.Active = state;
        $http({
                url:route + user.UserId,
                headers: {
                    'Content-Type': 'application/json'
                },
                method: 'PATCH',
                data: [{
                    op: "replace",
                    path: "/Active",
                    value: user.Active
                }]
            }).success(function(data, status, headers, config) {
            })
            .error(function(data, status, headers, config) {
                alert('something gone wrong');
                console.log(data);
                user.Active = !state;
            });
    };




});
