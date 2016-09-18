using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rest.Dtos
{
    public class PatientDto : ShortUserDto
    {
        public string URLImage { get; set; }
        public AppointmentDto Procedure { get; set; }


        //    x.UserId,
        //                x.Name,
        //                x.Surname,
        //                URLImage = Constants.ThisServer + x.URLImage,

        //                Procedure = x.Appointments.Select(
        //                p => new
        //                {
        //                    p.AppointmentId,
        //                    ProcedureName = p.Procedure.Name,
        //                    DoctorFullName = p.Doctor.Name + " " + p.Doctor.Surname,
        //                    DoctorId = p.Doctor.UserId,
        //                    DepartmentId = p.Doctor.DepartmentId
        //}
        //                ).FirstOrDefault()
    }
}