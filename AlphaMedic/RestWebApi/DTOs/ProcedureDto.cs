using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestWebApi.DTOs
{
    public class ProcedureDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int DoctorId { get; set; }

        public string DoctorFullName { get; set; }
    }
}