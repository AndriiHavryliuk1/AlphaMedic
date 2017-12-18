using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Rest.Models;
using Rest.Models.AlphaMedicContext;
using Rest.Dtos;

namespace Rest.Controllers
{

	[RoutePrefix("api/treatments")]
	public class TreatmentsController : ApiController
	{
		private AlphaMedicContext db = new AlphaMedicContext();

		// GET: api/Treatments
		[Route("")]
		public IQueryable<Treatment> GetProcedures()
		{
			return db.Treatments;
		}

		// GET: api/Treatments/5
		[ResponseType(typeof(TreatmentDto))]
		[Route("{id:int}")]
		[Authorize(Roles = Roles.Receptionist + "," + Roles.AllDoctors + "," + Roles.Patient)]
		public IHttpActionResult GetTreatment(int id)
		{
			Treatment treatment = db.Treatments.Include(x => x.Medications).Where(c => c.ProcedureId == id).FirstOrDefault();
			if (treatment == null)
			{
				return NotFound();
			}

			return Ok(new TreatmentDto
			{
				ProcedureId = (int)treatment.ProcedureId,
				Name = treatment.Name,
				Description = treatment.Description,
				Date = treatment.Date,
				Price = (decimal)treatment.Price,
				Doctor = new ShortUserDto
				{
					UserId = treatment.Appointment.Doctor.UserId,
					Name = treatment.Appointment.Doctor.Name,
					Surname = treatment.Appointment.Doctor.Surname
				},
				Result = treatment.Result,
				Medications = treatment.Medications.Select(
									x => new MedicationDto
									{
										MedicationId = x.MedicationId,
										Description = x.Description,
										Price = x.Price
									}
									)
			}
			);
		}

		// PUT: api/Treatments/5
		[ResponseType(typeof(void))]
		[Route("{id:int}")]
		[Authorize(Roles = Roles.Receptionist + "," + Roles.AllDoctors)]
		public IHttpActionResult PutTreatment(int id, Treatment treatment)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var tre = db.Treatments.Find(id);
			if (tre.Result == null && treatment.Result != null)
			{
				tre.Result = treatment.Result;
				db.Entry(tre).State = EntityState.Modified;
			}
			else
			{
				tre.Medications = treatment.Medications;

				var ids = (from n in tre.Medications
						   select n.MedicationId).ToArray<int>();
				List<Medication> meds = (from m in db.Medications.Include(x => x.Treatments)
										 from id1 in ids
										 where m.MedicationId == id1
										 select m
						   ).ToList();
				tre.Medications = meds;
				foreach (Medication med in meds)
					med.Treatments.Add(tre);
				db.Entry(tre).State = EntityState.Modified;
				foreach (Medication med in meds)
					db.Entry(med).State = EntityState.Modified;
			}
			try
			{
				db.SaveChanges();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!TreatmentExists(id))
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

		// POST: api/Treatments
		[ResponseType(typeof(Treatment))]
		[Route("")]
		[Authorize(Roles = Roles.Receptionist + "," + Roles.AllDoctors)]
		public IHttpActionResult PostTreatment(Treatment treatment)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			//check if procedure already set
			if (db.Procedures.FirstOrDefault(x => x.ProcedureId == treatment.ProcedureId) != null)
				return Content(HttpStatusCode.Conflict, "Procedure existed!");

			db.Procedures.Add(treatment);

			try
			{
				db.SaveChanges();

			}
			catch (DbUpdateException)
			{
				if (TreatmentExists((int)treatment.ProcedureId))
				{
					return Conflict();
				}
				else
				{
					throw;
				}
			}

			return Ok(treatment);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}

		private bool TreatmentExists(int id)
		{
			return db.Procedures.Count(e => e.ProcedureId == id) > 0;
		}
	}
}