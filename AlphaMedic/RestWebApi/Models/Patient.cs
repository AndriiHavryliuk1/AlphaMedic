using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RestWebApi.Models
{
    public class Patient
    {

        
        public int PatientId { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public BloodGroup BloodGroup { get; set; }
       
        public virtual ICollection<Procedure> Procedures { get; set; }
    }

    public enum BloodGroup
    {
        a, b, c, d
    }


}