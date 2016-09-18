using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rest.Models
{
    public class WarningLabel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WarningLabelId { get; set; }
        public int MedicalHistoryId { get; set; }
        public string Description { get; set; }

        public virtual MedicalHistory MedicalHistory { get; set; }
    }
}