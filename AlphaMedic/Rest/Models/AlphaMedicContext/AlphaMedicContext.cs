using System.Data.Entity;

namespace Rest.Models.AlphaMedicContext
{
	public class AlphaMedicContext : DbContext
	{
		public AlphaMedicContext() : base(Constants.DbConnetionString) { }


		public virtual DbSet<Appointment> Appointments { get; set; }
		public virtual DbSet<Department> Departments { get; set; }
		public DbSet<Diagnosis> Diagnosiss { get; set; }
		public virtual DbSet<Doctor> Doctors { get; set; }
		public DbSet<Employee> Employees { get; set; }
		public DbSet<Examination> Examinations { get; set; }
		public DbSet<Feedback> Feedbacks { get; set; }
		public DbSet<MedicalHistory> MedicalHistorys { get; set; }
		public virtual DbSet<Medication> Medications { get; set; }
		public virtual DbSet<Patient> Patients { get; set; }
		public DbSet<Procedure> Procedures { get; set; }
		public DbSet<Schedule> Schedules { get; set; }
		public DbSet<Treatment> Treatments { get; set; }
		public virtual DbSet<User> Users { get; set; }
		public DbSet<UserClaim> UserClaims { get; set; }
		public DbSet<Vaccination> Vaccinations { get; set; }
		public DbSet<WarningLabel> WarningLabels { get; set; }
		public DbSet<MobileAuthentificator> MobileAuthentificators { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}

		public DbSet<BloodGroup> BloodGroups { get; set; }
	}
}