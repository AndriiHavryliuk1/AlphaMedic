using Rest.Models;

namespace Rest.Dtos
{
	public class AppointmentDto
	{
		public int AppointmentId { get; set; }
		public string ProcedureName { get; set; }
		public string DoctorFullName { get; set; }
		public int? DoctorId { get; set; }
		public int DepartmentId { get; set; }
		public Doctor Doctor { get; set; }
	}
}