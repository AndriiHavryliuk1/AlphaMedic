using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Rest.Models.AlphaMedicContext
{
    public class AlphaMedicContext : DbContext
    {
        public AlphaMedicContext() : base("Data Source=bootcamp;Initial Catalog=ALPHA_AlphaMedic;Integrated Security=True") { }


        //public DbSet<Administrator> Administrators { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public DbSet<Diagnosis> Diagnosiss { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Examination> Examinations { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        //public DbSet<HeadDepartment> HeadDepartments { get; set; }
        //public DbSet<HospitalDean> HospitalDeans { get; set; }
        public DbSet<MedicalHistory> MedicalHistorys { get; set; }
        public virtual DbSet<Medication> Medications { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public DbSet<Procedure> Procedures { get; set; }
        //public DbSet<Reseptionist> Reseptionists { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<Vaccionation> Vaccionations { get; set; }
        public DbSet<WarningLabel> WarningLabels { get; set; }
        public DbSet<MobileAuthentificator> MobileAuthentificators { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>()
        //                .Map<Employee>(m => m.Requires("UserType").HasValue("UE"))
        //                .Map<Patient>(m => m.Requires("UserType").HasValue("UP"));

        //    modelBuilder.Entity<Employee>()
        //    .Map<Doctor>(m => m.Requires("DoctorType").HasValue("ED"));

        //}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<BloodGroup> BloodGroups { get; set; }
    }
}