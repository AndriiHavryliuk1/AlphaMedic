using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using Rest.Models;
using Rest.Models.AlphaMedicContext;
using Rest.Dtos;

namespace Rest.Controllers
{
	[RoutePrefix("api/medicalhistory")]
	public class MedicalHistoriesController : ApiController
	{
		private readonly AlphaMedicContext db = new AlphaMedicContext();


		[Authorize]
		[Route("{id:int}")]
		public IHttpActionResult GetMedicalHistory(int id, DateTime? periodFrom, DateTime? periodTill,
			string procedure = null, int page = 1, int itemsPerPage = 15, string search = null)
		{

			var medicalHistory = db.MedicalHistorys.Find(id);

			if (medicalHistory == null)
				return NotFound();

			var currentUser = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
			if (currentUser == null)
				return NotFound();

			if (User.IsInRole(Roles.Patient) && medicalHistory.Patient.UserId != currentUser.UserId)
				return Content(HttpStatusCode.Forbidden, Messages.AccsesDenied);

			if (Tools.AnyRole(User, Roles.DoctorRoles))
			{
				if (User.IsInRole(Roles.Doctor) && medicalHistory.Patient.Appointments.All(x => x.DoctorId != currentUser.UserId))
					return Content(HttpStatusCode.Forbidden, Messages.AccsesDenied);

				var department = db.Doctors.FirstOrDefault(x => x.Email == this.User.Identity.Name).DepartmentId;
				if (department == default(int))
					return NotFound();

				if (User.IsInRole(Roles.DepartmentHead) &&
				medicalHistory.Patient.Appointments.All(d => d.Doctor.DepartmentId != department))
					return Content(HttpStatusCode.Forbidden, Messages.AccsesDenied);
			}

			#region filterDate
			if (procedure != null)
			{
				medicalHistory.Procedures = medicalHistory.Procedures.Where(x => x.GetType().BaseType.Name == procedure).ToArray();
			}

			if (periodFrom != null && periodTill != null)
			{
				medicalHistory.Procedures = medicalHistory.Procedures.Where(x => x.Date >= periodFrom && x.Date <= periodTill).ToArray();
			}
			if (periodFrom != null)
			{
				medicalHistory.Procedures = medicalHistory.Procedures.Where(x => x.Date >= periodFrom).ToArray();
			}

			if (periodTill != null)
			{
				medicalHistory.Procedures = medicalHistory.Procedures.Where(x => x.Date <= periodTill).ToArray();
			}
			#endregion

			if (!string.IsNullOrWhiteSpace(search))
			{
				search = search.ToLower();
				medicalHistory.Procedures = medicalHistory.Procedures.Where(x =>
					x.Name.ToLower().Contains(search)).ToArray();
			}


			IQueryable<ProcedureDto> examinations = medicalHistory.Procedures.Where(x => x.GetType().BaseType.Name == "Examination")
				.Select(x => new ExaminationDto
				{
					ProcedureId = (int)x.ProcedureId,
					Name = x.Name,
					Description = x.Description,
					Date = x.Date,
					Price = x.Price,
					State = x.Appointment.State,
					Doctor = x.Appointment.Doctor != null ? new ShortUserDto
					{
						UserId = x.Appointment.Doctor.UserId,
						Name = x.Appointment.Doctor.Name,
						Surname = x.Appointment.Doctor.Surname
					} : null,
					Diagnosis = ((Examination)x).Diagnosis
				}).AsQueryable();

			IQueryable<ProcedureDto> Treatments = medicalHistory.Procedures.Where(x => x.GetType().BaseType.Name == "Treatment")
				.Select(x => new TreatmentDto
				{
					ProcedureId = (int)x.ProcedureId,
					Name = x.Name,
					Description = x.Description,
					Date = x.Date,
					Price = x.Price,
					State = x.Appointment.State,
					Doctor = x.Appointment.Doctor != null ? new ShortUserDto
					{
						UserId = x.Appointment.Doctor.UserId,
						Name = x.Appointment.Doctor.Name,
						Surname = x.Appointment.Doctor.Surname
					} : null,
					Result = ((Treatment)x).Result,
					Medications = ((Treatment)x).Medications.Select(
					  y => new MedicationDto
					  {
						  MedicationId = y.MedicationId,
						  Description = y.Description,
						  Price = y.Price
					  })
				}).AsQueryable();

			IQueryable<ProcedureDto> vaccinations = medicalHistory.Procedures.Where(x => x.GetType().BaseType.Name == "Vaccination")
				.Select(x => new VaccinationDto
				{
					ProcedureId = (int)x.ProcedureId,
					Name = x.Name,
					Description = x.Description,
					Date = x.Date,
					Price = x.Price,
					State = x.Appointment.State,
					Doctor = x.Appointment.Doctor != null ? new ShortUserDto
					{
						UserId = x.Appointment.Doctor.UserId,
						Name = x.Appointment.Doctor.Name,
						Surname = x.Appointment.Doctor.Surname
					} : null
				}).AsQueryable();

			var rez = new
			{
				WarningLabels = medicalHistory.WarningLabels.Select(x => x.Description),
				Procedures = (examinations.Concat(Treatments).Concat(vaccinations)).OrderByDescending(x => x.Date).Where(x => x.State == AppointmentState.Finished)//x.Date < DateTime.Now)
			};

			// paging
			var usersPaged = rez.Procedures.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

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


		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}