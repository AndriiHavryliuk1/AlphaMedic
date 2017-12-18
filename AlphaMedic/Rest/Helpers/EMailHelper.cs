using Rest.Models;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace Rest.Helpers
{
	public class EMailHelper
	{
		private const string Email = "extraair@ukr.net";
		private const string fromPassword = "ExtraAir123";

		public static void SendNotification(EmailInput emailInput)
		{
			var fromAddress = new MailAddress(Email, "Extra Air");
			var toAddress = new MailAddress(emailInput.Email, emailInput.UserName);

			var smtp = new SmtpClient
			{
				Host = "smtp.ukr.net",
				Port = 465,
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
			};

			using (var message = new MailMessage(fromAddress, toAddress)
			{
				Subject = emailInput.Subject,
				Body = emailInput.Body,
			})
			{
				smtp.Send(message);
				smtp.Dispose();
			}
		}


		public static void SendConfirmRegisterNotification(EmailInput emailInput, int idUser)
		{
			var fromAddress = new MailAddress(Email, "Extra Air");
			var toAddress = new MailAddress(emailInput.Email, emailInput.UserName);
			var html = "Please confirm your account by clicking this link: <a href=\"" + Constants.MyClient
				+ "#/confirmRegistration/" + idUser + "\">link</a><br/>";

			var smtp = new SmtpClient
			{
				Host = "smtp.ukr.net",
				Port = 465,
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
			};

			var message = new MailMessage(fromAddress, toAddress);
			message.Subject = emailInput.Subject;
			message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

			smtp.Send(message);
			smtp.Dispose();
			message.Dispose();
		}


	}
}