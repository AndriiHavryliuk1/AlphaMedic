using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace RestWebApi.Models
{
    public class RestWebApiContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public RestWebApiContext() : base("name=RestWebApiContext")
        {
        }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {           
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Procedure> Procedures { get; set; }


        public System.Data.Entity.DbSet<RestWebApi.Models.Patient> Patients { get; set; }

        public System.Data.Entity.DbSet<RestWebApi.Models.Appointment> Appointments { get; set; }
    }
}
