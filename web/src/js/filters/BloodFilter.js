var app = angular.module('alphaMedicApp')

app.filter('bloodFilter', function() {
    return function(param) {
        switch (param) {
            case 0:
                return "AB+"
                break;
            case 1:
                return "AB-"
                break;
            case 2:
                return "A+"
                break;
            case 3:
                return "A-"
                break;
            case 4:
                return "A+"
                break;
            case 5:
                return "B+"
                break;
            case 6:
                return "B-"
                break;
            case 7:
                return "O+"
                break;
            case 8:
                return "O-"
                break;

            default:
                return "Undefinded"

        }

    }
})
