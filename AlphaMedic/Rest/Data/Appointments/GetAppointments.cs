using System;
using System.Collections.Generic;
using System.Linq;
using Rest.Core.Appointments;
using Rest.Dtos;
using Rest.Dtos.Pagginations;
using Rest.Models;
using Rest.Models.AlphaMedicContext;

namespace Rest.Data.Appointments
{
	public class GetAppointments : IGetAppointments
	{
		public List<AppointmentDto> GetAllAppointments()
		{
			using (var dbContext = new AlphaMedicContext())
			{
				return dbContext.Appointments.Select(MapAppointmentHellper).ToList();
			}
		}

		public AppointmentDto GetAppointment(int appointmentId)
		{
			using (var dbContext = new AlphaMedicContext())
			{
				return MapAppointmentHellper(dbContext.Appointments.Find(appointmentId));
			}
		}


		private AppointmentDto MapAppointmentHellper(Appointment appointment)
		{
			return new AppointmentDto
			{
				DoctorId = appointment.DoctorId,
				AppointmentId = appointment.AppointmentId,
				DepartmentId = appointment.Doctor.DepartmentId,
				Doctor = appointment.Doctor,
				DoctorFullName = appointment.Doctor.FullName,
				ProcedureName = appointment.Procedure.Name
			};
		}

		public List<AppointmentDto> GetAppointmentsWithPaginations(AppointmenPaginationsDto paginationsDto)
		{
			throw new NotImplementedException();
		}
	}
}