using System.Collections.Generic;

namespace Rest.Models
{
	public struct Messages
	{
		public const string AccsesDenied = "Access denied";
		public const string AppointmentNotFound = "Appointment not found";
		public const string UserNotFound = "User not found";
		public const string BadDataInFields = "Some fields contains bad data";
	}


	public class Roles
	{
		public const string Administrator = "Administrator";
		public const string Doctor = "Doctor";
		public const string AllDoctors = "Doctor,HospitalDean,HeadDepartment";
		public const string HospitalDean = "HospitalDean";
		public const string DepartmentHead = "HeadDepartment";
		public const string Receptionist = "Receptionist";
		public const string Patient = "Patient";
		public static List<string> DoctorRoles = new List<string> { "Doctor", "HospitalDean", "HeadDepartment" };
	}

	public static class Constants
	{
		public const string ThisServer = @"http://localhost:63741/";
		public const string MyClient = @"http://127.0.0.1:8081/";
		public const string DefaultDoctorImage = @"img/docs/profileAvatar.jpg";
		public const string DefaultPatientImage = @"img/patients/profileAvatar.jpg";
		public const string DbConnetionString = "Data Source=DESKTOP-4KHTQ74;Initial Catalog=ALPHA_AlphaMedic;Integrated Security=True";
	}
}