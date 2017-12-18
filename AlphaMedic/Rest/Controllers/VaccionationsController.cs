using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Rest.Models;
using Rest.Models.AlphaMedicContext;

namespace Rest.Controllers
{
	public class VaccinationsController : ApiController
	{
		private AlphaMedicContext db = new AlphaMedicContext();

		// GET: api/Vaccinations
		public IQueryable GetProcedures()
		{
			return db.Procedures;
		}

		// GET: api/Vaccinations/5
		[Authorize]
		[ResponseType(typeof(Vaccination))]
		public IHttpActionResult GetVaccination(int id)
		{
			var currentUser = db.Users.FirstOrDefault(x => x.Email == this.User.Identity.Name);


			Vaccination vaccination = (Vaccination)db.Procedures.Find(id);

			if (this.User.IsInRole(Roles.Patient) && vaccination.Appointment.PatientId != currentUser.UserId)
			{
				return Content(HttpStatusCode.Forbidden, Messages.AccsesDenied);
			}

			if (vaccination == null)
			{
				return NotFound();
			}

			return Ok(vaccination);
		}

		// PUT: api/Vaccinations/5
		[Authorize(Roles = Roles.AllDoctors)]
		[ResponseType(typeof(void))]
		public IHttpActionResult PutVaccination(int id, Vaccination vaccination)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (id != vaccination.ProcedureId)
			{
				return BadRequest();
			}

			db.Entry(vaccination).State = EntityState.Modified;

			try
			{
				db.SaveChanges();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!VaccinationExists(id))
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

		// POST: api/Vaccinations
		[Authorize(Roles = Roles.AllDoctors)]
		[ResponseType(typeof(Vaccination))]
		public IHttpActionResult PostVaccination(Vaccination vaccination)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}


			//check if procedure already set
			if (db.Procedures.FirstOrDefault(x => x.ProcedureId == vaccination.ProcedureId) != null)
				return Content(HttpStatusCode.Conflict, "Procedure existed!");


			db.Procedures.Add(vaccination);

			try
			{
				db.SaveChanges();

			}
			catch (DbUpdateException)
			{
				if (VaccinationExists((int)vaccination.ProcedureId))
				{
					return Conflict();
				}
				else
				{
					throw;
				}
			}

			return CreatedAtRoute("DefaultApi", new { id = vaccination.ProcedureId }, vaccination);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}

		private bool VaccinationExists(int id)
		{
			return db.Procedures.Count(e => e.ProcedureId == id) > 0;
		}
	}
}