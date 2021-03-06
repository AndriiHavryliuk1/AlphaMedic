﻿using System.Data.Entity;
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
	[RoutePrefix("api/procedures")]
	public class ProceduresController : ApiController
	{
		private AlphaMedicContext db = new AlphaMedicContext();

		// GET: api/Procedures
		public IQueryable<Procedure> GetProcedures()
		{
			return db.Procedures;
		}

		//GET: api/procedures/5
		[Route("{id:int}")]
		[Authorize(Roles = Roles.Patient + "," + Roles.AllDoctors + "," + Roles.Receptionist)]
		public IHttpActionResult GetProcedure(int id)
		{
			var proc = db.Procedures.Find(id);

			var currentUser = db.Users.FirstOrDefault(x => x.Email == this.User.Identity.Name);

			if (currentUser == null)
			{
				return Content(HttpStatusCode.NotFound, Messages.UserNotFound);
			}

			if (this.User.IsInRole(Roles.Patient) && proc.MedicalHistory.Patient.UserId != currentUser.UserId)
			{
				return Content(HttpStatusCode.Forbidden, Messages.AccsesDenied);
			}

			if (Tools.AnyRole(this.User, Roles.DoctorRoles) && proc.Appointment.Doctor.UserId != currentUser.UserId)
			{
				return Content(HttpStatusCode.Forbidden, Messages.AccsesDenied);
			}

			switch (proc.GetType().BaseType.Name)
			{
				case "Examination":
					{
						return Ok(
							new ExaminationDto
							{
								ProcedureId = (int)proc.ProcedureId,
								Name = proc.Name,
								Description = proc.Description,
								Date = proc.Date,
								Price = proc.Price ?? default(decimal),
								Diagnosis = ((Examination)proc).Diagnosis,
								Doctor = proc.Appointment.Doctor != null ? new ShortUserDto
								{
									Name = proc.Appointment.Doctor.Name,
									Surname = proc.Appointment.Doctor.Surname,
									UserId = proc.Appointment.Doctor.UserId
								} : null
							}
							);
					}
				case "Vaccination":
					{
						return Ok(
							new VaccinationDto
							{
								ProcedureId = (int)proc.ProcedureId,
								Name = proc.Name,
								Description = proc.Description,
								Date = proc.Date,
								Price = proc.Price ?? default(decimal),
								Doctor = proc.Appointment.Doctor != null ? new ShortUserDto
								{
									Name = proc.Appointment.Doctor.Name,
									Surname = proc.Appointment.Doctor.Surname,
									UserId = proc.Appointment.Doctor.UserId
								} : null
							}
							);
					}
				case "Treatment":
					{
						return Ok(
							new TreatmentDto
							{
								ProcedureId = (int)proc.ProcedureId,
								Name = proc.Name,
								Description = proc.Description,
								Date = proc.Date,
								Price = proc.Price ?? default(decimal),
								Doctor = proc.Appointment.Doctor != null ? new ShortUserDto
								{
									Name = proc.Appointment.Doctor.Name,
									Surname = proc.Appointment.Doctor.Surname,
									UserId = proc.Appointment.Doctor.UserId
								} : null,
								Result = ((Treatment)proc).Result,
								Medications = ((Treatment)proc).Medications.Select(
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
			}
			return BadRequest();
		}

		// PUT: api/Procedures/5
		[ResponseType(typeof(void))]
		[Authorize(Roles = Roles.Receptionist)]
		public IHttpActionResult PutProcedure(int id, Procedure procedure)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (id != procedure.ProcedureId)
			{
				return BadRequest();
			}

			db.Entry(procedure).State = EntityState.Modified;

			try
			{
				db.SaveChanges();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ProcedureExists(id))
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

		// POST: api/Procedures
		[ResponseType(typeof(Procedure))]
		[Authorize(Roles = Roles.Receptionist)]
		public IHttpActionResult PostProcedure(Procedure procedure)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			db.Procedures.Add(procedure);

			try
			{
				db.SaveChanges();
			}
			catch (DbUpdateException)
			{
				if (ProcedureExists((int)procedure.ProcedureId))
				{
					return Conflict();
				}
				else
				{
					throw;
				}
			}

			return CreatedAtRoute("DefaultApi", new { id = procedure.ProcedureId }, procedure);
		}


		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}

		private bool ProcedureExists(int id)
		{
			return db.Procedures.Count(e => e.ProcedureId == id) > 0;
		}
	}
}