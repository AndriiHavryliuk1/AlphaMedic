using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rest.Dtos;
using Rest.Models;
using Rest.Models.AlphaMedicContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http.Results;


namespace Rest.Controllers.Tests
{
    [TestClass()]
    public class DepartmentsControllerTests
    {
        private static DepartmentsController GetDepartmentsControllerWithInstalledDoctors(
             IEnumerable<Department> departmentsData,
             IEnumerable<Doctor> doctorsData)
        {
            var dpData = departmentsData.AsQueryable();
            var dcData = doctorsData.AsQueryable();

            var mockDepSet = new Mock<DbSet<Department>>();
            var mockDocSet = new Mock<DbSet<Doctor>>();
            var mockContext = new Mock<AlphaMedicContext>();
            mockDepSet.As<IQueryable<Department>>().Setup(m => m.Provider).Returns(dpData.Provider);
            mockDepSet.As<IQueryable<Department>>().Setup(m => m.Expression).Returns(dpData.Expression);
            mockDepSet.As<IQueryable<Department>>().Setup(m => m.ElementType).Returns(dpData.ElementType);
            mockDepSet.As<IQueryable<Department>>().Setup(m => m.GetEnumerator()).Returns(() => dpData.GetEnumerator());

            mockDocSet.As<IQueryable<Doctor>>().Setup(m => m.Provider).Returns(dcData.Provider);
            mockDocSet.As<IQueryable<Doctor>>().Setup(m => m.Expression).Returns(dcData.Expression);
            mockDocSet.As<IQueryable<Doctor>>().Setup(m => m.ElementType).Returns(dcData.ElementType);
            mockDocSet.As<IQueryable<Doctor>>().Setup(m => m.GetEnumerator()).Returns(() => dcData.GetEnumerator());

            mockContext.Setup(c => c.Departments).Returns(mockDepSet.Object);
            mockContext.Setup(d => d.Doctors).Returns(mockDocSet.Object);
            var service = new DepartmentsController(mockContext.Object);
            return service;
        }




        private static DepartmentsController GetDepartmentsController()
        {
            var emptyData = Enumerable.Empty<Department>().AsQueryable();
            var mockSet = new Mock<DbSet<Department>>();
            var mockContext = new Mock<AlphaMedicContext>();
            mockSet.As<IQueryable<Department>>().Setup(m => m.Provider).Returns(emptyData.Provider);
            mockSet.As<IQueryable<Department>>().Setup(m => m.Expression).Returns(emptyData.Expression);
            mockSet.As<IQueryable<Department>>().Setup(m => m.ElementType).Returns(emptyData.ElementType);
            mockSet.As<IQueryable<Department>>().Setup(m => m.GetEnumerator()).Returns(() => emptyData.GetEnumerator());
            mockContext.Setup(c => c.Departments).Returns(mockSet.Object);
            var service = new DepartmentsController(mockContext.Object);
            return service;
        }

        private static DepartmentsController GetDepartmentsControllerWithData(IEnumerable<Department> data)
        {
            var newdata = data.AsQueryable();
            var mockSet = new Mock<DbSet<Department>>();
            mockSet.As<IQueryable<Department>>().Setup(m => m.Provider).Returns(newdata.Provider);
            mockSet.As<IQueryable<Department>>().Setup(m => m.Expression).Returns(newdata.Expression);
            mockSet.As<IQueryable<Department>>().Setup(m => m.ElementType).Returns(newdata.ElementType);
            mockSet.As<IQueryable<Department>>().Setup(m => m.GetEnumerator()).Returns(() => newdata.GetEnumerator());
            var mockContext = new Mock<AlphaMedicContext>();
            mockContext.Setup(c => c.Departments).Returns(mockSet.Object);
            var service = new DepartmentsController(mockContext.Object);
            return service;
        }



        #region GetDepartmentsTests
        [TestMethod()]
        public void GetDepartments_DepartmentsListIsEmpty_CountIsZero_Test()
        {
            var expected = 0;
            var service = GetDepartmentsController();
            var medications = service.GetDepartments().ToArray();
            var actual = medications.Length;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetDepartments_DepartmentsListWithOneElement_CountIsOne_Test()
        {
            var department = new Department
            {
                DepartmentId = 1,
                Description = "this is department description",
                Name = "Name of Department"
            };
            var expected = 1;
            var service = GetDepartmentsControllerWithData(new[] { department });
            var medications = service.GetDepartments().ToArray();
            var actual = medications.Length;
            Assert.AreEqual(expected, actual);
        }



        [TestMethod()]
        public void GetDepartments_GettingDepartments_IdIsEqual_Test()
        {
            var department = new Department
            {
                DepartmentId = 1,
                Description = "this is department description",
                Name = "Name of Department"
            };
            var expected = 1;
            var service = GetDepartmentsControllerWithData(new[] { department });
            var medications = service.GetDepartments().ToArray();
            var actual = medications[0].DepartmentId;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetDepartments_GettingDepartments_NameIsEqual_Test()
        {
            var department = new Department
            {
                DepartmentId = 1,
                Description = "this is department description",
                Name = "Name of Department"
            };
            var expected = "Name of Department";
            var service = GetDepartmentsControllerWithData(new[] { department });
            var medications = service.GetDepartments().ToArray();
            var actual = medications[0].Name;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetDepartments_GettingDepartments_DescriptionIsEqual_Test()
        {
            var department = new Department
            {
                DepartmentId = 1,
                Description = "this is department description",
                Name = "Name of Department"
            };
            var expected = "this is department description";
            var service = GetDepartmentsControllerWithData(new[] { department });
            var medications = service.GetDepartments().ToArray();
            var actual = medications[0].Description;
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region GetDepartmentTests

        [TestMethod]
        public void GetDepartment_ListIsEmpty_ReturnsNotFound_Test()
        {
            var service = GetDepartmentsController();
            var actionResult = service.GetDepartment(1);
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetDepartment_DepartmentFromCurrentIdIsNotExist_ReturnsNotFound_Test()
        {
            var department = new Department
            {
                DepartmentId = 1,
                Description = "this is department description",
                Name = "Name of Department"
            };
            var service = GetDepartmentsControllerWithData(new[] { department });
            var actionResult = service.GetDepartment(2);
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod()]
        public void GetDepartment_FeedbacksIsNotSet_ReturnsEmptyArray_Test()
        {
            var expected = 0;
            var department = new Department
            {
                DepartmentId = 1,
            };
            var service = GetDepartmentsControllerWithData(new[] { department });
            var actionResult = service.GetDepartment(1) as OkNegotiatedContentResult<DepartmentFullDto>;
            var actual = actionResult.Content.Feedbacks.Length;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetDepartment_DoctorsIsNotSet_HeadDepartmentIsNull_Test()
        {
            var department = new Department
            {
                DepartmentId = 1,
                Description = "this is department description",
                Name = "Name of Department",
            };
            var service = GetDepartmentsControllerWithData(new[] { department });
            var actionResult = service.GetDepartment(1) as OkNegotiatedContentResult<DepartmentFullDto>;
            Assert.IsNull(actionResult.Content.HeadDepartment);
        }

        [TestMethod]
        public void GetDepartment_HeadDepartmentIsNotSet_HeadDepartmentIsNull_Test()
        {
            var department = new Department
            {
                DepartmentId = 1,
                Description = "this is department description",
                Name = "Name of Department",
                Doctors = new[]
                {
                    new Doctor
                    {
                        UserId = 1,
                        Name = "Doctor Name",
                        Surname = "Doctor Surname",
                        EmployeeType = EmployeeType.Doctor,
                        DoctorType= DoctorType.Doctor
                    }
                }
            };
            var service = GetDepartmentsControllerWithData(new[] { department });
            var actionResult = service.GetDepartment(1) as OkNegotiatedContentResult<DepartmentFullDto>;
            Assert.IsNull(actionResult.Content.HeadDepartment);
        }

        [TestMethod]
        public void GetDepartment_FeedBackIsSet_ReturnsOneFeedback_Test()
        {
            var department = new Department
            {
                DepartmentId = 1,
                Feedbacks = new[]
                {
                    new Feedback
                    {
                        FeedbackId=1
                    }
                }
            };
            var expected = 1;
            var service = GetDepartmentsControllerWithData(new[] { department });
            var actionResult = service.GetDepartment(1) as OkNegotiatedContentResult<DepartmentFullDto>;
            var actual = actionResult.Content.Feedbacks.Length;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetDepartment_FeedBackIsSet_FeedbackIdIsCorrect_Test()
        {

            var department = new Department
            {
                DepartmentId = 1,
                Feedbacks = new[]
                {
                    new Feedback
                    {
                        FeedbackId=1
                    }
                }
            };
            var expected = 1;
            var service = GetDepartmentsControllerWithData(new[] { department });
            var actionResult = service.GetDepartment(1) as OkNegotiatedContentResult<DepartmentFullDto>;
            var actual = actionResult.Content.Feedbacks[0].FeedbackId;
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void GetDepartment_FeedBackIsSet_FeedbackDateIsCorrect_Test()
        {

            var department = new Department
            {
                DepartmentId = 1,
                Feedbacks = new[]
                    {
                    new Feedback
                    {
                        FeedbackId=1,
                        Date = new DateTime(1488,2,28)
                    }
                }
            };
            var expected = new DateTime(1488, 2, 28);
            var service = GetDepartmentsControllerWithData(new[] { department });
            var actionResult = service.GetDepartment(1) as OkNegotiatedContentResult<DepartmentFullDto>;
            var actual = actionResult.Content.Feedbacks[0].Date;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetDepartment_FeedBackIsSet_FeedbackDepartmentIdIsCorrect_Test()
        {
            var department = new Department
            {
                DepartmentId = 1,
                Feedbacks = new[]
                        {
                    new Feedback
                    {
                        FeedbackId=1,
                        DepartmentId = 2
                    }
                }
            };
            var expected = 2;
            var service = GetDepartmentsControllerWithData(new[] { department });
            var actionResult = service.GetDepartment(1) as OkNegotiatedContentResult<DepartmentFullDto>;
            var actual = actionResult.Content.Feedbacks[0].DepartmentId;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetDepartment_FeedBackIsSet_FeedbackDescriptionIsCorrect_Test()
        {
            var department = new Department
            {
                DepartmentId = 1,
                Feedbacks = new[]
                    {
                    new Feedback
                    {
                        FeedbackId=1,
                        Description = "This is Department Description"
                    }
                }
            };
            var expected = "This is Department Description";
            var service = GetDepartmentsControllerWithData(new[] { department });
            var actionResult = service.GetDepartment(1) as OkNegotiatedContentResult<DepartmentFullDto>;
            var actual = actionResult.Content.Feedbacks[0].Description;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetDepartment_PatientInFeedbackIsNull_PatientFullNameIsAnonymous_Test()
        {
            var department = new Department
            {
                DepartmentId = 1,
                Feedbacks = new[]
                    {
                    new Feedback
                    {
                        FeedbackId=1,
                        Patient = null
                    }
                }
            };
            var expected = "Anonymous";
            var service = GetDepartmentsControllerWithData(new[] { department });
            var actionResult = service.GetDepartment(1) as OkNegotiatedContentResult<DepartmentFullDto>;
            var actual = actionResult.Content.Feedbacks[0].PatientFullName;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetDepartment_PatientInFeedbackIsІуе_PatientFullNameCorrect_Test()
        {
            var department = new Department
            {
                DepartmentId = 1,
                Feedbacks = new[]
                    {
                    new Feedback
                    {
                        FeedbackId=1,
                        Patient = new Patient
                        {
                            UserId = 1,
                            Name = "Name",
                            Surname = "Surname"
                        }
                    }
                }
            };
            var expected = "Name Surname";
            var service = GetDepartmentsControllerWithData(new[] { department });
            var actionResult = service.GetDepartment(1) as OkNegotiatedContentResult<DepartmentFullDto>;
            var actual = actionResult.Content.Feedbacks[0].PatientFullName;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetDepartment_HeadDepartmentIsSet_HeadDepartmentUserIdCorrect_Test()
        {
            var department = new Department
            {
                DepartmentId = 1,
                Description = "this is department description",
                Name = "Name of Department",
                Doctors = new[]
                {
                    new Doctor
                    {
                        UserId = 1,
                        Name = "Doctor Name",
                        Surname = "Doctor Surname",
                        EmployeeType = EmployeeType.Doctor,
                        DoctorType= DoctorType.HeadDepartment
                    }
                }
            };
            var expected = 1;
            var service = GetDepartmentsControllerWithData(new[] { department });
            var actionResult = service.GetDepartment(1) as OkNegotiatedContentResult<DepartmentFullDto>;
            var actual = actionResult.Content.HeadDepartment.UserId;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetDepartment_HeadDepartmentIsSet_HeadDepartmentNameCorrect_Test()
        {
            var department = new Department
            {
                DepartmentId = 1,
                Description = "this is department description",
                Name = "Name of Department",
                Doctors = new[]
                {
                    new Doctor
                    {
                        UserId = 1,
                        Name = "Doctor Name",
                        Surname = "Doctor Surname",
                        EmployeeType = EmployeeType.Doctor,
                        DoctorType= DoctorType.HeadDepartment
                    }
                }
            };
            var expected = "Doctor Name";
            var service = GetDepartmentsControllerWithData(new[] { department });
            var actionResult = service.GetDepartment(1) as OkNegotiatedContentResult<DepartmentFullDto>;
            var actual = actionResult.Content.HeadDepartment.Name;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetDepartment_HeadDepartmentIsSet_HeadDepartmentSurnameCorrect_Test()
        {
            var department = new Department
            {
                DepartmentId = 1,
                Description = "this is department description",
                Name = "Name of Department",
                Doctors = new[]
                {
                    new Doctor
                    {
                        UserId = 1,
                        Name = "Doctor Name",
                        Surname = "Doctor Surname",
                        EmployeeType = EmployeeType.Doctor,
                        DoctorType= DoctorType.HeadDepartment
                    }
                }
            };
            var expected = "Doctor Surname";
            var service = GetDepartmentsControllerWithData(new[] { department });
            var actionResult = service.GetDepartment(1) as OkNegotiatedContentResult<DepartmentFullDto>;
            var actual = actionResult.Content.HeadDepartment.Surname;
            Assert.AreEqual(expected, actual);
        }

        //Feedbacks = all == true ? feedbacks.ToArray() : feedbacks.Skip(Math.Max(0, department.Feedbacks.Count - 3)).ToArray(),
        //        FeedbacksCount = department.Feedbacks.Count,

        [TestMethod]
        public void GetDepartment_ParamAllIsTrue_ReturnAllFeedbacks_Test()
        {

            var all = true;
            var department = new Department
            {
                DepartmentId = 1,
                Feedbacks = new[]
                    {
                    new Feedback
                    {
                        FeedbackId=1,
                        Description = "This is Department Description"
                    },
                    new Feedback
                    {
                        FeedbackId=2,
                        Description = "This is Department Description"
                    },
                    new Feedback
                    {
                        FeedbackId=3,
                        Description = "This is Department Description"
                    },
                    new Feedback
                    {
                        FeedbackId=4,
                        Description = "This is Department Description"
                    },
                    new Feedback
                    {
                        FeedbackId=5,
                        Description = "This is Department Description"
                    },
                }
            };
            var expected = 5;
            var service = GetDepartmentsControllerWithData(new[] { department });
            var actionResult = service.GetDepartment(1, all) as OkNegotiatedContentResult<DepartmentFullDto>;
            var actual = actionResult.Content.Feedbacks.Length;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetDepartment_ParamAllIsFalse_ReturnOnlyLastThreeFeedbacks_Test()
        {

            var all = false;
            var department = new Department
            {
                DepartmentId = 1,
                Feedbacks = new[]
                    {
                    new Feedback
                    {
                        FeedbackId=1,
                        Description = "This is Department Description"
                    },
                    new Feedback
                    {
                        FeedbackId=2,
                        Description = "This is Department Description"
                    },
                    new Feedback
                    {
                        FeedbackId=3,
                        Description = "This is Department Description"
                    },
                    new Feedback
                    {
                        FeedbackId=4,
                        Description = "This is Department Description"
                    },
                    new Feedback
                    {
                        FeedbackId=5,
                        Description = "This is Department Description"
                    },
                }
            };
            var expected = new[] { 3, 4, 5 };
            var service = GetDepartmentsControllerWithData(new[] { department });
            var actionResult = service.GetDepartment(1, all) as OkNegotiatedContentResult<DepartmentFullDto>;
            var actual = actionResult.Content.Feedbacks.Select(x => x.FeedbackId).ToArray();
            Assert.IsTrue(actual.SequenceEqual(expected));
        }

        [TestMethod]
        public void GetDepartment_ParamAllIsTrue_CountOfFeedbacksIsCorrect_Test()
        {

            var all = true;
            var department = new Department
            {
                DepartmentId = 1,
                Feedbacks = new[]
                    {
                    new Feedback
                    {
                        FeedbackId=1,
                        Description = "This is Department Description"
                    },
                    new Feedback
                    {
                        FeedbackId=2,
                        Description = "This is Department Description"
                    },
                    new Feedback
                    {
                        FeedbackId=3,
                        Description = "This is Department Description"
                    },
                    new Feedback
                    {
                        FeedbackId=4,
                        Description = "This is Department Description"
                    },
                    new Feedback
                    {
                        FeedbackId=5,
                        Description = "This is Department Description"
                    },
                }
            };
            var expected = 5;
            var service = GetDepartmentsControllerWithData(new[] { department });
            var actionResult = service.GetDepartment(1, all) as OkNegotiatedContentResult<DepartmentFullDto>;
            var actual = actionResult.Content.FeedbacksCount;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetDepartment_ParamAllIsFalse_CountOfFeedbacksIsCorrect_Test()
        {

            var all = false;
            var department = new Department
            {
                DepartmentId = 1,
                Feedbacks = new[]
                    {
                    new Feedback
                    {
                        FeedbackId=1,
                        Description = "This is Department Description"
                    },
                    new Feedback
                    {
                        FeedbackId=2,
                        Description = "This is Department Description"
                    },
                    new Feedback
                    {
                        FeedbackId=3,
                        Description = "This is Department Description"
                    },
                    new Feedback
                    {
                        FeedbackId=4,
                        Description = "This is Department Description"
                    },
                    new Feedback
                    {
                        FeedbackId=5,
                        Description = "This is Department Description"
                    },
                }
            };
            var expected = 5;
            var service = GetDepartmentsControllerWithData(new[] { department });
            var actionResult = service.GetDepartment(1, all) as OkNegotiatedContentResult<DepartmentFullDto>;
            var actual = actionResult.Content.FeedbacksCount;
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region GetDeparmentDoctors
        [TestMethod()]
        public void GetDepartmentDoctors_DoctorsIsNotSet_ReturnsEmptyArray_Test()
        {
            var department = new Department
            {
                DepartmentId = 1           
            };
            var doctors = Enumerable.Empty<Doctor>();
            var expected = 0;
            var service = GetDepartmentsControllerWithInstalledDoctors(new[] { department }, doctors);
            var result = service.GetDepartmentDoctors(1);

            var actual = result.Count();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetDepartmentDoctors_DoctorsIsSet_CountOfArrayIsCorrect_Test()
        {
            var department = new Department
            {
                DepartmentId = 1,
                Name = "NameOfDepartment"

            };
            var doctors = new[]
                          {
                              new Doctor
                              {
                                  UserId = 1,                                  
                                  DepartmentId=1,
                                  Department = department,                                  
                                  Schedule = new Schedule
                                  {
                                      StartWorkingTime = new TimeSpan(10,0,0),
                                      FinishWorkingTime = new TimeSpan(11,0,0)
                                  }
                              },
                              new Doctor
                              {
                                  UserId = 2,                                  
                                  DepartmentId=1,
                                  Department = department,                                 
                                  Schedule = new Schedule
                                  {
                                      StartWorkingTime = new TimeSpan(10,0,0),
                                      FinishWorkingTime = new TimeSpan(11,0,0)
                                  }                                 
                              },
                              new Doctor
                              {
                                  UserId = 3,                                  
                                  DepartmentId=1,
                                  Department = department,
                                  
                                  Schedule = new Schedule
                                  {
                                      StartWorkingTime = new TimeSpan(10,0,0),
                                      FinishWorkingTime = new TimeSpan(11,0,0)
                                  }                                 
                              }
                          };
            var expected = 3;
            var service = GetDepartmentsControllerWithInstalledDoctors(new[] { department }, doctors);
            var result = service.GetDepartmentDoctors(1);
            var actual = result.Count();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetDepartmentDoctors_DoctorsIsSet_CountOfArrayIsCorrect2_Test()
        {
            var department = new Department
            {
                DepartmentId = 1,
                Name = "NameOfDepartment"
            };
            var doctors = new[]
                          {
                             new Doctor
                              {
                                  UserId = 1,
                                  Name = "Name",
                                  Surname = "Surname",
                                  DepartmentId=2,
                                  Department = department,
                                  Degree = "Degree",
                                  Education = "Eductaion",
                                  Schedule = new Schedule
                                  {
                                      StartWorkingTime = new TimeSpan(10,0,0),
                                      FinishWorkingTime = new TimeSpan(11,0,0)
                                  },
                                  URLImage = "URLImage",
                                  EmployeeType = EmployeeType.Doctor,
                                  DoctorType = DoctorType.Doctor

                              },
                              new Doctor
                              {
                                  UserId = 2,
                                  Name = "Name",
                                  Surname = "Surname",
                                  DepartmentId=1,
                                  Department = department,
                                  Degree = "Degree",
                                  Education = "Eductaion",
                                  Schedule = new Schedule
                                  {
                                      StartWorkingTime = new TimeSpan(10,0,0),
                                      FinishWorkingTime = new TimeSpan(11,0,0)
                                  },
                                  URLImage = "URLImage",
                                  EmployeeType = EmployeeType.Doctor,
                                  DoctorType = DoctorType.Doctor

                              },
                              new Doctor
                              {
                                  UserId = 3,
                                  Name = "Name",
                                  Surname = "Surname",
                                  DepartmentId=1,
                                  Department = department,
                                  Degree = "Degree",
                                  Education = "Eductaion",
                                  Schedule = new Schedule
                                  {
                                      StartWorkingTime = new TimeSpan(10,0,0),
                                      FinishWorkingTime = new TimeSpan(11,0,0)
                                  },
                                  URLImage = "URLImage",
                                  EmployeeType = EmployeeType.Doctor,
                                  DoctorType = DoctorType.Doctor

                              }
                          };
            var expected = 2;
            var service = GetDepartmentsControllerWithInstalledDoctors(new[] { department }, doctors);
            var result = service.GetDepartmentDoctors(1);
            var actual = result.Count();
            Assert.AreEqual(expected, actual);
        }

        #endregion
        //[TestMethod()]
        //public void PutDepartmentTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void PostDepartmentTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void PatchDepartmentTest1()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void DeleteDepartmentTest()
        //{
        //    Assert.Fail();
        //}
    }
}