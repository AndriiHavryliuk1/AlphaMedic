using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RestWebApi.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }

        public string FeedbackDescription { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        
        public DateTime FeedbackDate { get; set; }
        
        public int? DepartmentId { get; set; }

        [ForeignKey("DepartmentId"),]
        public virtual Department Department { get; set; }

        public int DoctorId { get; set; }
        
        
    }
}