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
	public class WarningLabelsController : ApiController
	{
		private AlphaMedicContext db = new AlphaMedicContext();

		// GET: api/WarningLabels
		public IQueryable<WarningLabel> GetWarningLabels()
		{
			return db.WarningLabels;
		}

		// GET: api/WarningLabels/5
		[Authorize]
		[ResponseType(typeof(WarningLabel))]
		public IHttpActionResult GetWarningLabel(int id)
		{
			var currentUser = db.Users.FirstOrDefault(x => x.Email == this.User.Identity.Name);

			if (currentUser == null)
			{
				return NotFound();
			}

			WarningLabel warningLabel = db.WarningLabels.Find(id);
			if (warningLabel == null)
			{
				return NotFound();
			}

			if (Tools.AnyRole(this.User, Roles.DoctorRoles) && !warningLabel.MedicalHistory.Patient.Appointments.Any(x => x.DoctorId == currentUser.UserId))
			{
				return Content(HttpStatusCode.Forbidden, Messages.AccsesDenied);
			}
			if (this.User.IsInRole(Roles.Patient) && warningLabel.MedicalHistory.Patient.UserId != currentUser.UserId)
			{
				return Content(HttpStatusCode.Forbidden, Messages.AccsesDenied);
			}

			return Ok(warningLabel);
		}

		// PUT: api/WarningLabels/5
		[Authorize(Roles = Roles.AllDoctors)]
		[ResponseType(typeof(void))]
		public IHttpActionResult PutWarningLabel(int id, WarningLabel warningLabel)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (id != warningLabel.WarningLabelId)
			{
				return BadRequest();
			}



			db.Entry(warningLabel).State = EntityState.Modified;

			try
			{
				db.SaveChanges();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!WarningLabelExists(id))
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

		// POST: api/WarningLabels
		[Authorize(Roles = Roles.AllDoctors)]

		public IHttpActionResult PostWarningLabel(WarningLabel warningLabel)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var currentUser = db.Users.FirstOrDefault(x => x.Email == this.User.Identity.Name);
			var medHist = db.MedicalHistorys.Find(warningLabel.MedicalHistoryId);

			if (currentUser == null)
			{
				return NotFound();
			}
			if (medHist == null || !medHist.Patient.Appointments.Any(x => x.DoctorId == currentUser.UserId))
			{
				return Content(HttpStatusCode.Forbidden, Messages.AccsesDenied);
			}

			db.WarningLabels.Add(warningLabel);
			db.SaveChanges();

			return Ok(new { warningLabel.Description, warningLabel.WarningLabelId, warningLabel.MedicalHistoryId });

		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}

		private bool WarningLabelExists(int id)
		{
			return db.WarningLabels.Count(e => e.WarningLabelId == id) > 0;
		}
	}
}