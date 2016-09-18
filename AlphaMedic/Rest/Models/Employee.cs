using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Rest.Models
{

    public enum EmployeeType
    {
        Administrator, 
        Receptionist, 
        Doctor
    }


    [Table("Employees")]
    public abstract class Employee : User
    {
        public  DateTime EmploymentDate { get; set; }
        public  string EmploymentRecordBookNumber { get; set; }
        public  DateTime? DismissalDate { get; set; }
        public  EmployeeType EmployeeType { get; set; }
        
    }
}