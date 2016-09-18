using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rest.Models
{
    public class Schedule
    {
        public int ScheduleId { get; set; }
        public TimeSpan StartWorkingTime { get; set; }
        public TimeSpan FinishWorkingTime { get; set; }   
    }
}