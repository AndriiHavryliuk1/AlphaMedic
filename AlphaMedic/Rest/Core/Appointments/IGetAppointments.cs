using System.Collections.Generic;
using Rest.Dtos;
using Rest.Dtos.Pagginations;

namespace Rest.Core.Appointments
{
	interface IGetAppointments
	{
		List<AppointmentDto> GetAllAppointments();
		List<AppointmentDto> GetAppointmentsWithPaginations(AppointmenPaginationsDto paginationsDto);
		AppointmentDto GetAppointment(int appointmentId);
	}
}
