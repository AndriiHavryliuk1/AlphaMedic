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
using System.Linq.Dynamic;
using Microsoft.AspNetCore.JsonPatch;
using System.Data.Entity.Validation;

namespace Rest.Controllers
{
    //[Authorize(Roles = "Patient")]
    [RoutePrefix("api/medications")]
    public class MedicationsController : ApiController
    {
        private AlphaMedicContext db = new AlphaMedicContext();

        public MedicationsController() { }

        public MedicationsController(AlphaMedicContext context)
        {
            db = context;
        }

        // GET: api/Medications
        [Route("")]
        [Authorize]
        public IEnumerable<MedicationDto> GetMedications()
        {
            try
            {
                return db.Medications.Select(
                    x => new MedicationDto
                    {
                        MedicationId = x.MedicationId,
                        Description = x.Description,
                        Price = x.Price
                    }
                    );
            }
            catch (NullReferenceException)
            {
                return Enumerable.Empty<MedicationDto>();
            }
        }


        [Route("{id:int}")]
        [Authorize(Roles=Roles.Administrator)]
        public IHttpActionResult PatchMedicationPrice(int id, JsonPatchDocument<Medication> patchData)
        {
            var objectToUpdate = db.Medications.Find(id);
            patchData.ApplyTo(objectToUpdate);

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



        // GET: api/Medications/5
        [Authorize]
        [ResponseType(typeof(MedicationDto))]
        public IHttpActionResult GetMedication(int id)
        {
            var medication = db.Medications.Where(x => x.MedicationId == id).
                Select(x => new MedicationDto
                {
                    MedicationId = x.MedicationId,
                    Description = x.Description,
                    Price = x.Price
                }).FirstOrDefault();

            if (medication == null)
            {
                return NotFound();
            }
            return Ok(medication);
        }

        // POST: api/Medications
        [Route("")]
        [Authorize(Roles =Roles.Administrator)]
        [ResponseType(typeof(Medication))]
        public IHttpActionResult PostMedication(Medication medication)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                db.Medications.Add(medication);
                db.SaveChanges();
            }catch(Exception)
            {
                return InternalServerError();
            }
            
            return Ok(medication);
        }

        // DELETE: api/Medications/5
       /* [ResponseType(typeof(Medication))]
        public IHttpActionResult DeleteMedication(int id)
        {
            Medication medication = db.Medications.Find(id);
            if (medication == null)
            {
                return NotFound();
            }

            db.Medications.Remove(medication);
            db.SaveChanges();

            return Ok(medication);
        }*/

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MedicationExists(int id)
        {
            return db.Medications.Count(e => e.MedicationId == id) > 0;
        }
    }
}