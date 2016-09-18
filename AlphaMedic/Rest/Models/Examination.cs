using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Rest.Models
{
    [Table("Examinations")]
    public class Examination : Procedure
    {
        public int? DiagnosisId { get; set; }

        public virtual Diagnosis Diagnosis { get; set; }
    }
}