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
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.Routing;

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
            IEnumerable<Appointment> dbAppointments = null,
            string identityRole = ""
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
                mockSet.Setup(m => m.Include("Appointments")).Returns(mockSet.Object);
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


                //mockContext.Setup(m => m.Include("Appointments")).Returns(mockPersonSet.Object);
            }
            var service = new PatientsController(mockContext.Object);
            service.User = new GenericPrincipal(
                new GenericIdentity(
                    "example@gmail.com",
                    "Passport"), new[] { identityRole });

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

        //var fakeHttpContext = new Mock<HttpContextBase>();
        //var fakeIdentity = new GenericIdentity("User");
        //var principal = new GenericPrincipal(fakeIdentity, null);

        //fakeHttpContext.Setup(t => t.User).Returns(principal);
        //var controllerContext = new Mock<ControllerContext>();
        //controllerContext.Setup(t => t.HttpContext).Returns(fakeHttpContext.Object);

        //_requestController = new RequestController();

        ////Set your controller ControllerContext with fake context
        //_requestController.ControllerContext = controllerContext.Object; 



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

        [TestMethod]
        public void GetPatients_CurrentUserIsNotFound_ThrowsArgumentNullExceptionAndSendServerInternalError_Test()
        {
            var service = GetPatientsControllerForGetPatientsMethod(
                dbUsers: new[]
                {
                    new UserMock()
                    {
                        UserId = 1,
                        Email = "example2@gmail.com"
                    }
                }
                );
            var actionResult = service.GetPatients();
            Assert.IsTrue(actionResult is InternalServerErrorResult);
        }

        [TestMethod]
        public void GetPatients_DBDoctorsIsNotSet_ThrowsArgumentNullExceptionAndSendServerInternalError_Test()
        {
            var service = GetPatientsControllerForGetPatientsMethod(
                dbUsers: new[]
                {
                    new UserMock()
                    {
                        UserId = 1,
                        Email = "example@gmail.com"
                    }
                }
                );
            var actionResult = service.GetPatients();
            Assert.IsTrue(actionResult is InternalServerErrorResult);
        }

        [TestMethod]
        public void GetPatients_DoctorIsExistButHaventDoctorType_ReturnAccessDeniedAndStatusCodeIsCorrect_Test()
        {

            var expected = HttpStatusCode.Forbidden;
            var service = GetPatientsControllerForGetPatientsMethod(
                dbUsers: new[]
                {
                    new UserMock()
                    {
                        UserId = 1,
                        Email = "example@gmail.com"
                    }
                },
                dbDoctors: new[]
                {
                    new Doctor()
                    {
                        UserId = 1,
                    }
                }
                );
            var actionResult = service.GetPatients() as NegotiatedContentResult<string>;
            var actual = actionResult.StatusCode;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetPatients_DoctorIsExistButHaventDoctorType_ReturnAccessDeniedAndMessageIsCorrect_Test()
        {

            var expected = Messages.AccsesDenied;
            var service = GetPatientsControllerForGetPatientsMethod(
                dbUsers: new[]
                {
                    new UserMock()
                    {
                        UserId = 1,
                        Email = "example@gmail.com"
                    }
                },
                dbDoctors: new[]
                {
                    new Doctor()
                    {
                        UserId = 1,
                    }
                }
                );
            var actionResult = service.GetPatients() as NegotiatedContentResult<string>;
            var actual = actionResult.Content;
            Assert.AreEqual(expected, actual);
        }



        [TestMethod]
        public void GetPatients_DoctorTypeIsDoctorButParamIsNotSet_ReturnForbidden_Test()
        {

            var expected = HttpStatusCode.Forbidden;
            var service = GetPatientsControllerForGetPatientsMethod(
                dbUsers: new[]
                {
                    new UserMock()
                    {
                        UserId = 1,
                        Email = "example@gmail.com"
                    }
                },
                dbDoctors: new[]
                {
                    new Doctor()
                    {
                        UserId = 1,
                        DoctorType = DoctorType.Doctor
                    }
                },
                identityRole: "Doctor"

                );
            var actionResult = service.GetPatients() as NegotiatedContentResult<string>;
            var actual = actionResult.StatusCode;
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void GetPatients_DoctorTypeIsDoctorParamIsNotMatch_ReturnForbidden_Test()
        {

            var expected = HttpStatusCode.Forbidden;
            var service = GetPatientsControllerForGetPatientsMethod(
                dbUsers: new[]
                {
                    new UserMock()
                    {
                        UserId = 1,
                        Email = "example@gmail.com"
                    }
                },
                dbDoctors: new[]
                {
                    new Doctor()
                    {
                        UserId = 1,
                        DoctorType = DoctorType.Doctor
                    }
                },
                identityRole: "Doctor"

                );
            var actionResult = service.GetPatients(
                doctor: 2
                ) as NegotiatedContentResult<string>;
            var actual = actionResult.StatusCode;
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void GetPatients_DoctorTypeIsDepartmentHeadParamNotMatch_ReturnForbidden_Test()
        {

            var expected = HttpStatusCode.Forbidden;
            var service = GetPatientsControllerForGetPatientsMethod(
                dbUsers: new[]
                {
                    new UserMock()
                    {
                        UserId = 1,
                        Email = "example@gmail.com"
                    }
                },
                dbDoctors: new[]
                {
                    new Doctor()
                    {
                        UserId = 1,
                        DoctorType = DoctorType.HeadDepartment,
                        DepartmentId = 1
                    }
                },
                identityRole: "HeadDepartment"

                );
            var actionResult = service.GetPatients(
                doctor: 1,
                department: 2
                ) as NegotiatedContentResult<string>;
            var actual = actionResult.StatusCode;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetPatients_DBPatientsNotSet_ThrowsArgumentNullExceptionAndSendServerInternalError_Test()
        {
            var service = GetPatientsControllerForGetPatientsMethod(
                dbUsers: new[]
                {
                    new UserMock()
                    {
                        UserId = 1,
                        Email = "example@gmail.com"
                    }
                },
                dbDoctors: new[]
                {
                    new Doctor()
                    {
                        UserId = 1,
                        DoctorType = DoctorType.Doctor,
                    }
                },
                identityRole: "Doctor"

                );
            var actionResult = service.GetPatients(
                doctor: 1
                );
            Assert.IsTrue(actionResult is InternalServerErrorResult);
        }

        [TestMethod]
        public void GetPatients_DBPatientsIsEmpty_ReturnEmptyArray_Test()
        {
            var expected = 0;
            var service = GetPatientsControllerForGetPatientsMethod(
                dbUsers: new[]
                {
                    new UserMock()
                    {
                        UserId = 1,
                        Email = "example@gmail.com"
                    }
                },
                dbDoctors: new[]
                {
                    new Doctor()
                    {
                        UserId = 1,
                        DoctorType = DoctorType.Doctor,
                    }
                },
                dbPatients: Enumerable.Empty<Patient>(),
                dbAppointments: Enumerable.Empty<Appointment>(),
                identityRole: "Doctor"
                );
            var actionResult = service.GetPatients(
                doctor: 1
                ) as OkNegotiatedContentResult<JsonDto>;
            var actual = actionResult.Content.data.Count;
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void GetPatients_DBPatientsIsEmpty_CountIsCorrect_Test()
        {
            var expected = 0;
            var service = GetPatientsControllerForGetPatientsMethod(
                dbUsers: new[]
                {
                    new UserMock()
                    {
                        UserId = 1,
                        Email = "example@gmail.com"
                    }
                },
                dbDoctors: new[]
                {
                    new Doctor()
                    {
                        UserId = 1,
                        DoctorType = DoctorType.Doctor,
                    }
                },
                dbPatients: Enumerable.Empty<Patient>(),
                identityRole: "Doctor"
                );
            var actionResult = service.GetPatients(
                doctor: 1
                ) as OkNegotiatedContentResult<JsonDto>;
            var actual = actionResult.Content.count;
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void GetPatients_DoctorIsHospitalDean_ReturnsAllPatients_Test()
        {
            var expected = 4;
            var service = GetPatientsControllerForGetPatientsMethod(
                dbUsers: new[]
                {
                    new UserMock()
                    {
                        UserId = 1,
                        Email = "example@gmail.com"
                    }
                },
                dbDoctors: new[]
                {
                    new Doctor()
                    {
                        UserId = 1,
                        DoctorType = DoctorType.HospitalDean,
                    }
                },
                dbPatients: new[] {
                    new Patient
                    {
                        UserId = 2,
                    },
                    new Patient
                    {
                        UserId = 3,
                    },
                    new Patient
                    {
                        UserId = 4,
                    },
                    new Patient
                    {
                        UserId = 5,
                    }

                },
                identityRole: "HospitalDean"
                );
            var actionResult = service.GetPatients(
                ) as OkNegotiatedContentResult<JsonDto>;
            var actual = actionResult.Content.count;
            Assert.AreEqual(expected, actual);

        }       
    }
}