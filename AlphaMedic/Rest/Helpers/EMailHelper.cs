using Rest.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Rest.Helpers
{
    public class EMailHelper
    {
        private const string Email = "andrii.havryliuk@edvantis.com";
        private const string fromPassword = "Andrew12";

        public static void SendNotification(EmailInput emailInput)
        {
            var fromAddress = new MailAddress(Email, "Alpha Medic");
            var toAddress = new MailAddress(emailInput.Email, emailInput.UserName);

            var smtp = new SmtpClient
            {
                Host = "smtp.office365.com",
                Port = 587,
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
            var fromAddress = new MailAddress(Email, "Alpha Medic");
            var toAddress = new MailAddress(emailInput.Email, emailInput.UserName);
            string html = "Please confirm your account by clicking this link: <a href=\"" + Constants.MyClient 
                + "#/confirmRegistration/" + idUser + "\">link</a><br/>";

            var smtp = new SmtpClient
            {
                Host = "smtp.office365.com",
                Port = 587,
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