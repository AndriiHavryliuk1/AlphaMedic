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

namespace Rest.Controllers
{
    public class VaccionationsController : ApiController
    {
        private AlphaMedicContext db = new AlphaMedicContext();

        // GET: api/Vaccionations
        public IQueryable GetProcedures()
        {
            return db.Procedures;
        }

        // GET: api/Vaccionations/5
        [Authorize]
        [ResponseType(typeof(Vaccionation))]
        public IHttpActionResult GetVaccionation(int id)
        {
            var currentUser = db.Users.FirstOrDefault(x => x.Email == this.User.Identity.Name);
          
                       
            Vaccionation vaccionation = (Vaccionation)db.Procedures.Find(id);

            if(this.User.IsInRole(Roles.Patinet) && vaccionation.Appointment.PatientId!=currentUser.UserId)
            {
                return Content(HttpStatusCode.Forbidden, Messages.AccsesDenied);
            }

            if (vaccionation == null)
            {
                return NotFound();
            }

            return Ok(vaccionation);
        }

        // PUT: api/Vaccionations/5
        [Authorize(Roles = Roles.AllDoctors)]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVaccionation(int id, Vaccionation vaccionation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vaccionation.ProcedureId)
            {
                return BadRequest();
            }

            db.Entry(vaccionation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VaccionationExists(id))
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

        // POST: api/Vaccionations
        [Authorize(Roles =Roles.AllDoctors)]
        [ResponseType(typeof(Vaccionation))]
        public IHttpActionResult PostVaccionation(Vaccionation vaccionation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            //check if procedure already set
            if (db.Procedures.FirstOrDefault(x => x.ProcedureId == vaccionation.ProcedureId) != null)
                return Content(HttpStatusCode.Conflict, "Procedure existed!");


            db.Procedures.Add(vaccionation);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (VaccionationExists((int)vaccionation.ProcedureId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = vaccionation.ProcedureId }, vaccionation);
        }
        /*
        // DELETE: api/Vaccionations/5
        [ResponseType(typeof(Vaccionation))]
        public IHttpActionResult DeleteVaccionation(int id)
        {
            var vaccionation = db.Procedures.Find(id);
            if (vaccionation == null)
            {
                return NotFound();
            }

            db.Procedures.Remove(vaccionation);
            db.SaveChanges();

            return Ok(vaccionation);
        }*/

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VaccionationExists(int id)
        {
            return db.Procedures.Count(e => e.ProcedureId == id) > 0;
        }
    }
}