using Rest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rest.Dtos
{
    public class DoctorDto : ShortUserDto
    {
        public int DepartmentId { get; set; }
        public int DoctorId { get; set; }
        public string DepartmentName { get; set; }
        public string Degree { get; set; }
        public string Education { get; set; }
        public string DoctorType { get; set; }
        public TimeSpan StartWorkingTime { get; set; }
        public TimeSpan FinishWorkingTime { get; set; }
        public string URLImage { get; set; }
        public bool? Active { get; set; }
        public DoctorType DoctorTypeInt { get; set; }
    }
}