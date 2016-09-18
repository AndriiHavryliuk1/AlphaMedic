using System;
using System.Collections.Generic;

namespace alphamedic.core
{
    public class Schedule
    {
        public List<Appointment> Appointments { get; set; }

        public DateTime StartWorkingTime { get; set; }

        public DateTime FinishWorkingTime { get; set; }
    }
}