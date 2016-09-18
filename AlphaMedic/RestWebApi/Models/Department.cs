using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace RestWebApi.Models
{
    public class Department
    {
       
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentDescription { get; set; }
        public int DepartmentHeadId { get; set; }

        [ForeignKey("DepartmentHeadId")]
        public virtual Doctor DepartmentHead { get; set; }
      
    }
}