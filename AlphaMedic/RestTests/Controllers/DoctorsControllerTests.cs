using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rest.Controllers;
using Rest.Dtos;
using Rest.Models;
using Rest.Models.AlphaMedicContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace Rest.Controllers.Tests
{
    [TestClass()]
    public class DoctorsControllerTests
    {

        private IEnumerable<Doctor> Doctors
        {
            get
            {
                return new List<Doctor>()  {
                new Doctor()
                {
                    Name = "Marry",
                    Surname = "Many",
                    Degree = "kjhgvbkjgb",
                    Education = "kjhugkjgbh",
                    DoctorType = 0,
                    Department = new Department() { Name = "Syrgery"},
                    Active = true,
                    UserId = 1,
                    DepartmentId = 2
                },
                new Doctor()
                {
                    Name = "Jekky",
                    Surname = "Chan",
                    Degree = "kjhgvbkjgb",
                    Education = "kjhugkjgbh",
                    DoctorType = 0,
                    Department = new Department() { Name = "Syrgery"},
                    Active = true,
                    UserId = 2,
                    DepartmentId = 1
                },
                new Doctor()
                {
                    Name = "Pitep",
                    Surname = "Peters",
                    Degree = "kjhgvbkjgb",
                    Education = "kjhugkjgbh",
                    DoctorType = 0,
                    Department = new Department() { Name = "Syrgery"},
                    Active = false,
                    UserId = 6,
                    DepartmentId = 3
                },
                new Doctor()
                {
                    Name = "Jerry",
                    Surname = "Kan",
                    Degree = "kjhgvbkjgb",
                    Education = "kjhugkjgbh",
                    DoctorType = 0,
                    Department = new Department() { Name = "Syrgery"},
                    Active = true,
                    UserId = 3,
                    DepartmentId = 1
                },
                new Doctor()
                {
                    Name = "Jeny",
                    Surname = "Simon",
                    Degree = "kjhgvbkjgb",
                    Education = "kjhugkjgbh",
                    DoctorType = 0,
                    Department = new Department() { Name = "Syrgery"},
                    Active = false,
                    UserId = 4,
                    DepartmentId = 1
                }
            };
            }
        }

        private static DoctorsController GetDoctorsController()
        {
            var emptyDocData = Enumerable.Empty<Doctor>().AsQueryable();
            var emptyEmpData = Enumerable.Empty<Employee>().AsQueryable();
            var emptyUserData = Enumerable.Empty<User>().AsQueryable();
            var mockDocSet = new Mock<DbSet<Doctor>>();
            var mockEmpSet = new Mock<DbSet<Employee>>();
            var mockUserSet = new Mock<DbSet<User>>();
            var mockContext = new Mock<AlphaMedicContext>();
            mockDocSet.As<IQueryable<Doctor>>().Setup(m => m.Provider).Returns(emptyDocData.Provider);
            mockDocSet.As<IQueryable<Doctor>>().Setup(m => m.Expression).Returns(emptyDocData.Expression);
            mockDocSet.As<IQueryable<Doctor>>().Setup(m => m.ElementType).Returns(emptyDocData.ElementType);
            mockDocSet.As<IQueryable<Doctor>>().Setup(m => m.GetEnumerator()).Returns(() => emptyDocData.GetEnumerator());
            mockContext.Setup(c => c.Doctors).Returns(mockDocSet.Object);

            //mockEmpSet.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(emptyEmpData.Provider);
            //mockEmpSet.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(emptyEmpData.Expression);
            //mockEmpSet.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(emptyEmpData.ElementType);
            //mockEmpSet.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(() => emptyEmpData.GetEnumerator());
            //mockContext.Setup(c => c.Employees).Returns(mockEmpSet.Object);

            //mockUserSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(emptyUserData.Provider);
            //mockUserSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(emptyUserData.Expression);
            //mockUserSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(emptyUserData.ElementType);
            //mockUserSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => emptyUserData.GetEnumerator());
            //mockContext.Setup(c => c.Users).Returns(mockUserSet.Object);
            var service = new DoctorsController(mockContext.Object);
            return service;
        }

        private static DoctorsController GetDoctorsControllerWithData(
            IEnumerable<Doctor> doctorsData
            //IEnumerable<Employee> employerData,
            //IEnumerable<User> userData
            )
        {
            var docData = doctorsData.AsQueryable();
            //var empData = employerData.AsQueryable();
            //var usersData = userData.AsQueryable();
            var mockDocSet = new Mock<DbSet<Doctor>>();
            var mockEmpSet = new Mock<DbSet<Employee>>();
            var mockUserSet = new Mock<DbSet<User>>();
            var mockContext = new Mock<AlphaMedicContext>();
            mockDocSet.As<IQueryable<Doctor>>().Setup(m => m.Provider).Returns(docData.Provider);
            mockDocSet.As<IQueryable<Doctor>>().Setup(m => m.Expression).Returns(docData.Expression);
            mockDocSet.As<IQueryable<Doctor>>().Setup(m => m.ElementType).Returns(docData.ElementType);
            mockDocSet.As<IQueryable<Doctor>>().Setup(m => m.GetEnumerator()).Returns(() => docData.GetEnumerator());
            mockContext.Setup(c => c.Doctors).Returns(mockDocSet.Object);

            //mockEmpSet.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(empData.Provider);
            //mockEmpSet.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(empData.Expression);
            //mockEmpSet.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(empData.ElementType);
            //mockEmpSet.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(() => empData.GetEnumerator());
            //mockContext.Setup(c => c.Employees).Returns(mockEmpSet.Object);

            //mockUserSet.As<IQueryable<User>>().Setup((IQueryable<User> m) => m.Provider).Returns(usersData.Provider);
            //mockUserSet.As<IQueryable<User>>().Setup((IQueryable<User> m) => m.Expression).Returns(usersData.Expression);
            //mockUserSet.As<IQueryable<User>>().Setup((IQueryable<User> m) => m.ElementType).Returns(usersData.ElementType);
            //mockUserSet.As<IQueryable<User>>().Setup((IQueryable<User> m) => m.GetEnumerator()).Returns(() => usersData.GetEnumerator());
            //mockContext.Setup(c => c.Users).Returns(mockUserSet.Object);
            var service = new DoctorsController(mockContext.Object);
            return service;
        }



        [TestMethod()]
        public void GetDoctors_DoctorsListByDepartmentFilterTest()
        {
            var expectedArray = new []{ 2,3,4 };        
            var service = GetDoctorsControllerWithData(Doctors);
            var a = service.GetUsers(1, 15, "DoctorId", false, null, 1, null);
            
            var httpResult = a as OkNegotiatedContentResult<JsonDto>;
            var resArray = httpResult.Content.data.Cast<DoctorDto>().Select(x => x.UserId).ToArray();
            Assert.IsTrue(resArray.SequenceEqual(expectedArray));

        }



        [TestMethod()]
        public void GetDoctors_DoctorsListBySearchFilterTest()
        {
            
            var expectedArray = new[] { 2, 3, 4 };
            var service = GetDoctorsControllerWithData(Doctors);
            var a = service.GetUsers(1, 15, "DoctorId", false, "Je", null, null);

            var httpResult = a as OkNegotiatedContentResult<JsonDto>;
            var resArray = httpResult.Content.data.Cast<DoctorDto>().Select(x => x.UserId).ToArray();
            Assert.IsTrue(resArray.SequenceEqual(expectedArray));

        }

        [TestMethod()]
        public void GetDoctors_DoctorsListByPage_One_and_three_elementTest()
        {
            var expectedArray = new[] { 1, 2, 3 };
            var service = GetDoctorsControllerWithData(Doctors);
            var a = service.GetUsers(1, 3, "DoctorId", false, null, null, null);

            var httpResult = a as OkNegotiatedContentResult<JsonDto>;
            var resArray = httpResult.Content.data.Cast<DoctorDto>().Select(x => x.UserId).ToArray();
            Assert.IsTrue(resArray.SequenceEqual(expectedArray));

        }

        [TestMethod()]
        public void GetDoctors_DoctorsListBySortDoctorId_reverseTest()
        {
            var expectedArray = new[] { 6, 4, 3, 2, 1};
            var service = GetDoctorsControllerWithData(Doctors);
            var a = service.GetUsers(1, 100, "DoctorId", true, null, null, null);

            var httpResult = a as OkNegotiatedContentResult<JsonDto>;
            var resArray = httpResult.Content.data.Cast<DoctorDto>().Select(x => x.UserId).ToArray();
            Assert.IsTrue(resArray.SequenceEqual(expectedArray));
        }

        [TestMethod()]
        public void GetDoctors_DoctorsListByActiveFilterTest()
        {
            var expectedArray = new[] { 4, 6 };
            var service = GetDoctorsControllerWithData(Doctors);
            var a = service.GetUsers(1, 100, "DoctorId", false, null, null, false);

            var httpResult = a as OkNegotiatedContentResult<JsonDto>;
            var resArray = httpResult.Content.data.Cast<DoctorDto>().Select(x => x.UserId).ToArray();
            Assert.IsTrue(resArray.SequenceEqual(expectedArray));
        }
    }
}