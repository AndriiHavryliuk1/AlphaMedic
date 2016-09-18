using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Rest.Models
{
    public class Department:IFeedbackObject
    {
        public Department()
        {
            Doctors = new List<Doctor>();
            Feedbacks = new List<Feedback>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int DepartmentId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string URLImage { get; set; }


        public virtual ICollection<Doctor> Doctors { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }


        [NotMapped]
        public Doctor HeadDepartment { get; set; }
    }
}