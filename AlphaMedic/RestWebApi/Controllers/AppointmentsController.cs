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

namespace RestWebApi.Controllers
{
    [RoutePrefix("api/appointments")]
    public class AppointmentsController : ApiController
    {
        private RestWebApiContext db = new RestWebApiContext();

        // GET: api/Appointments
        public IQueryable GetAppointments()
        {
            return from a in db.Appointments
                   join d in db.Doctors on a.DoctorId equals d.DoctorId
                   join p in db.Patients on a.PatientId equals p.PatientId
                   select new
                   {
                       a.Id,
                       a.ProcedureId,
                       a.AppointmentState,
                       a.AppointmentDuration,
                       a.AppointmentDate,
                       Doctor = (from u in db.Users
                                 where d.UserId == u.UserId
                                 select new
                                 {
                                     d.DoctorId,
                                     u.UserName,
                                     u.UserSurname
                                 }).FirstOrDefault(),
                       Patient = (from u2 in db.Users
                                  where p.UserId == u2.UserId
                                  select new
                                  {
                                      p.PatientId,
                                      u2.UserName,
                                      u2.UserSurname
                                  }).FirstOrDefault()
                   };
            //  return null;
        }

        [Route("~/api/patientCabinet/{id:int}/appointments")]
        public IQueryable GetPatientAppointments(int id)
        {
            return from a in db.Appointments
                   where a.PatientId == id
                   join d in db.Doctors on a.DoctorId equals d.DoctorId
                   join p in db.Patients on a.PatientId equals p.PatientId
                   select new
                   {
                       a.Id,
                       a.ProcedureId,
                       a.AppointmentState,
                       a.AppointmentDuration,
                       a.AppointmentDate,
                       Doctor = (from u in db.Users
                                 where d.UserId == u.UserId
                                 select new
                                 {
                                     d.DoctorId,
                                     u.UserName,
                                     u.UserSurname
                                 }).FirstOrDefault(),
                       Patient = (from u2 in db.Users
                                  where p.UserId == u2.UserId
                                  select new
                                  {
                                      p.PatientId,
                                      u2.UserName,
                                      u2.UserSurname
                                  }).FirstOrDefault()
                   };
            //  return null;
        }

        [Route("~/api/DoctorCabinet/{id:int}/appointments")]
        public IQueryable GetDoctorAppointments(int id)
        {
            return from a in db.Appointments
                   where a.DoctorId == id
                   join d in db.Doctors on a.DoctorId equals d.DoctorId
                   join p in db.Patients on a.PatientId equals p.PatientId
                   select new
                   {
                       a.Id,
                       a.ProcedureId,
                       a.AppointmentState,
                       a.AppointmentDuration,
                       a.AppointmentDate,
                       Doctor = (from u in db.Users
                                 where d.UserId == u.UserId
                                 select new
                                 {
                                     d.DoctorId,
                                     u.UserName,
                                     u.UserSurname
                                 }).FirstOrDefault(),
                       Patient = (from u2 in db.Users
                                  where p.UserId == u2.UserId
                                  select new
                                  {
                                      p.PatientId,
                                      u2.UserName,
                                      u2.UserSurname
                                  }).FirstOrDefault()
                   };
            //  return null;
        }

        [Route("~/api/schedule/{id:int}")]
        public IQueryable GetDoctorAppointmentsAsEvents(int id)
        {
            return from a in db.Appointments
                   where a.DoctorId == id
                   join d in db.Doctors on a.DoctorId equals d.DoctorId
                   join p in db.Patients on a.PatientId equals p.PatientId
                   select new
                   {
                       url = "#/appointmentInfo/" + a.Id.ToString(),
                       finish = a.AppointmentDuration,
                       start = a.AppointmentDate,
                       title = (from u2 in db.Users
                                where p.UserId == u2.UserId
                                select u2.UserName + " " + u2.UserSurname
                                ).FirstOrDefault()
                   };
            //  return null;
        }


        // GET: api/Appointments/5
        [ResponseType(typeof(Appointment))]
        public IHttpActionResult GetAppointment(int id)
        {
            var appointment = (from a in db.Appointments
                               join d in db.Doctors on a.DoctorId equals d.DoctorId
                               join p in db.Patients on a.PatientId equals p.PatientId
                               where a.Id == id
                               select new
                               {
                                   a.Id,
                                   a.ProcedureId,
                                   a.AppointmentState,
                                   a.AppointmentDuration,
                                   a.AppointmentDate,
                                   a.AppointmentDescription,
                                   Procedure = (from p in db.Procedures
                                                where a.ProcedureId == p.Id
                                                select new
                                                {
                                                    p.Id,
                                                    p.Name,
                                                    p.Description,
                                                    p.Diagnosis,
                                                }).FirstOrDefault(),
                                   Doctor = (from u in db.Users
                                             where d.UserId == u.UserId
                                             select new
                                             {
                                                 d.DoctorId,
                                                 u.UserName,
                                                 u.UserSurname
                                             }
                                             ).FirstOrDefault(),
                                   Patient = (from u2 in db.Users
                                              where p.UserId == u2.UserId
                                              select new
                                              {
                                                  p.PatientId,
                                                  u2.UserName,
                                                  u2.UserSurname
                                              }
                                              ).FirstOrDefault()
                               }).FirstOrDefault();




            //   Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return NotFound();
            }

            return Ok(appointment);
        }

        // PUT: api/Appointments/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAppointment(int id, Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != appointment.Id)
            {
                return BadRequest();
            }

            db.Entry(appointment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
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

        // POST: api/Appointments
        [ResponseType(typeof(Appointment))]
        public IHttpActionResult PostAppointment(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Appointments.Add(appointment);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = appointment.Id }, appointment);
        }

        // DELETE: api/Appointments/5
        [ResponseType(typeof(Appointment))]
        public IHttpActionResult DeleteAppointment(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return NotFound();
            }

            db.Appointments.Remove(appointment);
            db.SaveChanges();

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
            return db.Appointments.Count(e => e.Id == id) > 0;
        }
    }
}