using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rest.Models
{
    [Table("Procedures")]
    public abstract class Procedure
    {
        [Key, ForeignKey("Appointment")]
        public int? ProcedureId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public int? MedicalHistoryId { get; set; }
                
        public DateTime Date { get; set; }
        public decimal? Price { get; set; }
       
        public virtual MedicalHistory MedicalHistory { get; set; }        
        public virtual Appointment Appointment { get; set; }
    }
}