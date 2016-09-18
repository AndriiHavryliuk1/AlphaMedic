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

namespace Rest.Controllers
{
    [RoutePrefix("api/medicalhistory")]
    public class MedicalHistoriesController : ApiController
    {
        private AlphaMedicContext db = new AlphaMedicContext();


        // GET: api/Medicalhistory/5  
        [Authorize]
        [Route("{id:int}")]
        public IHttpActionResult GetMedicalHistory(int id, DateTime? periodFrom, DateTime? periodTill,
            string procedure = null, int page = 1, int itemsPerPage = 15, string search = null)
        {

            var currentUser = db.Users.FirstOrDefault(x => x.Email == this.User.Identity.Name);

            MedicalHistory medicalHistory = db.MedicalHistorys.Find(id);

            if (medicalHistory == null)
            {
                return NotFound();
            }

            if (Tools.AnyRole(this.User,Roles.DoctorRoles) && !medicalHistory.Patient.Appointments.Any(x => x.DoctorId == currentUser.UserId))
            {
                return Content(HttpStatusCode.Forbidden, Messages.AccsesDenied);
            }

            if(this.User.IsInRole(Roles.Patinet)  && medicalHistory.Patient.UserId != currentUser.UserId)
            {
                return Content(HttpStatusCode.Forbidden, Messages.AccsesDenied);
            }

            
            #region filterDate
            if (procedure != null)
            {
                medicalHistory.Procedures = medicalHistory.Procedures.Where(x => x.GetType().BaseType.Name == procedure).ToList<Procedure>();
            }

            if (periodFrom != null && periodTill != null)
            {
                medicalHistory.Procedures = medicalHistory.Procedures.Where(x => x.Date >= periodFrom && x.Date <= periodTill).ToList();
            }
            if (periodFrom != null)
            {
                medicalHistory.Procedures = medicalHistory.Procedures.Where(x => x.Date >= periodFrom).ToList();
            }

            if (periodTill != null)
            {
                medicalHistory.Procedures = medicalHistory.Procedures.Where(x => x.Date <= periodTill).ToList();
            }
            #endregion

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                medicalHistory.Procedures = medicalHistory.Procedures.Where(x =>
                    x.Name.ToLower().Contains(search)).ToList();
            }


            IQueryable<ProcedureDto> Examinations = medicalHistory.Procedures.Where(x => x.GetType().BaseType.Name == "Examination")
                .Select(x => new ExaminationDto
                {
                    ProcedureId = (int)x.ProcedureId,
                    Name = x.Name,
                    Description = x.Description,
                    Date = x.Date,
                    Doctor = new ShortUserDto
                    {
                        UserId = x.Appointment.Doctor.UserId,
                        Name = x.Appointment.Doctor.Name,
                        Surname = x.Appointment.Doctor.Surname
                    },
                    Diagnosis = ((Examination)x).Diagnosis
                }).AsQueryable();

            IQueryable<ProcedureDto> Treatments = medicalHistory.Procedures.Where(x => x.GetType().BaseType.Name == "Treatment")
                .Select(x => new TreatmentDto
                {
                    ProcedureId = (int)x.ProcedureId,
                    Name = x.Name,
                    Description = x.Description,
                    Date = x.Date,
                    Doctor = new ShortUserDto
                    {
                        UserId = x.Appointment.Doctor.UserId,
                        Name = x.Appointment.Doctor.Name,
                        Surname = x.Appointment.Doctor.Surname
                    },
                  //Medications = ((Treatment)x).Medications.ToArray()
                        }).AsQueryable();

            IQueryable<ProcedureDto> Vaccinations = medicalHistory.Procedures.Where(x => x.GetType().BaseType.Name == "Vaccination")
                .Select(x => new VaccionationDto
                {
                    ProcedureId = (int)x.ProcedureId,
                    Name = x.Name,
                    Description = x.Description,
                    Date = x.Date,
                    Doctor = new ShortUserDto
                    {
                        UserId = x.Appointment.Doctor.UserId,
                        Name = x.Appointment.Doctor.Name,
                        Surname = x.Appointment.Doctor.Surname
                    }
                }).AsQueryable();

            var rez = new
            {
                WarningLabels = medicalHistory.WarningLabels.Select(x => x.Description),
                Procedures = (Examinations.Concat(Treatments).Concat(Vaccinations)).OrderByDescending(x => x.Date).Where(x => x.Date < DateTime.Now)
            };

            // paging
            var usersPaged = rez.Procedures.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

            // json result
            var json = new
            {
                count = rez.Procedures.Count(),
                data = new
                {
                    WarningLabels = rez.WarningLabels,
                    Procedures = usersPaged
                }
            };

            return Ok(json);
        }

       /* // PUT: api/MedicalHistories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMedicalHistory(int id, MedicalHistory medicalHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != medicalHistory.MedicalHistoryId)
            {
                return BadRequest();
            }

            db.Entry(medicalHistory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicalHistoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }*/
/*
        // POST: api/MedicalHistories
        [ResponseType(typeof(MedicalHistory))]
        public IHttpActionResult PostMedicalHistory(MedicalHistory medicalHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MedicalHistorys.Add(medicalHistory);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = medicalHistory.MedicalHistoryId }, medicalHistory);
        }
        */

        // DELETE: api/MedicalHistories/5
        /*  [ResponseType(typeof(MedicalHistory))]
          public IHttpActionResult DeleteMedicalHistory(int id)
          {
              MedicalHistory medicalHistory = db.MedicalHistorys.Find(id);
              if (medicalHistory == null)
              {
                  return NotFound();
              }

              db.MedicalHistorys.Remove(medicalHistory);
              db.SaveChanges();

              return Ok(medicalHistory);
          }
          */
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MedicalHistoryExists(int id)
        {
            return db.MedicalHistorys.Count(e => e.MedicalHistoryId == id) > 0;
        }
    }
}