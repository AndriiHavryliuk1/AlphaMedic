using System;

namespace Rest.Dtos
{
    public class FeedbackDto
    {
        public int FeedbackId { get; set; }
        public int? DepartmentId { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }               
        public string PatientFullName { get; set; }
        public string   PatientURLImage { get; set; }
    }
}