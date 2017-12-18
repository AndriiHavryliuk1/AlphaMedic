using System;
using Rest.Models;

namespace Rest.Dtos.Pagginations
{
	public class AppointmenPaginationsDto
	{
		public DateTime? PeriodFrom { get; set; }
		public DateTime? PeriodTill { get; set; }
		public AppointmentState? State { get; set; } = null;
		public int Page { get; set; } = 1;
		public int ItemsPerPage { get; set; } = 15;
		public int? Doctor { get; set; } = null;
		public int? Department { get; set; } = null;
	}
}