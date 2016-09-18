app.directive("tableView", function() {
    return {
        restrict: 'A',
        templateUrl: function(element, attrs) {

            switch (attrs["tableView"]) {
                case "doctorsView":
                    return "src/views/templates/ViewDoctorsTemplate.html";
                    break;
                case "patientsView":
                    return "src/views/templates/ViewPatientsTemplate.html";
                    break;
                case "registerEmployeeView":
                    return "src/views/templates/RegisterEmployeeTemplate.html";
                    break;
                case "employeeView":
                    return "src/views/templates/ViewEmployeeTemplate.html";
                    break;
                case "medicationsView":
                    return "src/views/templates/ViewMedicationsTemplate.html";
                    break;
                case "newDepartment":
                    return "src/views/templates/AddDepartmentTemplate.html";
                    break;


            }




        }
    }
});
