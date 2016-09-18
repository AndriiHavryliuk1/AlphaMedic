using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rest.Models
{
   
    public class MedicalHistory
    {
        public MedicalHistory()
        {
            Procedures = new List<Procedure>();
            WarningLabels = new List<WarningLabel>();
        }

        [Key,ForeignKey("Patient")]
        public int MedicalHistoryId { get; set; }

        public virtual Patient Patient { get; set; }
        public virtual ICollection<WarningLabel> WarningLabels { get; set; }
        public virtual ICollection<Procedure> Procedures { get; set; }
    }
}