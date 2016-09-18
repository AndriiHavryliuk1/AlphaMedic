using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rest.Models
{
    public enum DoctorType
    {
        Doctor=2,
        HeadDepartment=5,
        HospitalDean=4
    }


    [Table("Doctors")]
    public class Doctor : Employee
    {
        public Doctor()
        {
            Feedbacks = new List<Feedback>();
        }


        public string Degree { get; set; }
        public string Education { get; set; }
        [ForeignKey("Schedule")]
        public int? ScheduleId { get; set; }
        public int DepartmentId { get; set; }
        public DoctorType DoctorType { get; set; }

        
        public virtual Schedule Schedule { get; set; }
        public virtual Department Department { get; set; }

        public virtual ICollection<Feedback> Feedbacks { get; set; }


        [NotMapped]
        public string FullName { get { return this.Name + " " + this.Surname; } }

    }
}