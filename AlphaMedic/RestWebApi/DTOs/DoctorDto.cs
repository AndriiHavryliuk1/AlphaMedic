using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestWebApi.DTOs
{
    public class DoctorDto
    {

        public int DoctorId { get; set; }

        public string Degree { get; set; }

        public string Education { get; set; }

        public string DoctorFullName { get; set; }
    }
}