using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rest.Controllers;
using Rest.Dtos;
using Rest.Models;
using Rest.Models.AlphaMedicContext;
using RestTests.ModelsMock;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace Rest.Controllers.Tests
{
    [TestClass()]
    public class PatientsControllerTests
    {
        #region factory
        private static PatientsController GetPatientsControllerForGetPatientsMethod(
            IEnumerable<User> dbUsers = null,
            IEnumerable<Doctor> dbDoctors = null,
            IEnumerable<Patient> dbPatients = null,
            IEnumerable<Appointment> dbAppointments = null
            )
        {
            var mockContext = new Mock<AlphaMedicContext>();

            if (dbUsers != null)
            {
                var mockSet = new Mock<DbSet<User>>();
                mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(dbUsers.AsQueryable().Provider);
                mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(dbUsers.AsQueryable().Expression);
                mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(dbUsers.AsQueryable().ElementType);
                mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => dbUsers.AsQueryable().GetEnumerator());
                mockContext.Setup(c => c.Users).Returns(mockSet.Object);
            }

            if (dbPatients != null)
            {
                var mockSet = new Mock<DbSet<Patient>>();
                mockSet.As<IQueryable<Patient>>().Setup(m => m.Provider).Returns(dbPatients.AsQueryable().Provider);
                mockSet.As<IQueryable<Patient>>().Setup(m => m.Expression).Returns(dbPatients.AsQueryable().Expression);
                mockSet.As<IQueryable<Patient>>().Setup(m => m.ElementType).Returns(dbPatients.AsQueryable().ElementType);
                mockSet.As<IQueryable<Patient>>().Setup(m => m.GetEnumerator()).Returns(() => dbPatients.AsQueryable().GetEnumerator());
                mockContext.Setup(c => c.Patients).Returns(mockSet.Object);
            }

            if (dbDoctors != null)
            {
                var mockSet = new Mock<DbSet<Doctor>>();
                mockSet.As<IQueryable<Doctor>>().Setup(m => m.Provider).Returns(dbDoctors.AsQueryable().Provider);
                mockSet.As<IQueryable<Doctor>>().Setup(m => m.Expression).Returns(dbDoctors.AsQueryable().Expression);
                mockSet.As<IQueryable<Doctor>>().Setup(m => m.ElementType).Returns(dbDoctors.AsQueryable().ElementType);
                mockSet.As<IQueryable<Doctor>>().Setup(m => m.GetEnumerator()).Returns(() => dbDoctors.AsQueryable().GetEnumerator());
                mockContext.Setup(c => c.Doctors).Returns(mockSet.Object);
            }

            if (dbAppointments != null)
            {
                var mockSet = new Mock<DbSet<Appointment>>();
                mockSet.As<IQueryable<Appointment>>().Setup(m => m.Provider).Returns(dbAppointments.AsQueryable().Provider);
                mockSet.As<IQueryable<Appointment>>().Setup(m => m.Expression).Returns(dbAppointments.AsQueryable().Expression);
                mockSet.As<IQueryable<Appointment>>().Setup(m => m.ElementType).Returns(dbAppointments.AsQueryable().ElementType);
                mockSet.As<IQueryable<Appointment>>().Setup(m => m.GetEnumerator()).Returns(() => dbAppointments.AsQueryable().GetEnumerator());
                mockContext.Setup(c => c.Appointments).Returns(mockSet.Object);
            }

            var service = new PatientsController(mockContext.Object);
            return service;
        }
        #endregion
        /*
        public int? MedicalHistoryId { get; set; }
        public int? BloodGroupId { get; set; }


        public virtual MedicalHistory MedicalHistory { get; set; }
        public virtual BloodGroup BloodGroup { get; set; }
        public virtual ICollection<Appointment> Appointments { get; private set; }
        public virtual ICollection<Feedback> Feedbacks { get; private set; }
        */




        //[Route("")]
        //[Authorize(Roles = Roles.Administrator + "," + Roles.AllDoctors + "," + Roles.Receptionist)]
        //public IHttpActionResult GetPatients(int page = 1, int itemsPerPage = 15,
        //    string sortBy = "UserId", bool reverse = false,
        //    string search = null, int? doctor = null,
        //    int? department = null)
        //{
        //    try
        //    {
        //        var currentUser = db.Users.FirstOrDefault(x => x.Email == this.User.Identity.Name);
        //        var maybeDoctor = db.Doctors.FirstOrDefault(x => x.UserId == currentUser.UserId);

        //        if (maybeDoctor != null)
        //        {
        //            if (Tools.AnyRole(this.User, Roles.DoctorRoles)
        //                && maybeDoctor.DoctorType != DoctorType.HospitalDean &&
        //                  (maybeDoctor.DoctorType == DoctorType.HeadDepartment && maybeDoctor.DepartmentId != department)
        //                && (doctor != currentUser.UserId || doctor == null))
        //            {
        //                return Content(HttpStatusCode.Forbidden, Messages.AccsesDenied);
        //            }
        //        }
        //    }
        //    catch (ArgumentNullException)
        //    {
        //        return InternalServerError();
        //    }

        //    var users = db.Patients.Include(c => c.Appointments).
        //    Select(x => new
        //    {
        //        x.UserId,
        //        x.Name,
        //        x.Surname,
        //        URLImage = Constants.ThisServer + x.URLImage,

        //        Procedure = x.Appointments.Select(
        //        p => new
        //        {
        //            ProcedureId = p.AppointmentId,
        //            ProcedureName = p.Procedure.Name ?? string.Empty,
        //            Doctor = (p.Doctor != null ?
        //            new
        //            {
        //                DoctorFullName = p.Doctor.Name + " " + p.Doctor.Surname,
        //                DoctorId = p.Doctor.UserId,
        //                DepartmentId = p.Doctor.DepartmentId
        //            }
        //            : null)
        //        }
        //        ).FirstOrDefault()
        //    }
        //    ).AsQueryable();

        //    if (department != null)
        //    {
        //        users = users.Where(x => x.Procedure.Doctor.DepartmentId == department);
        //    }

        //    if (doctor != null)
        //    {
        //        users = users.Where(x => x.Procedure.Doctor.DoctorId == doctor);
        //    }

        //    // searching
        //    if (!string.IsNullOrWhiteSpace(search))
        //    {
        //        search = search.ToLower();
        //        users = users.Where(x =>
        //            (x.Name + x.Surname).ToLower().Contains(search.Replace(" ", "")) ||
        //            (x.Surname + x.Name).ToLower().Contains(search.Replace(" ", "")));
        //    }

        //    // sorting (done with the System.Linq.Dynamic library available on NuGet)
        //    users = users.OrderBy(sortBy + (reverse ? "descending" : ""));

        //    // paging
        //    var usersPaged = users.Skip((page - 1) * itemsPerPage).Take(itemsPerPage);

        //    // json result
        //    var json = new
        //    {
        //        count = users.Count(),
        //        data = usersPaged
        //    };

        //    return Ok(json);

        //}

        [TestMethod]
        public void GetPatients_DBUsersNotSet_ThrowsArgumentNullExceptionAndSendServerInternalError_Test()
        {
            var service = GetPatientsControllerForGetPatientsMethod();
            var actionResult = service.GetPatients();
            Assert.IsTrue(actionResult is InternalServerErrorResult);
        }

        [TestMethod]
        public void GetPatients_DBUsersIsEmpty_ThrowsArgumentNullExceptionAndSendServerInternalError_Test()
        {
            var service = GetPatientsControllerForGetPatientsMethod(
                dbUsers: Enumerable.Empty<User>()
                );
            var actionResult = service.GetPatients();
            Assert.IsTrue(actionResult is InternalServerErrorResult);
        }

        //[TestMethod]
        //public void GetPatients_CurrentUserIsNotFound_ThrowsArgumentNullExceptionAndSendServerInternalError_Test()
        //{
        //    var service = GetPatientsControllerForGetPatientsMethod(
        //        dbUsers: new[]
        //        {
        //            new UserMock()
        //            {
        //                UserId = 1,

        //            }
        //        }
        //        );
        //   // var actionResult = service.GetPatients(;
        //    Assert.IsTrue(actionResult is InternalServerErrorResult);
        //}



        //[TestMethod]
        //public void GetPatients_DBDoctorsNotSet_ThrowsArgumentNullExceptionAndSendServerInternalError_Test()
        //{
        //    var service = GetPatientsControllerForGetPatientsMethod(
        //   //     db
        //        );
        //    var actionResult = service.GetPatients();
        //    Assert.IsTrue(actionResult is InternalServerErrorResult);
        //}


        [TestMethod()]
        public void GetPatientsByDoctorIdTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetPatientAppointmentsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetPatientTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAllPatientsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PutPatientTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PutPatientTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PostPatientTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeletePatientTest()
        {
            Assert.Fail();
        }
    }
}