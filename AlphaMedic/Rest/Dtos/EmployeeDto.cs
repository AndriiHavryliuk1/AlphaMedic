using Rest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rest.Dtos
{
    public class EmployeeDto:UserDto
    {
        public DateTime EmploymentDate { get; set; }
        public string EmploymentRecordBookNumber { get; set; }
        public DateTime? DismissalDate { get; set; }
        public EmployeeType EmployeeType { get; set; }
        
        public void UpateEmployee(Employee emp)
        {
            emp.Name = Name;
            emp.Surname = Surname;
            emp.Phone = Phone;
            emp.Email = Email;
            emp.EmploymentRecordBookNumber = EmploymentRecordBookNumber;
            emp.Address = Address;
            emp.EmployeeType = EmployeeType;
            
        }

    }
}