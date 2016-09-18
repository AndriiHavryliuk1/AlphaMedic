using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Rest.Models
{
    [Table("Patients")]
    public class Patient : User
    {
        public Patient()
        {
            Appointments = new List<Appointment>();
            Feedbacks = new List<Feedback>();
        }

       
        public int? MedicalHistoryId { get; set; }
        public int? BloodGroupId { get; set; }

         
        public virtual MedicalHistory MedicalHistory { get; set; }
        public virtual BloodGroup BloodGroup { get; set; }
        public virtual ICollection<Appointment> Appointments { get; private set; }
        public virtual ICollection<Feedback> Feedbacks { get; private set; }

        [NotMapped]
        public string FullName { get { return this.Name + " " + this.Surname; } }
    }
}