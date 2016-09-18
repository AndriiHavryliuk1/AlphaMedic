using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Rest.Models;
using Rest.Models.AlphaMedicContext;
using System;
using System.Linq.Dynamic;
using Microsoft.AspNetCore.JsonPatch;
using System.Data.Entity.Validation;
using Rest.Dtos;

namespace Rest.Controllers
{
    [RoutePrefix("api/Doctors")]
    public class DoctorsController : ApiController
    {
        private AlphaMedicContext db = new AlphaMedicContext();

        public DoctorsController() { }

        public DoctorsController(AlphaMedicContext context)
        {
            this.db = context;
        }

        [Route("")]
        public IHttpActionResult GetUsers(int page = 1, int itemsPerPage = 15, string sortBy = "DoctorId", bool reverse = false, string search = null, int? department = null, bool? isActive = null)
        {
            var users = db.Doctors.Select(x => new DoctorDto
            {
                UserId = x.UserId, //Achtung, do not touch.
                DoctorId = x.UserId,
                URLImage = Constants.ThisServer + x.URLImage,
                Name = x.Name,
                Surname = x.Surname,
                Degree = x.Degree,
                DepartmentId = x.DepartmentId,
                Education = x.Education,
                DoctorType = x.DoctorType.ToString(),
                DoctorTypeInt = x.DoctorType,
                DepartmentName = (x.Department != null ? x.Department.Name : null),
                Active = x.Active
            });

            if (department != null)
            {
                users = users.Where(x => x.DepartmentId == department);
            }

            if (isActive != null)
            {
                users = users.Where(x => x.Active == isActive);
            }
            // searching
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                users = users.Where(x =>
                    (x.Name+x.Surname).ToLower().Contains(search.Replace(" ", "")) ||
                    (x.Surname + x.Name).ToLower().Contains(search.Replace(" ", "")));
            }

            // sorting (done with the System.Linq.Dynamic library available on NuGet)
            users = users.OrderBy(sortBy + (reverse ? " descending" : ""));

            // paging
            var usersPaged = users.Skip((page - 1) * itemsPerPage).Take(itemsPerPage);

            // json result
            var json = new JsonDto()
            {
                count = users.Count(),
                data = usersPaged.ToArray()
            };

            return Ok(json);
        }


        // GET: api/Doctors/5
        [Route("{id:int}")]
        public IHttpActionResult GetDoctor(int id, [FromUri] bool all = true)
        {
            Doctor doctor = db.Doctors.FirstOrDefault(x => x.UserId == id);
            if (doctor == null)
            {
                return NotFound();
            }

            var feedbacks = doctor.Feedbacks.Select(x => new
            {
                x.Date,
                x.Description,
                x.DoctorId,
                x.FeedbackId,
                PatientFullName =
                (x.Patient == null ? "Anonymous" : x.Patient.Name + " " + x.Patient.Surname),
                PatientURLImage = (x.Patient == null ? Constants.ThisServer + Constants.DefaultPatientImage : Constants.ThisServer + x.Patient.URLImage)
            });

            var res = new
            {
                doctor.DepartmentId,
                doctor.UserId,
                DepartmentName = doctor.Department.Name,
                doctor.ScheduleId,
                doctor.FullName,
                doctor.Phone,
                doctor.Degree,
                doctor.Education,
                DoctorType= doctor.DoctorType.ToString(),
                Feedbacks = all == true ? feedbacks : feedbacks.Skip(Math.Max(0, doctor.Feedbacks.Count - 3)),
                FeedbacksCount = doctor.Feedbacks.Count,
                Gender = doctor.Gender.ToString(),
                URLImage = Constants.ThisServer + doctor.URLImage,
                doctor.Schedule.StartWorkingTime,
                doctor.Schedule.FinishWorkingTime
            };

            return Ok(res);
        }

        // GET: api/Doctors/5/Appointments
        [Route("{id:int}/Appointments")]
        [Authorize(Roles =Roles.AllDoctors+","+Roles.Receptionist)]
        public IHttpActionResult GetAppointmentList(int id)
        {
            Doctor doctor = db.Doctors.FirstOrDefault(x => x.UserId == id);
            if (doctor == null)
            {
                return NotFound();
            }
            var res = new
            {
                doctor.UserId,
                doctor.ScheduleId,
                doctor.Name,
                doctor.Surname,
                doctor.Gender,
                doctor.DateOfBirth,
                doctor.Address,
                doctor.Phone,
                doctor.DepartmentId,
                doctor.DoctorType,
                URLImage = Constants.ThisServer + doctor.URLImage,

                Appointments = db.Appointments.Where(x => x.DoctorId == id && x.Date > DateTime.Now).Select(a => new
                {
                    a.AppointmentId,
                    PatientFullName = a.Patient.Name + " " + a.Patient.Surname,
                    a.PatientId,
                    a.Date,
                    a.Duration,
                    State = a.State == 0 ? "Unconfirmed" : "Accepted"
                })

            };
            res.Appointments.OrderByDescending(x => x.Date).Skip(Math.Max(0, res.Appointments.Count() - 5));
            return Ok(res);

        }

        // PUT: api/Doctors/5
        [Route("{id:int}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDoctor(int id, Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != doctor.UserId)
            {
                return BadRequest();
            }

            db.Entry(doctor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPut]
        [Route("~/api/scheduleUpdate/{id:int}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutWorkingHours(int id, Schedule schedule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != schedule.ScheduleId)
            {
                return BadRequest();
            }

            db.Entry(schedule).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("")]
        // POST: api/Doctors
        [ResponseType(typeof(Doctor))]
        public IHttpActionResult PostDoctor(Doctor doctor)
        {
            if (!ModelState.IsValid || (db.Users.FirstOrDefault(x => x.Email == doctor.Email) != null))
            {
                return BadRequest(ModelState);
            }

            Schedule schedule = new Schedule()
            {
                StartWorkingTime = TimeSpan.FromHours(0),
                FinishWorkingTime = TimeSpan.FromHours(0)

            };
            try
            {

                db.Schedules.Add(schedule);
                db.SaveChanges();

                doctor.ScheduleId = schedule.ScheduleId;

                var type = doctor.EmployeeType.ToString();

                doctor.UserClaim = db.UserClaims.FirstOrDefault(x => x.ClaimValue == type);

                doctor.EmploymentDate = DateTime.Now;

                doctor.URLImage = Constants.DefaultDoctorImage;

                db.Doctors.Add(doctor);

                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return InternalServerError(ex);
            }

            return Ok();
        }
        // PATCH: 
        [Route("{id:int}")]
        public IHttpActionResult PatchActiveState(int id, JsonPatchDocument<Doctor> patchData)
        {
            var doctor = db.Doctors.Find(id);
         
            patchData.ApplyTo(doctor);

            if (doctor.DoctorType == DoctorType.HeadDepartment)
            {
                var headDoc = db.Doctors.Where(x => (x.DepartmentId == doctor.DepartmentId && x.DoctorType == DoctorType.HeadDepartment)).FirstOrDefault();
                if (headDoc != null && headDoc.UserId != doctor.UserId)
                {
                    return BadRequest("Department head is already exists");
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                string s = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    s += String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                         eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        s += String.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }

                return BadRequest(s);
            }
            return Ok();

        }

     /*  // DELETE: api/Doctors/5
        [ResponseType(typeof(Doctor))]
        public IHttpActionResult DeleteDoctor(int id)
        {
            Doctor doctor = db.Doctors.FirstOrDefault(x => x.UserId == id);
            if (doctor == null)
            {
                return NotFound();
            }

            db.Users.Remove(doctor);
            db.SaveChanges();

            return Ok(doctor);
        }*/

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DoctorExists(int id)
        {
            return db.Users.Count(e => e.UserId == id) > 0;
        }
    }
}