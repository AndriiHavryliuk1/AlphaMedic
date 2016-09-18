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
using Rest.Dtos;

namespace Rest.Controllers
{   [Authorize]
    [RoutePrefix("api/examinations")]
    public class ExaminationsController : ApiController
    {
        private AlphaMedicContext db = new AlphaMedicContext();

        // GET: api/Examinations
        [Route("")]
        [Authorize]
        public IHttpActionResult GetExaminations()
        {
            return Ok(db.Examinations.Select(
               x => new ProcedureDto
               {
                   Date = x.Date,
                   ProcedureId = (int)x.ProcedureId,
                   Name = x.Name,
                   Description = x.Description,
                   Price = (decimal)x.Price,                   
               }
                ));
        }

        // GET: api/Examinations/5
        [Route("{id:int}")]
        [ResponseType(typeof(ProcedureDto))]
        [Authorize(Roles =Roles.Receptionist+","+Roles.Patinet+","+Roles.AllDoctors)]
        public IHttpActionResult GetExamination(int id)
        {
            
            Examination examination = (Examination)db.Procedures.Find(id);
                                       
            var rez = new 
            {
                ProcedureId = (int)examination.ProcedureId,
                ProcedureName = examination.Name,
                ProcedureDescription = examination.Description,
                Price=(decimal)examination.Price                  
                
            };
            if (examination == null)
            {
                return NotFound();
            }

            return Ok(rez);
        }

        // PUT: api/Examinations/5
        [Authorize(Roles =Roles.Receptionist)]
        [ResponseType(typeof(void))]
        [Route("{id:int}")]
        public IHttpActionResult PutExamination(int id, Examination examination)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != examination.ProcedureId)
            {
                return BadRequest();
            }

            db.Entry(examination).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExaminationExists(id))
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

        [Route("{id:int}")]     
        [Authorize(Roles=Roles.Receptionist)]
        public IHttpActionResult PostDiagnosis(int id, Diagnosis diagnosis) {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Diagnosiss.Add(diagnosis);
            db.SaveChanges();
            var examination = db.Examinations.Find(id);
            db.Entry(examination).State = EntityState.Modified;
            examination.Diagnosis = diagnosis;
            db.SaveChanges();
            return Ok(diagnosis);
        }


        // POST: api/Examinations
        [Route("")]
        [ResponseType(typeof(Examination))]
        [Authorize(Roles=Roles.Receptionist+","+Roles.AllDoctors)]
        public IHttpActionResult PostExamination(Examination examination)
        {           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //check if procedure already set
            if (db.Procedures.FirstOrDefault(x => x.ProcedureId == examination.ProcedureId) != null)
                return Content(HttpStatusCode.Conflict,"Procedure existed!");

            db.Examinations.Add(examination);
            db.SaveChanges();

            return Ok(examination);
        }

        // DELETE: api/Examinations/5
        [ResponseType(typeof(Examination))]
        public IHttpActionResult DeleteExamination(int id)
        {
            Examination examination = (Examination)db.Procedures.Find(id);
            if (examination == null)
            {
                return NotFound();
            }

            db.Procedures.Remove(examination);
            db.SaveChanges();

            return Ok(examination);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ExaminationExists(int id)
        {
            return db.Procedures.Count(e => e.ProcedureId == id) > 0;
        }
    }
}