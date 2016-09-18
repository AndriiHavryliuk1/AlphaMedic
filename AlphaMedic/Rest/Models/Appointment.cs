using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Rest.Models
{
    public enum AppointmentState
    {
        Unconfirmed, 
        Accepted, 

    }

    public class Appointment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int AppointmentId { get; set; }

        public AppointmentState State { get; set; }

        [ForeignKey("Doctor")]
        public int? DoctorId { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; }

        public string Description { get; set; }
        
        public DateTime? Date { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? Duration { get; set; }

        public virtual Doctor Doctor { get; set; }
        public virtual Procedure Procedure { get; set; }
        public virtual Patient Patient { get; set; }
        
    }
}