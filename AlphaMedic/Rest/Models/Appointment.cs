using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rest.Models
{
	public enum AppointmentState
	{
		Unconfirmed,
		Accepted,
		Finished
	}

	public class Appointment
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int AppointmentId { get; set; }

		public AppointmentState State { get; set; }

		[ForeignKey("Doctor")]
		public int? DoctorId { get; set; }

		[ForeignKey("Patient")]
		public int PatientId { get; set; }

		public string Description { get; set; }

		public DateTime? Date { get; set; }

		[DataType(DataType.Time)]
		public TimeSpan? Duration { get; set; }

		public bool? Checked { get; set; }

		public virtual Doctor Doctor { get; set; }
		public virtual Procedure Procedure { get; set; }
		public virtual Patient Patient { get; set; }

	}
}