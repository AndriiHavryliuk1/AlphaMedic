using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data.Entity;
using Rest.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using Rest.Dtos;
using Rest.Models.AlphaMedicContext;
using Rest.Models;

namespace Rest.Controllers.Tests
{

    [TestClass()]
    public class MedicationsControllerTests
    {
        private static MedicationsController GetMedicationsController()
        {
            var emptyData = Enumerable.Empty<Medication>().AsQueryable();
            var mockSet = new Mock<DbSet<Medication>>();
            var mockContext = new Mock<AlphaMedicContext>();
            mockSet.As<IQueryable<Medication>>().Setup(m => m.Provider).Returns(emptyData.Provider);
            mockSet.As<IQueryable<Medication>>().Setup(m => m.Expression).Returns(emptyData.Expression);
            mockSet.As<IQueryable<Medication>>().Setup(m => m.ElementType).Returns(emptyData.ElementType);
            mockSet.As<IQueryable<Medication>>().Setup(m => m.GetEnumerator()).Returns(() => emptyData.GetEnumerator());
            mockContext.Setup(c => c.Medications).Returns(mockSet.Object);
            var service = new MedicationsController(mockContext.Object);
            return service;
        }

        //private static MedicationsController GetMedicationsController()
        //{
        //    var mockSet = new Mock<DbSet<Medication>>();
        //    var mockContext = new Mock<AlphaMedicContext.AlphaMedicContext>();
        //    mockContext.Setup(c => c.Medications).Returns(mockSet.Object);
        //    var service = new MedicationsController(mockContext.Object);
        //    return service;
        //}

        private static MedicationsController GetMedicationsControllerWithData(IEnumerable<Medication> data)
        {
            var newdata = data.AsQueryable();
            var mockSet = new Mock<DbSet<Medication>>();
            mockSet.As<IQueryable<Medication>>().Setup(m => m.Provider).Returns(newdata.Provider);
            mockSet.As<IQueryable<Medication>>().Setup(m => m.Expression).Returns(newdata.Expression);
            mockSet.As<IQueryable<Medication>>().Setup(m => m.ElementType).Returns(newdata.ElementType);
            mockSet.As<IQueryable<Medication>>().Setup(m => m.GetEnumerator()).Returns(() => newdata.GetEnumerator());
            var mockContext = new Mock<AlphaMedicContext>();
            mockContext.Setup(c => c.Medications).Returns(mockSet.Object);
            var service = new MedicationsController(mockContext.Object);
            return service;
        }


        [TestMethod()]
        public void GetMedications_GetData_IdIsEqual_Test()
        {
            var medication = new Medication
            {
                MedicationId = 1,
                Description = "Drug1",
                Price = 20
            };
            var expected = 1;
            var service = GetMedicationsControllerWithData(new[] { medication });
            var medications = service.GetMedications().ToArray();
            var actual = medications[0].MedicationId;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetMedications_GetData_DescriptionIsEqual_Test()
        {
            var medication = new Medication
            {
                MedicationId = 1,
                Description = "Drug1",
                Price = 20
            };
            var expected = "Drug1";
            var service = GetMedicationsControllerWithData(new[] { medication });
            var medications = service.GetMedications().ToArray();
            var actual = medications[0].Description;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetMedications_GetData_PriceIsEqual_Test()
        {
            var medication = new Medication
            {
                MedicationId = 1,
                Description = "Drug1",
                Price = 20
            };
            var expected = 20m;
            var service = GetMedicationsControllerWithData(new[] { medication });
            var medications = service.GetMedications().ToArray();
            var actual = medications[0].Price;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetMedications_GetData_CountIsCorrect_Test()
        {

            var data = new[]
            {
                new Medication
                {
                    MedicationId = 1,
                    Description = "Drug1",
                    Price = 20
                },
                new Medication
                {
                    MedicationId = 2,
                    Description = "Drug2",
                    Price = 30
                }
            };
            var expected = 2;
            var service = GetMedicationsControllerWithData(data);
            var medications = service.GetMedications().ToArray();
            var actual = medications.Length;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetMedication_GetExistingMedication_ResponceIsNotNull_Test()
        {
            var medication = new Medication
            {
                MedicationId = 1,
                Description = "Drug1",
                Price = 20
            };

            var service = GetMedicationsControllerWithData(new[] { medication });
            var actionResult = service.GetMedication(1)
                as OkNegotiatedContentResult<MedicationDto>;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void GetMedication_GetExistingMedication_ContentIsNotNull_Test()
        {
            var medication = new Medication
            {
                MedicationId = 1,
                Description = "Drug1",
                Price = 20
            };

            var service = GetMedicationsControllerWithData(new[] { medication });
            var actionResult = service.GetMedication(1)
                as OkNegotiatedContentResult<MedicationDto>;
            Assert.IsNotNull(actionResult.Content);
        }

        [TestMethod]
        public void GetMedication_GetExistingMedication_ContentIdIsCorrect_Test()
        {
            var medication = new Medication
            {
                MedicationId = 1,
                Description = "Drug1",
                Price = 20
            };

            var expected = 1;
            var service = GetMedicationsControllerWithData(new[] { medication });
            var actionResult = service.GetMedication(1)
                as OkNegotiatedContentResult<MedicationDto>;
            var actual = actionResult.Content.MedicationId;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetMedication_GetExistingMedication_ContentDescriptionIsCorrect_Test()
        {
            var medication = new Medication
            {
                MedicationId = 1,
                Description = "Drug1",
                Price = 20
            };

            var expected = "Drug1";
            var service = GetMedicationsControllerWithData(new[] { medication });
            var actionResult = service.GetMedication(1)
                as OkNegotiatedContentResult<MedicationDto>;
            var actual = actionResult.Content.Description;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetMedication_GetExistingMedication_ContentPriceIsCorrect_Test()
        {
            var medication = new Medication
            {
                MedicationId = 1,
                Description = "Drug1",
                Price = 20
            };

            var expected = 20m;
            var service = GetMedicationsControllerWithData(new[] { medication });
            var actionResult = service.GetMedication(1)
                as OkNegotiatedContentResult<MedicationDto>;
            var actual = actionResult.Content.Price;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetMedication_GetNotExistingMedication_ReturnsNotFound_Test()
        {
            var medication = new Medication
            {
                MedicationId = 1,
                Description = "Drug1",
                Price = 20
            };
            var service = GetMedicationsControllerWithData(new[] { medication });
            var actionResult = service.GetMedication(2);
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PostMedication_AddNewMedication_ReturnCorrectActionResult_Test()
        {
            var medication = new Medication
            {
                MedicationId = 1,
                Description = "Drug1",
                Price = 20
            };
            var servise = GetMedicationsController();
            var actionResult = servise.PostMedication(medication);
            Assert.IsInstanceOfType(
                actionResult, typeof(OkNegotiatedContentResult<Medication>));
        }

        [TestMethod]
        public void PostMedication_AddNewMedication_ReturnsCorrectResponceId_Test()
        {
            var medication = new Medication
            {
                MedicationId = 1,
                Description = "Drug1",
                Price = 20
            };

            var expected = 1;

            var servise = GetMedicationsController();
            var actionResult =servise.PostMedication(medication)            
                as OkNegotiatedContentResult<Medication>;
            var actual = actionResult.Content.MedicationId;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PostMedication_AddNewMedication_GetMedicationByIdReturnsCorrectResponceDescription_Test()
        {
            var medication = new Medication
            {
                MedicationId = 1,
                Description = "Drug1",
                Price = 20
            };

            var expected = "Drug1";

            var servise = GetMedicationsController();
            var actionResult = servise.PostMedication(medication)
                as OkNegotiatedContentResult<Medication>;
            var actual = actionResult.Content.Description;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PostMedication_AddNewMedication_GetMedicationByIdReturnsCorrectResponcePrice_Test()
        {
            var medication = new Medication
            {
                MedicationId = 1,
                Description = "Drug1",
                Price = 20
            };

            var expected = 20m;

            var servise = GetMedicationsController();
            var actionResult = servise.PostMedication(medication)
                as OkNegotiatedContentResult<Medication>;
            var actual = actionResult.Content.Price;
            Assert.AreEqual(expected, actual);
        }
    }
}