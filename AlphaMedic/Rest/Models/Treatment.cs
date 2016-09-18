using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Rest.Models
{
    [Table("Treatments")]
    public class Treatment : Procedure
    {
        public Treatment()
        {
            Medications = new List<Medication>();
        }       
        public string Result { get; set; }

        public virtual ICollection<Medication> Medications { get; set; }
    }
}