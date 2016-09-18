var app = angular.module('alphaMedicApp');
app.directive("errorpage",['$routeParams', function($routeParams) {
    return {
        restrict: 'A',
        template: function() {
            switch (parseInt($routeParams['id'])) {
                case 404:
                    return `<div class="hero-unit centerPageError">
                                    <h1>Page Not Found <small><font face="Tahoma" color="red">Error 404</font></small></h1>
                                    <br />
                                    <p>The page you requested could not be found, either contact your webmaster or try again. Use your browsers <b>Back</b> button to navigate to the page you have prevously come from</p>
                                    <p><b>Or you could just press this neat little button:</b></p>
                                    <a href="#/main" class="btn btn-large btn-info"><i class="icon-home icon-white"></i> Take Me Home</a>
                                </div>`;
                    break;
                case 403:
                    return `<div class="hero-unit centerPageError">
                                    <h1>Access forbidden <small><font face="Tahoma" color="red">Error 403</font></small></h1>
                                    <br />
                                    <p>The page you requested could not access to you, either contact your webmaster or try again. Use your browsers <b>Back</b> button to navigate to the page you have prevously come from</p>
                                    <p><b>Or you could just press this neat little button:</b></p>
                                    <a href="#/main" class="btn btn-large btn-info"><i class="icon-home icon-white"></i> Take Me Home</a>
                                </div>`;
                    break;
                case 500:
                return `<div class="hero-unit centerPageError">
                                <h1>Server error <small><font face="Tahoma" color="red">Error 500</font></small></h1>
                                <br />
                                <p>Please try to repeat this operation later, or contact your webmaster . Use your browsers <b>Back</b> button to navigate to the page you have prevously come from</p>
                                <p><b>Or you could just press this neat little button:</b></p>
                                <a href="#/main" class="btn btn-large btn-info"><i class="icon-home icon-white"></i> Take Me Home</a>
                            </div>`;
                    break;
            }
        }
    }
}]);
