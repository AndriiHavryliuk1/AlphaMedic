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
    [RoutePrefix("api/doctors")]
    public class DoctorsController : ApiController
    {
        private RestWebApiContext db = new RestWebApiContext();

        // GET: api/Doctors
        [Route("")]
        public IQueryable GetDoctors()
        {
            return from u in db.Users
                   join d in db.Doctors on u.UserId equals d.UserId //into gj
                   join v in db.Departments on d.DepartmentId equals v.DepartmentId
                   select new
                   {
                       d.DoctorId,
                       d.Degree,
                       d.Education,
                       u.UserName,
                       u.UserSurname,
                       v.DepartmentId,
                       v.DepartmentName
                   };


            //from d in db.Doctors
            //join u in db.Users on d.UserId equals u.UserId into gd
            //join v in db.Departments on              
        }

        // GET: api/Doctors/5
        [Route("{id:int}")]
        [ResponseType(typeof(DoctorDetailsDto))]
        public IHttpActionResult GetDoctor(int id)
        {

            var doctor = (from u in db.Users
                         join d in db.Doctors on u.UserId equals d.UserId
                         join v in db.Departments on d.DepartmentId equals v.DepartmentId
                         where d.DoctorId == id
                         select new
                         {
                             FullName = u.UserName + " " + u.UserSurname,
                             d.Degree,
                             d.Education,
                             u.UserGender,
                             u.UserPhone,
                             v.DepartmentId,
                             v.DepartmentName,
                             Feedbacks = from f in db.Feedbacks
                                         join u2 in db.Users on f.UserId equals u2.UserId
                                         where f.DoctorId == d.DoctorId
                                         select new
                                         {
                                             f.FeedbackId,
                                             f.FeedbackDescription,
                                             u2.UserId,
                                             UserFullName = u2.UserName + " " + u2.UserSurname,
                                             f.FeedbackDate
                                         }
                         }).FirstOrDefault();

            if (doctor == null)
            {
                return NotFound();
            }

            return Ok(doctor);
        }

        // PUT: api/Doctors/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDoctor(int id, Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != doctor.DoctorId)
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

        // POST: api/Doctors
        [ResponseType(typeof(Doctor))]
        public IHttpActionResult PostDoctor(Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Doctors.Add(doctor);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = doctor.DoctorId }, doctor);
        }

        // DELETE: api/Doctors/5
        [ResponseType(typeof(Doctor))]
        public IHttpActionResult DeleteDoctor(int id)
        {
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return NotFound();
            }

            db.Doctors.Remove(doctor);
            db.SaveChanges();

            return Ok(doctor);
        }

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
            return db.Doctors.Count(e => e.DoctorId == id) > 0;
        }
    }
}