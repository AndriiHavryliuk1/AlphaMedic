using System.ComponentModel.DataAnnotations.Schema;

namespace RestWebApi.Models
{

    public class Doctor
    {
        public int DoctorId { get; set; }

        public string Degree { get; set; }

        public string Education { get; set; }

        public int UserId { get; set; }

        public int DepartmentId { get; set; }

        //[ForeignKey("DepartmentId")]
        //public virtual Department Department { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
