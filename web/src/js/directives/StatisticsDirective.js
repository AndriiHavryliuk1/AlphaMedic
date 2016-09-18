var app = angular.module('alphaMedicApp');

app.directive("statTable", function() {
    var table;

    return {
        scope: false,
        link: function($scope, $element, $attrs) {
          $scope.selectedDoc="0";
            $scope.$on('changeSelectedOption', function(event, localscope) {
                $scope.$watch('selectedDoc',function(){
                  $element.empty();

                table = `<table id="statTab" class="table" border=1>
                         <tr> <th>Department</th>
                          <th>Doctors</th>
                          <th>Patients Count</th>
                          <th>Appoitment Count</th>
                          <th>Total Work Hours</th>
                          </tr>`;

                var deps = [];

                for (var key in localscope.statDepartments) {
                    var docs = [];
                    if($scope.selectedDoc==null ||$scope.selectedDoc==0){

                    for (var docKey in localscope.statDoctors) {
                        if (localscope.statDoctors[docKey].DepartmentId == localscope.statDepartments[key].DepartmentId) {
                            docs.push(localscope.statDoctors[docKey]);
                        }
                    }
                  }
                  else {
                    for (var docKey in localscope.statDoctors) {
                        if (localscope.statDoctors[docKey].DepartmentId == localscope.statDepartments[key].DepartmentId
                        && localscope.statDoctors[docKey].DoctorId==$scope.selectedDoc) {
                            docs.push(localscope.statDoctors[docKey]);
                        }
                    }
                  }


                    if (docs.length > 0) {
                        table += `<tr><td rowspan="` + docs.length + `">` +
                            localscope.statDepartments[key].Name +
                            `</td>` + GenerateColumns(docs[0].UserName,
                                docs[0].PatientsCount,
                                docs[0].AppoitmentsCount,
                                docs[0].TotalWorkHours) + `</tr>`;

                        for (var i = 1; i < docs.length; i++) {
                            table += `<tr>` + GenerateColumns(docs[i].UserName,
                                docs[i].PatientsCount,
                                docs[i].AppoitmentsCount,
                                docs[i].TotalWorkHours) + `</tr>`;
                        }
                    }

                }
                table += "</table>";
                localscope.table = table;
                $element.append(table);
});
            });

        }

    }
});
var GenerateColumns = function(UserName, PatientsCount, AppoitmentsCount, TotalWorkHours) {
    var table = `<td>` + UserName +
        `</td><td>` + PatientsCount + `</td>
        <td>` + AppoitmentsCount + `</td>
        <td>` + TotalWorkHours + `</td>`

    return table;
}
