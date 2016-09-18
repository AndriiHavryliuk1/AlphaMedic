using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rest.Models
{
    public class Diagnosis
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int DiagnosisId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}