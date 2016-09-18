using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Rest.Models;
using Rest.Models.AlphaMedicContext;
using Rest.Helpers;
using System.Threading.Tasks;
using Rest.Dtos;
using System.Linq.Dynamic;
using System.Security.Principal;
using System.Collections.Generic;

namespace Rest.Controllers
{
    //[Authorize(Roles = "Patient,Doctor")]
    [RoutePrefix("api/appointments")]
    public class AppointmentsController : ApiController
    {
        private AlphaMedicContext db = new AlphaMedicContext();
        

        
        // GET: api/Appointments
        [Route("")]
        [Authorize(Roles=Roles.Receptionist+","+Roles.AllDoctors)]
        public IHttpActionResult GetAppointments(DateTime? periodFrom, DateTime? periodTill,
            AppointmentState? state = null, int page = 1, int itemsPerPage = 15, int? doctor = null, int? department = null)
        {
            var list = db.Appointments.ToArray();
            
            #region Filterdate
            if (doctor != null)
            {
                list = list.Where(x => x.DoctorId == doctor).ToArray();
            }

            if (department != null)
            {
                list = list.Where(x => x.Doctor.DepartmentId == department).ToArray();
            }

            if (periodFrom != null && periodTill != null)
            {
                list = list.Where(x => x.Date >= periodFrom && x.Date <= periodTill).ToArray();
            }
            if (periodFrom != null)
            {
                list = list.Where(x => x.Date >= periodFrom).ToArray();
            }

            if (periodTill != null)
            {
                list = list.Where(x => x.Date <= periodTill).ToArray();
            }

            if (state != null)
            {
                list = list.Where(x => x.State == state).ToArray();
            }
            #endregion
            var a = list.Where(x => x.Date == null).Select(x => new
            {
                x.AppointmentId,
                x.DoctorId,
                Doctor = new { Name = x.Doctor != null ? x.Doctor.Name : null, Surname = x.Doctor != null ? x.Doctor.Surname : null },
                x.Date,
                x.Duration,
                State = x.State == 0 ? "Unconfirmed" : "Accepted"
            }).ToList();
            var b = list.Where(x => x.Date != null).Select(x => new
            {
                x.AppointmentId,
                x.DoctorId,
                Doctor  = new { Name = x.Doctor != null ? x.Doctor.Name : null, Surname = x.Doctor != null ? x.Doctor.Surname : null },
                x.Date,
                x.Duration,
                State = x.State == 0 ? "Unconfirmed" : "Accepted"
            }).OrderByDescending(x => x.Date > DateTime.Now).ToList();
            var c = a.Concat(b);

            // sorting (done with the System.Linq.Dynamic library available on NuGet)
            c = c.OrderBy("Date" + (true ? " descending" : "")).ToList();


            // paging
            var usersPaged = c.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
            var json = new
            {
                count = list.Count(),
                data = usersPaged
            };

            return Ok(json);
        }
   


        // GET: api/Appointments/5
        [Route("{id:int}")]
        [Authorize(Roles=Roles.AllDoctors + "," + Roles.Patinet + "," + Roles.Receptionist)]
        [ResponseType(typeof(Appointment))]
        public IHttpActionResult GetAppointment(int id)
        {

            var currentUser = db.Users.FirstOrDefault(x => x.Email == this.User.Identity.Name);

            if (currentUser == null)
            {
                return Content(HttpStatusCode.NotFound, Messages.UserNotFound);
            }

            Appointment appointment = db.Appointments.FirstOrDefault(x => x.AppointmentId == id);

            if(appointment == null)
            {
                return Content(HttpStatusCode.NotFound, Messages.AppointmentNotFound);
            }
            
            if(this.User.IsInRole(Roles.Patinet)&& appointment.PatientId!=currentUser.UserId)
            {
                return Content(HttpStatusCode.Forbidden, Messages.AccsesDenied);
            }

            try
            {

                var a = new
                {
                    DoctorFullName = appointment.Doctor != null ? appointment.Doctor.Name + " " + appointment.Doctor.Surname : null,
                    DoctorURLImage = appointment.Doctor != null ? Constants.ThisServer + appointment.Doctor.URLImage : null,
                    DoctorId = appointment.DoctorId,
                    PatientId = appointment.PatientId,
                    PatientFullName = appointment.Patient.Name + " " + appointment.Patient.Surname,
                    PatientURLImage = Constants.ThisServer + appointment.Patient.URLImage,
                    Date = appointment.Date,
                    Description = appointment.Description,
                    Duration = appointment.Duration,
                    State = appointment.State == 0 ? "Unconfirmed" : "Accepted",
                    ProcedureType = appointment.Procedure == null ? null : appointment.Procedure.GetType().BaseType.Name
                };
                return Ok(a);
            } catch (Exception ex)
            {
                return InternalServerError(ex);
            }

           

         
        }

        [HttpPut]
        [Authorize(Roles=Roles.Receptionist)]
        [Route("{id:int}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateAndConfirmAppointment(int id, Appointment appointment)
        {

            appointment.AppointmentId = id;

            db.Entry(appointment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                //   SendEmail(appointment);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
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

        [HttpPost]
        [Route("{id:int}/sendmail")]
        [ResponseType(typeof(void))]
        public IHttpActionResult SendEmail(int id, EmailPostDto emailPostDto)
        {
            if (emailPostDto.deleteFlag == true && emailPostDto.appointment.Date < DateTime.Now)
            {
                return BadRequest();
            }

            EmailInput emailInput = new EmailInput();
            var pat = db.Patients.Find(emailPostDto.appointment.PatientId);
            emailInput.UserName = pat.FullName;
            emailInput.Email = pat.Email;
            emailInput.Subject = emailPostDto.deleteFlag == false ? "Confirm appointment!" : "Deleted appointment!";

            emailInput.Body = emailPostDto.deleteFlag == false ?
                "Hello! \nYou reserved to apointment on " + emailPostDto.appointment.Date + "\nYour doctor:" + 
                db.Doctors.Find(emailPostDto.appointment.DoctorId).FullName + "\nBest regard,\nAlphaMedic" :


                 "Hello! \nI am sorry but you deleted from your appointment:\nAppointment date:" + emailPostDto.appointment.Date +
                 "\nAppointment symptoms:" + emailPostDto.appointment.Description +
                 "\nbecause It has already recerved. Please register for anouther" +
                 " appointment and choose anouther date.\nBest regard,\nAlphaMedic";
            try
            {
                EMailHelper.SendNotification(emailInput);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok();

        }

        //GET: api/schedule/1
        [Route("~/api/schedule/{id:int}")]
        public IQueryable GetDoctorAppointmentsAsEvents(int id)
        {

            var a = db.Appointments.Where(x => x.Doctor.ScheduleId == id).ToArray();
            var b = a.Select(e => new
            {
                url = "#/appointmentInfo/" + e.AppointmentId.ToString(),
                start = e.Date,
                finish = e.Date + e.Duration,
                title = e.Patient.Name + " " + e.Patient.Surname
            }
           );


            return b.AsQueryable();
        }


        [Route("")]
        [ResponseType(typeof(Appointment))]
        [Authorize(Roles = Roles.Receptionist + "," + Roles.Patinet+ ","+Roles.AllDoctors)]
        public IHttpActionResult PostAppointment(Appointment appointment)
        {
            if (db.Employees.Any(x => x.UserId == appointment.PatientId))
            {
                return BadRequest("Employee can't register for appointment");
            }

            if (appointment.Date < DateTime.Now)
            {
                return BadRequest();
            }

            appointment.Doctor = db.Doctors.Include(c => c.Schedule).
                   FirstOrDefault(d => d.UserId == appointment.DoctorId);
            appointment.Patient = db.Patients.Include(c => c.MedicalHistory).
                FirstOrDefault(p => p.UserId == appointment.PatientId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Appointments.Add(appointment);
            db.SaveChanges();

            return Ok(new
            {
                Id = appointment.AppointmentId,
                Date = appointment.Date
            });

        }


        // DELETE: api/Appointments/5
        [Route("{id:int}")]
        [ResponseType(typeof(Appointment))]
        [Authorize(Roles=Roles.Receptionist)]
        public IHttpActionResult DeleteAppointment(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return NotFound();
            }

            try
            {
                db.Procedures.Remove(db.Procedures.Find(id));
                db.SaveChanges();
                db.Appointments.Remove(appointment);
                db.SaveChanges();
                //  SendEmail(appointment, true);
            }
            catch (Exception)
            {
                return NotFound();
            }
            return Ok(appointment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AppointmentExists(int id)
        {
            return db.Appointments.Count(e => e.AppointmentId == id) > 0;
        }
    }
}