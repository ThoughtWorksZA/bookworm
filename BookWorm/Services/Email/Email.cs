using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace BookWorm.Services.Email
{
    public class Email : IEmail
    {
        public const string ConfirmationEmailSubject = "The administror of PUKU created a user for you";
        private const string Template = @"Dear User,
The administrator of PUKU has created an account for you. To complete the registration process click on this link
http://puku.co.za/Account/RegisterConfirmation/{0}";

        public void SendConfirmation(string from, string to, string securityToken)
        {
            var client = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential("pukusite@gmail.com", "b00ksRc00l")
                };

            var mm = new MailMessage(from, to, ConfirmationEmailSubject, string.Format(Template, securityToken))
                {
                    BodyEncoding = Encoding.UTF8,
                    DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
                };
            client.Send(mm);
        }
    }
}