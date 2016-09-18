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
using RestWebApi.Models;
using RestWebApi.DTOs;

namespace RestWebApi.Controllers
{
    public class PatientsController : ApiController
    {
        private RestWebApiContext db = new RestWebApiContext();

        // GET: api/Patients
        public IQueryable GetPatients()
        {

            return from p in db.Patients
                   from r in db.Procedures
                   where r.PatientId == p.PatientId
                   select new
                   {
                       PatientId = p.PatientId,
                       PatientFullName = p.User.UserName + " " + p.User.UserSurname,
                       CurrentProcedure = r.Name,
                       DoctorId = r.DoctorId,
                       DoctorFullName = r.Doctor.User.UserName + " " + r.Doctor.User.UserSurname
                   };
        }

        // GET: api/Patients/5
        //[ResponseType(typeof(Patient))]
        public IHttpActionResult GetPatient(int id)
        {
            var patient = (from p in db.Patients
                           from u in db.Users
                           where u.UserId == p.UserId
                           select new
                           {
                               PatientName = u.UserName,
                               PatientSurname = u.UserSurname,
                               PatientDateOfBirth = u.UserDateOfBirth,
                               PatientPhone = u.UserPhone,
                               PatientAdress = u.UserAdress,
                               PatientGender = u.UserGender,
                               PatientBloodGroup = p.BloodGroup
                           }).FirstOrDefault();

            //Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }

            return Ok(patient);
        }

        // PUT: api/Patients/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPatient(int id, Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != patient.PatientId)
            {
                return BadRequest();
            }

            db.Entry(patient).State = EntityState.Modified;

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

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Patients
        [ResponseType(typeof(Patient))]
        public IHttpActionResult PostPatient(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Patients.Add(patient);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = patient.PatientId }, patient);
        }

        // DELETE: api/Patients/5
        [ResponseType(typeof(Patient))]
        public IHttpActionResult DeletePatient(int id)
        {
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }

            db.Patients.Remove(patient);
            db.SaveChanges();

            return Ok(patient);
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
            return db.Patients.Count(e => e.PatientId == id) > 0;
        }
    }
}