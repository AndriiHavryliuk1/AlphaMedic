using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestWebApi.DTOs
{
    public class PatientDto
    {
        public int PatientId { get; set; }

        public string PatientFullName { get; set; }

        public string CurrentProcedure{get;set;}

        public int DoctorId { get; set; }

        public string DoctorFullName { get; set; }

        //public ProcedureDto CurrentProcedure { get; set; }
    }
}