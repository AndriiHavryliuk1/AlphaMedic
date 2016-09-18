using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Rest.Models
{
    public interface IFeedbackObject
    {
        ICollection<Feedback> Feedbacks { get; set; }
    }

    public class Feedback
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FeedbackId { get; set; }
        
        [ForeignKey("Patient")]
        public int? PatientId { get; set; }
        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }
        [ForeignKey("Doctor")]
        public int? DoctorId { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
       // public int FeedbackObjectId { get; set; }

       // public virtual IFeedbackObject FeedbackObject { get; set; }
        
        public virtual Department Department { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual Patient Patient { get; set; }

    }
}