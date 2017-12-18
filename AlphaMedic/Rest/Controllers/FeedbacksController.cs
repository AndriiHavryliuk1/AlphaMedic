using System;
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
	public class FeedbacksController : ApiController
	{
		private AlphaMedicContext db = new AlphaMedicContext();

		// GET: api/Feedbacks
		public IHttpActionResult GetFeedbacks([FromUri] int? DepartmentId = null, [FromUri] int? DoctorId = null, [FromUri] bool all = true)
		{

			var feedbacks = db.Feedbacks;
			if (DepartmentId != null)
			{
				var res = (feedbacks.Where(x => x.DepartmentId == DepartmentId).Select(y => new
				{
					y.Date,
					y.Description,
					y.DoctorId,
					y.DepartmentId,
					y.FeedbackId,
					PatientFullName = (y.Patient == null ? "Anonymous" : y.Patient.Name + " " + y.Patient.Surname),
					PatientURLImage = (y.Patient == null ? Constants.ThisServer + Constants.DefaultPatientImage : Constants.ThisServer + y.Patient.URLImage)
				}));
				res = all == true ? res : res.OrderBy(x => x.FeedbackId).Skip(Math.Max(0, res.Count() - 3));
				return Ok(res);
			}
			if (DoctorId != null)
			{
				var res = (feedbacks.Where(x => x.DoctorId == DoctorId).Select(y => new
				{
					y.Date,
					y.Description,
					y.DoctorId,
					y.DepartmentId,
					y.FeedbackId,
					PatientFullName = (y.Patient == null ? "Anonymous" : y.Patient.Name + " " + y.Patient.Surname),
					PatientURLImage = (y.Patient == null ? Constants.ThisServer + Constants.DefaultPatientImage : Constants.ThisServer + y.Patient.URLImage)
				}));

				res = all == true ? res : res.OrderBy(x => x.FeedbackId).Skip(Math.Max(0, res.Count() - 3));
				return Ok(res);
			}

			return BadRequest();
		}

		// GET: api/Feedbacks/5
		[ResponseType(typeof(Feedback))]
		public IHttpActionResult GetFeedback(int id)
		{
			Feedback feedback = db.Feedbacks.Find(id);
			if (feedback == null)
			{
				return NotFound();
			}

			return Ok(feedback);
		}

		// PUT: api/Feedbacks/5
		[ResponseType(typeof(void))]
		public IHttpActionResult PutFeedback(int id, Feedback feedback)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (id != feedback.FeedbackId)
			{
				return BadRequest();
			}

			db.Entry(feedback).State = EntityState.Modified;

			try
			{
				db.SaveChanges();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!FeedbackExists(id))
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

		// POST: api/Feedbacks
		[ResponseType(typeof(Feedback))]
		public IHttpActionResult PostFeedback(Feedback feedback)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			db.Feedbacks.Add(feedback);
			db.SaveChanges();

			return CreatedAtRoute("DefaultApi", new { id = feedback.FeedbackId }, feedback);
		}

		// DELETE: api/Feedbacks/5
		[ResponseType(typeof(Feedback))]
		public IHttpActionResult DeleteFeedback(int id)
		{
			Feedback feedback = db.Feedbacks.Find(id);
			if (feedback == null)
			{
				return NotFound();
			}

			db.Feedbacks.Remove(feedback);
			db.SaveChanges();

			return Ok(feedback);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}

		private bool FeedbackExists(int id)
		{
			return db.Feedbacks.Count(e => e.FeedbackId == id) > 0;
		}
	}
}