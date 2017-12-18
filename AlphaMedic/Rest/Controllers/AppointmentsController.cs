using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Rest.Data.Appointments;
using Rest.Models;
using Rest.Models.AlphaMedicContext;
using Rest.Dtos;

namespace Rest.Controllers
{
	[RoutePrefix("api/appointments")]
	public class AppointmentsController : ApiController
	{
		private readonly AlphaMedicContext db = new AlphaMedicContext();



		[Route("")]
		[Authorize(Roles = Roles.Receptionist + "," + Roles.AllDoctors)]
		public IHttpActionResult GetAppointments(DateTime? periodFrom, DateTime? periodTill,
			AppointmentState? state = null, int page = 1, int itemsPerPage = 15, int? doctor = null, int? department = null)
		{
			var list = db.Appointments.Where(x => x.State != AppointmentState.Finished).ToArray();

			#region Filterdate
			if (doctor != null)
			{
				list = list.Where(x => x.DoctorId == doctor).ToArray();
			}

			if (department != null)
			{
				list = list.Where(x => x.Doctor != null && x.Doctor.DepartmentId == department).ToArray();
			}

			if (periodFrom != null && periodTill != null)
			{
				list = list.Where(x => x.Date >= periodFrom && x.Date <= periodTill).ToArray();
			}

			if (periodFrom != null)
			{
				list = list.Where(x => x.Date >= periodFrom).ToArray();
			}

			if (periodTill != null)
			{
				list = list.Where(x => x.Date <= periodTill).ToArray();
			}

			if (state != null)
			{
				list = list.Where(x => x.State == state).ToArray();
			}
			#endregion

			var f = list.Where(x => x.Checked == false).Select(x => new
			{
				x.AppointmentId,
				x.DoctorId,
				Doctor = new { Name = x.Doctor?.Name, Surname = x.Doctor?.Surname },
				x.Date,
				x.Duration,
				State = x.State.ToString(),
			}).OrderByDescending(x => x.AppointmentId).ToArray();

			var a = list.Where(x => x.Date == null).Select(x => new
			{
				x.AppointmentId,
				x.DoctorId,
				Doctor = new { Name = x.Doctor?.Name, Surname = x.Doctor?.Surname },
				x.Date,
				x.Duration,
				State = x.State.ToString()
			}).ToArray();


			var b = list.Where(x => x.Date != null).Select(x => new
			{
				x.AppointmentId,
				x.DoctorId,
				Doctor = new { Name = x.Doctor?.Name, Surname = x.Doctor?.Surname },
				x.Date,
				x.Duration,
				State = x.State.ToString()
			}).OrderBy(x => x.Date).ToArray();
			var c = (f.Concat(a)).Concat(b).Distinct();

			// paging
			var usersPaged = c.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
			var json = new
			{
				count = c.Count(),
				data = usersPaged
			};

			return Ok(json);
		}



		[Route("{id:int}")]
		[Authorize(Roles = Roles.AllDoctors + "," + Roles.Patient + "," + Roles.Receptionist)]
		[ResponseType(typeof(Appointment))]
		public IHttpActionResult GetAppointment(int id)
		{

			var currentUser = db.Users.FirstOrDefault(x => x.Email == this.User.Identity.Name);

			if (currentUser == null)
			{
				return Content(HttpStatusCode.NotFound, Messages.UserNotFound);
			}

			var appointment = db.Appointments.FirstOrDefault(x => x.AppointmentId == id);

			if (appointment == null)
			{
				return Content(HttpStatusCode.NotFound, Messages.AppointmentNotFound);
			}

			if (this.User.IsInRole(Roles.Patient) && appointment.PatientId != currentUser.UserId)
			{
				return Content(HttpStatusCode.Forbidden, Messages.AccsesDenied);
			}

			if (Tools.AnyRole(this.User, Roles.DoctorRoles) && appointment.Doctor.UserId != currentUser.UserId)
			{
				return Content(HttpStatusCode.Forbidden, Messages.AccsesDenied);
			}

			try
			{

				var a = new
				{
					DoctorFullName = appointment.Doctor != null ? appointment.Doctor.Name + " " + appointment.Doctor.Surname : null,
					DoctorURLImage = appointment.Doctor != null ? Constants.ThisServer + appointment.Doctor.URLImage : null,
					DoctorId = appointment.DoctorId,
					PatientId = appointment.PatientId,
					PatientFullName = appointment.Patient.Name + " " + appointment.Patient.Surname,
					PatientURLImage = Constants.ThisServer + appointment.Patient.URLImage,
					Date = appointment.Date,
					Description = appointment.Description,
					Duration = appointment.Duration,
					State = appointment.State.ToString(),
					ProcedureType = appointment.Procedure == null ? null : appointment.Procedure.GetType().BaseType.Name,
					Checked = appointment.Checked
				};
				return Ok(a);
			}
			catch (Exception ex)
			{
				return InternalServerError(ex);
			}




		}

		[HttpPut]
		[Authorize(Roles = Roles.Receptionist + "," + Roles.AllDoctors)]
		[Route("{id:int}")]
		[ResponseType(typeof(void))]
		public IHttpActionResult UpdateAndConfirmAppointment(int id, Appointment appointment)
		{
			var currentUser = db.Users.FirstOrDefault(x => x.Email == this.User.Identity.Name);
			if (appointment.DoctorId != currentUser.UserId && Tools.AnyRole(this.User, Roles.DoctorRoles))
			{
				return Content(HttpStatusCode.Forbidden, Messages.AccsesDenied);
			}

			appointment.AppointmentId = id;

			db.Entry(appointment).State = EntityState.Modified;

			try
			{
				db.SaveChanges();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!AppointmentExists(id))
				{
					return NotFound();
				}
				throw;
			}

			return StatusCode(HttpStatusCode.NoContent);
		}


		[HttpPost]
		[Route("checkdateforapp")]
		[ResponseType(typeof(void))]
		public IHttpActionResult CheckDateForAppointment(Appointment appointment)
		{
			var appoints = db.Appointments.Where(x => x.DoctorId == appointment.DoctorId && x.Date >= DateTime.Now).ToArray();

			if (appointment.Date == null || appointment.Duration == null) return Ok();
			foreach (var list in appoints)
			{
				if (list.Date == null || list.Duration == null) continue;
				var tmp = list.Date + list.Duration;
				var enterApp = appointment.Date + appointment.Duration;
				if ((appointment.Date == list.Date) ||
					(enterApp == tmp) ||
					((appointment.Date) >= list.Date && (appointment.Date) <= tmp) ||
					((appointment.Date) >= list.Date && enterApp <= tmp) ||
					(appointment.Date <= list.Date && enterApp >= list.Date) ||
					(appointment.Date <= list.Date && enterApp >= tmp))
				{
					return NotFound();
				}
			}
			return Ok();

		}


		[HttpPost]
		[Route("{id:int}/sendmail")]
		[ResponseType(typeof(void))]
		public IHttpActionResult SendEmail(int id, EmailPostDto emailPostDto)
		{
			if (emailPostDto.EmailState == EmailPostState.delete && (emailPostDto.appointment == null || emailPostDto.appointment.Date < DateTime.Now))
			{
				return BadRequest();
			}

			EmailInput emailInput = new EmailInput();
			var pat = db.Patients.Find(emailPostDto.appointment.PatientId);
			emailInput.UserName = pat.FullName;
			emailInput.Email = pat.Email;

			switch (emailPostDto.EmailState)
			{
				case EmailPostState.confirm:
					{
						emailInput.Subject = "Confirm appointment!";
						emailInput.Body =
							$"Hello! \nYou reserved to apointment on {emailPostDto.appointment.Date}\nYour doctor:{db.Doctors.Find(emailPostDto.appointment.DoctorId).FullName}\nPlease delete appointment and register one more if It`s date doesn`t suitable for you\nBest regard,\nAlphaMedic";
						break;
					}

				case EmailPostState.delete:
					{
						emailInput.Subject = "Deleted appointment!";
						emailInput.Body = "Hello! \nI am sorry but you deleted from your appointment:\nAppointment date:" + emailPostDto.appointment.Date +
				 "\nAppointment symptoms:" + emailPostDto.appointment.Description +
				 "\nbecause It has already recerved. Please register for anouther" +
				 " appointment and choose anouther date.\nBest regard,\nAlphaMedic";
						break;
					}

				case EmailPostState.change:
					{
						emailInput.Subject = "Change appointment!";
						emailInput.Body = "Hello! \nWe changed your appointment date because doctor busies on this date " + emailPostDto.appointment.Date + "\nYour doctor:" +
						db.Doctors.Find(emailPostDto.appointment.DoctorId)?.FullName + "\nPlease delete appointment and register one more if It`s date doesn`t suitable for you\nBest regard,\nAlphaMedic";
						break;
					}

			}
			try
			{
				// EMailHelper.SendNotification(emailInput);
			}
			catch (Exception)
			{
				return BadRequest();
			}

			return Ok();

		}

		//GET: api/schedule/1
		[Route("~/api/schedule/{id:int}")]
		public IQueryable GetDoctorAppointmentsAsEvents(int id)
		{

			var a = db.Appointments.Where(x => x.State == AppointmentState.Accepted && x.Doctor.ScheduleId == id && x.Date != null && (bool)x.Patient.Active).ToArray();
			var b = a.Select(e => new
			{
				url = "#/appointmentInfo/" + e.AppointmentId.ToString(),
				start = e.Date,
				finish = e.Date + e.Duration,
				title = e.Patient.Name + " " + e.Patient.Surname
			}
		   );


			return b.AsQueryable();
		}


		[Route("")]
		[ResponseType(typeof(Appointment))]
		[Authorize(Roles = Roles.Receptionist + "," + Roles.Patient + "," + Roles.AllDoctors)]
		public IHttpActionResult PostAppointment(Appointment appointment)
		{
			if (db.Employees.Any(x => x.UserId == appointment.PatientId))
			{
				return BadRequest("Employee can't register for appointment");
			}

			if (appointment.Date < DateTime.UtcNow)
			{
				return BadRequest("Wrong date");
			}

			appointment.Doctor = db.Doctors.Include(c => c.Schedule).
				   FirstOrDefault(d => d.UserId == appointment.DoctorId);
			appointment.Patient = db.Patients.Include(c => c.MedicalHistory).
				FirstOrDefault(p => p.UserId == appointment.PatientId);

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			db.Appointments.Add(appointment);
			db.SaveChanges();

			return Ok(new
			{
				Id = appointment.AppointmentId,
				Date = appointment.Date
			});

		}

		// DELETE: api/Appointments/5
		[Route("{id:int}")]
		[ResponseType(typeof(Appointment))]
		[Authorize(Roles = Roles.Receptionist + "," + Roles.Patient)]
		public IHttpActionResult DeleteAppointment(int id)
		{
			var appointment = db.Appointments.Find(id);
			if (appointment == null)
			{
				return NotFound();
			}
			var proc = db.Procedures.Find(id);
			try
			{
				if (proc != null)
				{
					db.Procedures.Remove(proc);
					db.SaveChanges();
				}
				db.Appointments.Remove(appointment);
				db.SaveChanges();
			}
			catch (Exception)
			{
				return NotFound();
			}
			return Ok(appointment);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}

		private bool AppointmentExists(int id)
		{
			return db.Appointments.Count(e => e.AppointmentId == id) > 0;
		}

		private object CheckForPermissions(int id)
		{
			var currentUser = db.Users.FirstOrDefault(x => x.Email == this.User.Identity.Name);

			if (currentUser == null)
			{
				return Content(HttpStatusCode.NotFound, Messages.UserNotFound);
			}

			var appointment = db.Appointments.FirstOrDefault(x => x.AppointmentId == id);

			if (appointment == null)
			{
				return Content(HttpStatusCode.NotFound, Messages.AppointmentNotFound);
			}

			if (this.User.IsInRole(Roles.Patient) && appointment.PatientId != currentUser.UserId)
			{
				return Content(HttpStatusCode.Forbidden, Messages.AccsesDenied);
			}

			if (Tools.AnyRole(this.User, Roles.DoctorRoles) && appointment.Doctor.UserId != currentUser.UserId)
			{
				return Content(HttpStatusCode.Forbidden, Messages.AccsesDenied);
			}

			return true;
		}
	}
}