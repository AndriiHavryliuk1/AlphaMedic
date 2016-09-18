using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestWebApi.DTOs
{
    public class DoctorDetailsDto
    {
        public int DoctorId { get; set; }

        public string DoctorFullName { get; set; }

        public string Degree { get; set; }

        public string Education { get; set; }

        public string Phone { get; set; }

      //  public string DepartmentName { get; set;}

    }
}