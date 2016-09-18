using System.ComponentModel.DataAnnotations.Schema;

namespace RestWebApi.Models
{
    public class Procedure
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }

        public int PatientId { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }

        public int Price { get; set; }

        public string Diagnosis { get; set; }
    }
}