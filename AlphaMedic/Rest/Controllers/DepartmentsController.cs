using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Rest.Models;
using Rest.Models.AlphaMedicContext;
using Microsoft.AspNetCore.JsonPatch;
using System.Data.Entity.Validation;
using Rest.Dtos;

namespace Rest.Controllers
{
    [RoutePrefix("api/departments")]
    public class DepartmentsController : ApiController
    {

        private AlphaMedicContext db = new AlphaMedicContext();

        public DepartmentsController() { }

        public DepartmentsController(AlphaMedicContext context)
        {
            db = context;
        }

        // GET: api/Departments
        [Route("")]
        public IEnumerable<DepartmentDto> GetDepartments()
        {
            try
            {
                return db.Departments.Select(x => new DepartmentDto
                {
                    Name = x.Name,
                    Description = x.Description,
                    DepartmentId = x.DepartmentId
                }
                );
            }
            catch (NullReferenceException)
            {
                return Enumerable.Empty<DepartmentDto>().AsQueryable();
            }
        }

        // GET: api/Departments/5
        [Route("{id:int}")]
        public IHttpActionResult GetDepartment(int id, [FromUri] bool all = true)
        {
            Department department = db.Departments.FirstOrDefault(x => x.DepartmentId == id);
            if (department == null)
            {
                return NotFound();
            }

            var doc = department.Doctors.FirstOrDefault(x => x.DoctorType == DoctorType.HeadDepartment);

            var feedbacks = department.Feedbacks.Select(x => new FeedbackDto
            {
                Date = x.Date,
                Description = x.Description,
                DepartmentId = x.DepartmentId,
                FeedbackId = x.FeedbackId,
                PatientFullName = (x.Patient == null ? "Anonymous" : x.Patient.Name + " " + x.Patient.Surname),
                 PatientURLImage = (x.Patient == null ? Constants.ThisServer + Constants.DefaultPatientImage : Constants.ThisServer + x.Patient.URLImage)
            });

            ShortUserDto HeadDepartment = null;

            if (doc != null)
            {
                HeadDepartment = new ShortUserDto
                {
                    Surname = doc.Surname,
                    Name = doc.Name,
                    UserId = doc.UserId
                };
            }
            var a = new DepartmentFullDto
            {
                HeadDepartment = HeadDepartment,
                DepartmentId = department.DepartmentId,
                Description = department.Description,
                Name = department.Name,
                Feedbacks = all == true ? feedbacks.ToArray() : feedbacks.Skip(Math.Max(0, department.Feedbacks.Count - 3)).ToArray(),
                FeedbacksCount = department.Feedbacks.Count,
                URLImage = Constants.ThisServer + department.URLImage
            };
            return Ok(a);
        }

        //GET: api/departments/5/Doctors
        [Route("{id:int}/doctors")]
        public IEnumerable<DoctorDto> GetDepartmentDoctors(int id)
        {
            try
            {
                return db.Doctors.Where(d => d.DepartmentId == id)
                    .Select(x =>
                          new DoctorDto
                          {
                              UserId = x.UserId,
                              Name = x.Name,
                              Surname = x.Surname,
                              DepartmentId = (x.Department != null ? x.Department.DepartmentId : default(int)),
                              DepartmentName = (x.Department != null ? x.Department.Name : string.Empty),
                              Degree = x.Degree,
                              Education = x.Education,
                              StartWorkingTime = (x.Schedule != null ? x.Schedule.StartWorkingTime : default(TimeSpan)),
                              FinishWorkingTime = (x.Schedule != null ? x.Schedule.FinishWorkingTime : default(TimeSpan)),
                              URLImage = Constants.ThisServer + x.URLImage,
                          }).ToArray();
            }
            catch (NullReferenceException)
            {
                return Enumerable.Empty<DoctorDto>();
            }
        }

     

        // POST: api/Departments
        [Route("")]
        [Authorize(Roles=Roles.Administrator)]
        public IHttpActionResult PostDepartment(Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                db.Departments.Add(department);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok(department.DepartmentId);
        }

        [Route("{id:int}")]
        [Authorize(Roles=Roles.Administrator)]
        public IHttpActionResult PatchDepartment(int id, JsonPatchDocument<Department> patchData)
        {
            var department = db.Departments.Find(id);

            patchData.ApplyTo(department);

            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException)
            {
                return BadRequest();
            }
            return Ok();
        }

        // DELETE: api/Departments/5
     /*   [ResponseType(typeof(Department))]
        public IHttpActionResult DeleteDepartment(int id)
        {
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }

            db.Departments.Remove(department);
            db.SaveChanges();

            return Ok(department);
        }*/

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DepartmentExists(int id)
        {
            return db.Departments.Count(e => e.DepartmentId == id) > 0;
        }
    }
}