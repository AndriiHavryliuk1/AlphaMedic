using System;

namespace RestWebApi.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public AppointmentState AppointmentState { get; set; }

        public int ProcedureId { get; set; }

        public string AppointmentDescription { get; set; }

        public DateTime AppointmentDate { get; set; }

        public DateTime AppointmentDuration { get; set; }
    }

    public enum AppointmentState
    {
        a,b,c,d
    }
}