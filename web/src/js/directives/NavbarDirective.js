app.directive("navbar", function($rootScope) {
    return {
        restrict: "A",
        template: function(element, attrs) {
            $rootScope.isAuthorized = localStorage.getItem('token') != undefined;
            return `<div class="form-group" ng-show="!isAuthorized">
            <ul class="nav navbar-nav">
                <li  ng-class="getPath() == '/signIn' ? 'active' : ''">
                    <a href="#/signIn">Sign In</a>
                </li>
                <li  ng-class="getPath() == '/signUp' ? 'active' : ''">
                    <a href="#/signUp">Sign Up</a>
                </li>
            </ul>
        </div>
        <div class="form-group" ng-show="isAuthorized">
        <ul class="nav navbar-nav">
            <li ng-class="getPath() == '/Cabinet' ? 'active' : ''" >
                <a ng-click="Cabinet()" href="">Cabinet</a>
            </li>
            <li>
                <a ng-click="LogOut()" href="">Sign Out</a>
            </li>
        </ul>
    </div>`
        }
    }
});

/*
ng-class="GetChecked() == 'signUp' ? 'active' : ''
ng-class="GetChecked() == 'signIn' ? 'active' : ''"
ng-class="GetChecked() == 'Cabinet' ? 'active' : ''"
*/
