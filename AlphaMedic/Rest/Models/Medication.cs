using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rest.Models
{
    public class Medication
    {

        public Medication()
        {
            this.Treatments = new List<Treatment>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MedicationId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

       
        public virtual ICollection<Treatment> Treatments { get; set; }
    }
}