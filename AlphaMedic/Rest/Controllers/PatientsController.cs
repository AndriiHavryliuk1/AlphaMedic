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
using System.Security.Claims;
using System.Linq.Dynamic;
using System.Data.Entity.Validation;
using Microsoft.AspNetCore.JsonPatch;
using Rest.Dtos;
using Rest.Helpers;

namespace Rest.Controllers
{
    [RoutePrefix("api/patients")]
    public class PatientsController : ApiController
    {
        private AlphaMedicContext db = new AlphaMedicContext();

        public PatientsController() { }
        public PatientsController(AlphaMedicContext context)
        {
            this.db = context;
        }

        // GET: api/Patients
        [Route("")]
        [Authorize(Roles = Roles.Administrator + "," + Roles.AllDoctors + "," + Roles.Receptionist)]
        public IHttpActionResult GetPatients(int page = 1, int itemsPerPage = 15,
            string sortBy = "UserId", bool reverse = false,
            string search = null, int? doctor = null,
            int? department = null)
        {
            try
            {
                var currentUser = db.Users.FirstOrDefault(x => x.Email == this.User.Identity.Name);
                var maybeDoctor = db.Doctors.FirstOrDefault(x => x.UserId == currentUser.UserId);

                if (maybeDoctor != null)
                {
                    if (Tools.AnyRole(this.User, Roles.DoctorRoles)
                        && maybeDoctor.DoctorType != DoctorType.HospitalDean &&
                          (maybeDoctor.DoctorType == DoctorType.HeadDepartment && maybeDoctor.DepartmentId != department)
                        && (doctor != currentUser.UserId || doctor == null))
                    {
                        return Content(HttpStatusCode.Forbidden, Messages.AccsesDenied);
                    }
                }
            }
            catch (ArgumentNullException)
            {
                return InternalServerError();
            }

            var users = db.Patients.Include(c => c.Appointments).
            Select(x => new
            {
                x.UserId,
                x.Name,
                x.Surname,
                URLImage = Constants.ThisServer + x.URLImage,

                Procedure = x.Appointments.Select(
                p => new
                {
                    ProcedureId = p.AppointmentId,
                    ProcedureName = p.Procedure.Name ?? string.Empty,
                    Doctor = (p.Doctor != null ?
                    new
                    {
                        DoctorFullName = p.Doctor.Name + " " + p.Doctor.Surname,
                        DoctorId = p.Doctor.UserId,
                        DepartmentId = p.Doctor.DepartmentId
                    }
                    : null)
                }
                ).FirstOrDefault()
            }
            ).AsQueryable();

            if (department != null)
            {
                users = users.Where(x => x.Procedure.Doctor.DepartmentId == department);
            }

            if (doctor != null)
            {
                users = users.Where(x => x.Procedure.Doctor.DoctorId == doctor);
            }

            // searching
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                users = users.Where(x =>
                    (x.Name + x.Surname).ToLower().Contains(search.Replace(" ", "")) ||
                    (x.Surname + x.Name).ToLower().Contains(search.Replace(" ", "")));
            }

            // sorting (done with the System.Linq.Dynamic library available on NuGet)
            users = users.OrderBy(sortBy + (reverse ? "descending" : ""));

            // paging
            var usersPaged = users.Skip((page - 1) * itemsPerPage).Take(itemsPerPage);

            // json result
            var json = new
            {
                count = users.Count(),
                data = usersPaged
            };

            return Ok(json);

        }
        [Authorize]
        [Route("{id:int}/patients")]
        public IHttpActionResult GetPatientsByDoctorId(int id)
        {
            var currentUser = db.Users.FirstOrDefault(x => x.Email == this.User.Identity.Name);

            if ((Tools.AnyRole(this.User, Roles.DoctorRoles) && currentUser.UserId != id) && !this.User.IsInRole(Roles.Receptionist))
            {
                return Content(HttpStatusCode.Forbidden, Messages.AccsesDenied);
            }

            var dep = db.Doctors.FirstOrDefault(x => x.UserId == id).Department;
            var data = new
            {
                Department = new
                {
                    dep.DepartmentId,
                    dep.Name
                },
                Patients = db.Appointments.Where(x => x.DoctorId == id).Select(p => p.Patient).Select(a => new
                {
                    a.UserId,
                    a.Name,
                    a.Surname,
                    URLImage = Models.Constants.ThisServer + a.URLImage,

                    Procedure = a.Appointments.Select(
                       p => new
                       {
                           p.AppointmentId,
                           ProcedureName = p.Procedure.Name,
                           DoctorFullName = p.Doctor.Name + " " + p.Doctor.Surname,
                           DoctorId = p.Doctor.UserId
                       }
                       ).FirstOrDefault()
                }
                               ).AsQueryable()


            };

            return Ok(data);
        }



        [HttpPost]
        [Route("{id:int}/confirmRegistration")]
        [ResponseType(typeof(void))]
        public IHttpActionResult SendConfirmRegisterEmail(int id, object user)
        {

            EmailInput emailInput = new EmailInput();
            var pat = db.Patients.Find(id);
            emailInput.UserName = pat.FullName;
            emailInput.Email = pat.Email;
            emailInput.Subject = "Confirm registration!";

            try
            {
                EMailHelper.SendConfirmRegisterNotification(emailInput, id);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok();

        }

        [Authorize]
        [Route("~/api/patients/{id:int}/appointments")]

        public IHttpActionResult GetPatientAppointments(int id)
        {
            var currentUser = db.Users.FirstOrDefault(x => x.Email == this.User.Identity.Name);

            if (currentUser == null)
            {
                return NotFound();
            }

            if (this.User.IsInRole(Roles.Patinet) && currentUser.UserId != id)
            {
                return Content(HttpStatusCode.Forbidden, Messages.AccsesDenied);
            }

            var result = db.Appointments.Where(x => ((x.PatientId == id) && (x.Date > DateTime.Now))).Select(
               x => new
               {
                   x.AppointmentId,
                   x.Date,
                   x.Duration,
                   State = x.State == 0 ? "Unconfirmed" : "Accepted",
                   x.DoctorId,
                   x.Doctor.Name,
                   x.Doctor.Surname
               }
             );

            result = result.OrderBy("Date" );// + (true ? " descending" : ""));
            var usersPaged = result.Skip(Math.Max(0, result.Count() - 7)).ToArray();
            //result.OrderByDescending(x => x.Date).Skip(Math.Max(0, result.Count() - 5));
            return Ok(usersPaged);
        }

        // GET: api/Patients/5       


        [Authorize]
        [Route("{id:int}", Name = "GetPatientById")]
        public IHttpActionResult GetPatient(int id)
        {


            var currentUser = db.Users.FirstOrDefault(x => x.Email == this.User.Identity.Name);

            if (currentUser == null)
            {
                return NotFound();
            }


            if (this.User.IsInRole(Roles.Patinet) && id != currentUser.UserId)
            {
                return Content(HttpStatusCode.Forbidden, Messages.AccsesDenied);
            }

            var patient = db.Patients.FirstOrDefault(x => x.UserId == id);

            if (Tools.AnyRole(this.User, Roles.DoctorRoles) && !patient.Appointments.Any(x => x.DoctorId == currentUser.UserId))

            {
                return Content(HttpStatusCode.Forbidden, Messages.AccsesDenied);
            }


            if (!ModelState.IsValid || patient == null)
            {
                return NotFound();
            }

            var a = new
            {
                patient.UserId,
                patient.FullName,
                patient.DateOfBirth,
                patient.Phone,
                patient.Name,
                patient.Surname,
                patient.Address,
                Gender = patient.Gender.ToString(),
                patient.BloodGroup,
                URLImage = Constants.ThisServer + patient.URLImage
            };

            return Ok(a);




        }
        [Authorize(Roles = Roles.Administrator)]
        [Route("allPatients", Name = "AllPatients")]
        public IHttpActionResult GetAllPatients(int page = 1, int itemsPerPage = 15,
            string sortBy = "UserId", bool reverse = false,
            string search = null, bool? isActive = null)
        {
            var users = db.Patients.Select(
                 p => new
                 {
                     p.Name,
                     p.Surname,
                     Gender = p.Gender.ToString(),
                     p.DateOfBirth,
                     p.Phone,
                     p.Address,
                     p.Email,
                     p.Active,
                     p.UserId

                 });

            if (isActive != null)
            {
                users = users.Where(x => x.Active == isActive);
            }
            // searching
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                users = users.Where(x =>
                    x.Name.ToLower().Contains(search) ||
                    x.Surname.ToLower().Contains(search));
            }

            // sorting (done with the System.Linq.Dynamic library available on NuGet)
            users = users.OrderBy(sortBy + (reverse ? "descending" : ""));

            // paging
            var usersPaged = users.Skip((page - 1) * itemsPerPage).Take(itemsPerPage);


            var json = new
            {
                count = users.Count(),
                data = usersPaged
            };

            return Ok(json);

        }



        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("changeState/{id:int}", Name = "ChangeState")]
        public IHttpActionResult PutUser2(int id, object state)
        {
            var user = db.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }


            if (user.Active != true)
            {
                user.Active = true;
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
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


        [Authorize(Roles = Roles.Patinet)]
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("changepass/{id:int}", Name = "ChangePass2")]
        public IHttpActionResult PutPatient(int id, ChangePass user)
        {
            var currentUser = db.Users.FirstOrDefault(x => x.Email == this.User.Identity.Name);


            var tmp = db.Users.Find(id);

            if (tmp == null)
            {
                return NotFound();
            }

            if (currentUser.UserId != id)
            {
                return Content(HttpStatusCode.Forbidden, Messages.AccsesDenied);
            }

            if (user.OldPass != tmp.Password)
            {
                return BadRequest();
            }

            tmp.Password = user.NewPass;


            db.Entry(tmp).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
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



        // PUT: api/Patients/5
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("{id:int}", Name = "PutPatient")]
        public IHttpActionResult PutPatient(int id, Patient user)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            if (id != user.UserId)
            {
                return BadRequest();
            }

            var tmp = db.Users.Find(id);
            if (tmp == null)
            {
                return NotFound();
            }

            tmp.Name = user.Name;
            tmp.Surname = user.Surname;
            tmp.Phone = user.Phone;
            tmp.DateOfBirth = user.DateOfBirth;
            tmp.Address = user.Address;
            tmp.Gender = user.Gender;


            db.Entry(tmp).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        // POST: api/Patients
        [Route("")]
        [ResponseType(typeof(Patient))]
        public IHttpActionResult PostPatient(Patient patient)
        {
            if (!ModelState.IsValid || db.Users.Any(x => x.Email == patient.Email))
            {
                return BadRequest(ModelState);
            }

            var medicalHistory = new MedicalHistory() { MedicalHistoryId = patient.UserId };
            int UserClaimId = db.UserClaims.FirstOrDefault(x => x.ClaimValue == "Patient").Id;
            patient.UserClaimId = UserClaimId;

            patient.URLImage = Constants.DefaultPatientImage;
            try
            {
                db.Patients.Add(patient);
                db.MedicalHistorys.Add(medicalHistory);
                db.SaveChanges();
                return CreatedAtRoute("GetPatientById", new { id = patient.UserId }, patient);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/Patients/5
        [HttpDelete]
        [Route("{id:int}", Name = "DeletePatient")]
        [ResponseType(typeof(Patient))]
        public IHttpActionResult DeletePatient(int id, object data)
        {
            User patient = db.Users.Find(id);
            if (patient == null)
            {
                return NotFound();
            }

            db.Patients.Remove((Patient)patient);
            db.SaveChanges();

            return Ok();
        }


        [Route("forRegistrationConfirm/{id:int}", Name = "GetPatient")]
        public IHttpActionResult GetPatientForConfirm(int id)
        {


            var patient = db.Users.FirstOrDefault(x => x.UserId == id);

            if (patient == null)
            {
                return NotFound();
            }



            var a = new
            {
                patient.UserId,
                patient.Email,
                patient.Active
            };

            return Ok(a);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PatientExists(int id)
        {
            return db.Users.Count(e => e.UserId == id) > 0;
        }
    }
}